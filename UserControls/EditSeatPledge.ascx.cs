using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Custom.SOTHC.MiTM;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class EditSeatPledge : PortalControl
    {
        #region Private Properties

        private SeatPledge seatPledge = null;

        private string _listPage;

        #endregion


        #region Module Settings

        [SmartPageSetting("Seat Pledge List", "The page that will be used for listing existing seat pledges.", "UserControls/Custom/SOTHC/MiTM/SeatPledges.ascx", RelatedModuleLocation.Above)]
        public string SeatPledgeListSetting { get { return _listPage; } set { _listPage = value; } }

        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // Load the current seatPledge in memory for use throughout the control.
            //
            if (!String.IsNullOrEmpty(Request.QueryString["seat_pledge_id"]))
            {
                seatPledge = new SeatPledge(Convert.ToInt32(Request.QueryString["seat_pledge_id"]));
                if (seatPledge.SeatPledgeID == -1)
                    throw new System.Exception("Invalid seat pledge record specified.");
            }
            else
                seatPledge = new SeatPledge();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Boolean isNew;

                //
                // Set initial values.
                //
                psPerson.PersonID = seatPledge.PersonID;
                txtAmount.Text = seatPledge.Amount.ToString("######");
                if (seatPledge.AssignedSeat.SeatID != -1)
                    txtAssignedSeat.Text = seatPledge.AssignedSeat.Section + seatPledge.AssignedSeat.Number.ToString();
                if (seatPledge.RequestedSeat.SeatID != -1)
                    txtReqSeat.Text = seatPledge.RequestedSeat.Section + seatPledge.RequestedSeat.Number.ToString();
                cbReqFullSeat.Checked = (seatPledge.RequestedFullSeat != 0);
                cbReqBackRest.Checked = (seatPledge.RequestedBackRest != 0);
                cbReqLeg1.Checked = (seatPledge.RequestedLeg1 != 0);
                cbReqLeg2.Checked = (seatPledge.RequestedLeg2 != 0);
                cbReqLeg3.Checked = (seatPledge.RequestedLeg3 != 0);
                cbReqLeg4.Checked = (seatPledge.RequestedLeg4 != 0);
                cbReqArmLeft.Checked = (seatPledge.RequestedArmLeft != 0);
                cbReqArmRight.Checked = (seatPledge.RequestedArmRight != 0);

                //
                // Set enabled/disabled states.
                //
                isNew = (seatPledge.SeatPledgeID == -1);
                txtReqSeat.Enabled = isNew;
                cbReqFullSeat.Enabled = isNew;
                cbReqBackRest.Enabled = isNew;
                cbReqLeg1.Enabled = isNew;
                cbReqLeg2.Enabled = isNew;
                cbReqLeg3.Enabled = isNew;
                cbReqLeg4.Enabled = isNew;
                cbReqArmLeft.Enabled = isNew;
                cbReqArmRight.Enabled = isNew;
            }

            //
            // Reset error text.
            //
            lbPersonError.Text = "";
            lbAmountError.Text = "";
            lbAutoSeatError.Text = "";
            lbAutoSeatMsg.Text = "";
            lbReqSeatError.Text = "";
        }


        /// <summary>
        /// Automatically calculate the amount of money based on the selection in the
        /// requested check-boxes.
        /// </summary>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            Decimal amount = 0;


            amount += (cbReqFullSeat.Checked ? 10000 : 0);
            amount += (cbReqBackRest.Checked ? 5000 : 0);
            amount += (cbReqLeg1.Checked ? 1000 : 0);
            amount += (cbReqLeg2.Checked ? 1000 : 0);
            amount += (cbReqLeg3.Checked ? 1000 : 0);
            amount += (cbReqLeg4.Checked ? 1000 : 0);
            amount += (cbReqArmLeft.Checked ? 500 : 0);
            amount += (cbReqArmRight.Checked ? 500 : 0);

            txtAmount.Text = amount.ToString("######");
        }


        /// <summary>
        /// Automatically assign a seat to this pledge based on the following order:
        /// 1) The currently assigned seat (if any and if available).
        /// 2) The requested seat (if available).
        /// 3) The next available seat.
        /// </summary>
        protected void btnAutoSeat_Click(object sender, EventArgs e)
        {
            Seat seat = null;


            if (txtAmount.Text.Length == 0 || Convert.ToInt32(txtAmount.Text) == 0)
            {
                lbAutoSeatError.Text = "You must specify an amount first.";
                return;
            }

            //
            // Check the currently assigned seat.
            //
            if (seatPledge.AssignedSeatID != -1)
            {
                SeatPledgeCollection spc = new SeatPledgeCollection();
                Decimal amount = 0;
                
                spc.LoadByAssignedSeat(seatPledge.AssignedSeatID);
                foreach (SeatPledge sp in spc)
                {
                    if (sp.SeatPledgeID != seatPledge.SeatPledgeID)
                        amount += sp.Amount;
                }
                amount += Convert.ToInt32(txtAmount.Text);

                //
                // If the existing pledge amounts and the current amount are less than
                // 10,000 then we are good.
                //
                if (amount <= 10000)
                {
                    lbAutoSeatMsg.Text = "Existing seat will be reused.";
                    return;
                }
            }

            //
            // Check to make sure a requested seat has been specified.
            //
            if (txtReqSeat.Text.Length >= 2)
            {
                SeatPledgeCollection spc = new SeatPledgeCollection();
                Decimal amount = 0;

                //
                // Walk through all the pledges on the requested seat and add them up.
                //
                seat = new Seat(txtReqSeat.Text.Substring(0, 1).ToUpper(), Convert.ToInt32(txtReqSeat.Text.Substring(1)));
                spc.LoadByAssignedSeat(seat.SeatID);
                foreach (SeatPledge sp in spc)
                {
                    if (sp.SeatPledgeID != seatPledge.SeatPledgeID)
                        amount += sp.Amount;
                }
                amount += Convert.ToInt32(txtAmount.Text);

                //
                // If the existing pledge amounts and the current amount are less than
                // 10,000 then we are good.
                //
                if (amount <= 10000)
                {
                    txtAssignedSeat.Text = seat.Section + seat.Number.ToString();
                    lbAutoSeatMsg.Text = "Requested seat will be assigned.";
                    return;
                }
            }

            //
            // ... Otherwise, look for the next available seat.
            //
            seat = Seat.NextAvailableSeat(Convert.ToInt32(txtAmount.Text), seatPledge.SeatPledgeID);
            if (seat != null)
            {
                txtAssignedSeat.Text = seat.Section + seat.Number.ToString();
                lbAutoSeatMsg.Text = "The seat " + txtAssignedSeat.Text + " will be assigned.";
                return;
            }

            //
            // Could not find any seats.
            //
            lbAutoSeatError.Text = "Could not find an available seat to assign. This is either a really good thing or it means you might have entered more than $10,000 in the amount field.";
        }


        /// <summary>
        /// Save the SeatPledge object to the database.
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool requiredMissing = false;


            //
            // Check to make sure all the required fields have been entered. This could be done
            // with client-side validation, but the .NET validation stuff is a pain and ugly.
            //
            if (psPerson.PersonID == -1)
            {
                lbPersonError.Text = "Person is a required field.";
                requiredMissing = true;
            }
            if (txtAmount.Text.Length < 1 || Convert.ToDecimal(txtAmount.Text) == 0)
            {
                lbAmountError.Text = "Amount is a required field.";
                requiredMissing = true;
            }
            if (txtAssignedSeat.Text.Length < 2)
            {
                lbAutoSeatError.Text = "Assigned Seat is a required field.";
                requiredMissing = true;
            }
            if (txtReqSeat.Text.Length < 2)
            {
                lbReqSeatError.Text = "Requested Seat is a required field.";
                requiredMissing = true;
            }
            if (requiredMissing)
                return;

            //
            // Save all the values the user supplied into the object.
            //
            seatPledge.PersonID = psPerson.PersonID;
            seatPledge.Amount = Convert.ToDecimal(txtAmount.Text);
            seatPledge.AssignedSeatID = new Seat(txtAssignedSeat.Text.Substring(0, 1).ToUpper(), Convert.ToInt32(txtAssignedSeat.Text.Substring(1))).SeatID;
            seatPledge.RequestedSeatID = new Seat(txtReqSeat.Text.Substring(0, 1).ToUpper(), Convert.ToInt32(txtReqSeat.Text.Substring(1))).SeatID;
            seatPledge.RequestedFullSeat = (cbReqFullSeat.Checked ? 1 : 0);
            seatPledge.RequestedBackRest = (cbReqBackRest.Checked ? 1 : 0);
            seatPledge.RequestedLeg1 = (cbReqLeg1.Checked ? 1 : 0);
            seatPledge.RequestedLeg2 = (cbReqLeg2.Checked ? 1 : 0);
            seatPledge.RequestedLeg3 = (cbReqLeg3.Checked ? 1 : 0);
            seatPledge.RequestedLeg4 = (cbReqLeg4.Checked ? 1 : 0);
            seatPledge.RequestedArmLeft = (cbReqArmLeft.Checked ? 1 : 0);
            seatPledge.RequestedArmRight = (cbReqArmRight.Checked ? 1 : 0);

            //
            // Save the object to the database.
            //
            seatPledge.Save(ArenaContext.Current.User.Identity.Name);

            //
            // Redirect to the list page.
            //
            Response.Redirect(String.Format("default.aspx?page={0}", SeatPledgeListSetting), true);
        }
    }
}