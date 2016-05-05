using System;
using System.Text;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

using Arena.Portal;
using Arena.Custom.SOTHC.MiTM;

namespace ArenaWeb.UserControls.Custom.MITM
{
	public partial class AdoptRemainingSeat : PortalControl
    {
        #region Private Properties

        SeatPledge _seatPledge = null;
        
        #endregion


        #region Module Settings

        [PageSetting("Redirect Page", "The page that the user will be redirected to after the e-mail is sent.", true)]
        public string RedirectPageSetting { get { return Setting("RedirectPage", "", true); } }

        [PageSetting("Edit Seat Pledge Page", "The page that the admin will be linked to for editing the seat pledge amount.", true)]
        public string EditSeatPledgePageSetting { get { return Setting("EditSeatPledgePage", "", true); } }

        [TextSetting("Admin Email", "The e-mail address that will receive e-mail alerts about the request to have the pledge increased.", true)]
        public string AdminEmailSetting { get { return Setting("AdminEmail", "", true); } }

        #endregion


        /// <summary>
        /// Initialize variables that will be needed later during the lifespan of this module.
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // We only reference the _seatPledge so just set it to null if we were
            // told to reference one that we can't find.
            //
            if (!String.IsNullOrEmpty(Request.QueryString["seat_pledge_id"]))
            {
                _seatPledge = new SeatPledge(Convert.ToInt32(Request.QueryString["seat_pledge_id"]));
                if (_seatPledge.SeatPledgeID == -1)
                    _seatPledge = null;
            }

        }


        /// <summary>
        /// The page is loading, make sure everything set correctly to some sane defaults the
        /// first time.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                //
                // Verify that we got a valid seat pledge to work with.
                //
                if (_seatPledge == null)
                {
                    throw new System.Exception("Seat pledge must be specified.");
                }

                //
                // Verify the user has permission to edit the current pledge.
                //
                if (CurrentArenaContext.Person == null || _seatPledge.PersonID != CurrentArenaContext.Person.PersonID)
                {
                    throw new System.Exception("Attempt to edit pledge for seat not owned by current user.");
                }

                //
                // Determine how much money is left.
                //
                SeatPledgeCollection spc = new SeatPledgeCollection();
                Decimal totalGift = 0;
                spc.LoadByAssignedSeat(_seatPledge.AssignedSeatID);
                foreach (SeatPledge sp in spc)
                {
                    totalGift += sp.Amount;
                }
                hfAmount.Value = (10000 - totalGift).ToString("C");
                
                ltAlready.Text = _seatPledge.Amount.ToString("C");
                ltNeeded.Text = hfAmount.Value;

                hlCancel.NavigateUrl = "~/" + Request.QueryString["redirect"];
            }
		}


        /// <summary>
        /// User is ready to save the dedication information.
        /// </summary>
        protected void btnAdopt_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine(String.Format("{0} has submitted a request to increase their pledge.<br />", CurrentArenaContext.Person.FullName));
            sb.AppendLine("<br />");

            sb.AppendLine(String.Format("Seat: {0}{1}<br />", _seatPledge.AssignedSeat.Section, _seatPledge.AssignedSeat.Number.ToString()));
            sb.AppendLine(String.Format("Increase by: {0} (currently {1})<br />", hfAmount.Value, _seatPledge.Amount.ToString("C")));
            sb.AppendLine("<br />");


            sb.AppendLine(String.Format("Click <a href=\"https://arena.shepnet.org/default.aspx?page={0}&amp;seat_pledge_id={1}\">here</a> to update their seat pledge.<br />", EditSeatPledgePageSetting, _seatPledge.SeatPledgeID.ToString()));
            sb.AppendLine("<br />");
            
            sb.AppendLine("Please remember to update their contributions pledge record as well.<br />");

            new Arena.Utility.ArenaSendMail().SendMail(
                CurrentOrganization.Settings["OrganizationEmail"],
                "Miracle In The Making",
                AdminEmailSetting,
                String.Empty,
                String.Empty,
                String.Empty,
                "Request to increase pledge",
                sb.ToString(),
                String.Empty);
            
            //
            // Redirect to the target page.
            //
            Response.Redirect(String.Format("default.aspx?page={0}", RedirectPageSetting), true);
        }


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

	}
}
