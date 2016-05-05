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
using Arena.Security;
using Arena.Utility;
using Arena.Custom.SOTHC.MiTM;

namespace ArenaWeb.UserControls.Custom.MITM
{
	public partial class EditDedication : PortalControl
    {
        #region Private Properties

        private Boolean canEdit = false, canApprove = false;
        private SeatPledge _seatPledge;
        private string _listPage;

        #endregion


        #region Module Settings

        [SmartPageSetting("Dedication List", "The page that will be used for listing existing dedications.", "UserControls/Custom/SOTHC/MiTM/ChairDedications.ascx", RelatedModuleLocation.Above, false)]
        public string DedicationListSetting { get { return _listPage; } set { _listPage = value; } }

        [PageSetting("Redirect Page", "The page that the user will be redirected to after saving their dedication. If not set then the DedicationList setting is used.", false)]
        public string RedirectPageSetting { get { return Setting("RedirectPage", "", false); } }

        [TextSetting("Mail From Email", "The e-mail address that will be used when sending the user an e-mail. If empty no message will be sent to the user.", false)]
        public string MailFromEmailSetting { get { return Setting("MailFromEmail", "", false); } }

        [TextSetting("Mail From Name", "The name that will be used when sending the user an e-mail. If empty no message will be sent to the user.", false)]
        public string MailFromNameSetting { get { return Setting("MailFromName", "", false); } }

        [TextSetting("Mail Subject", "The subject of the e-mail to use when sending a thank you to the user. If empty no message will be sent to the user.", false)]
        public string MailSubjectSetting { get { return Setting("MailSubject", "", false); } }

        [TextSetting("Mail Body", "The message body to use when sending a thank you to the user. If empty no message will be sent to the user. {0} will be replaced with the person's nickname.", false)]
        public string MailBodySetting { get { return Setting("MailBody", "", false); } }

        [TextSetting("Admin Email", "The e-mail address that will receive e-mail alerts about activity requiring approval. If empty no message will be sent to the admin user.", false)]
        public string AdminEmailSetting { get { return Setting("AdminEmail", "", false); } }

        [PageSetting("List Seat Pledges Page", "The page that lists the existing seat pledges in the system.", false)]
        public string ListSeatPledgesPageSetting { get { return Setting("ListSeatPledgesPage", "", false); } }

        [PageSetting("List Dedications Page", "The page that lists the existing dedications in the system.", false)]
        public string ListDedicationsPageSetting { get { return Setting("ListDedicationsPage", "", false); } }

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
            else if (!String.IsNullOrEmpty((String)Session["mitm_bac_choices"]) &&
                !String.IsNullOrEmpty((String)Session["mitm_bac_seat_id"]) &&
                !String.IsNullOrEmpty((String)Session["mitm_bac_amount"]))
            {
                _seatPledge = new SeatPledge();
            }

            //
            // Determine what permissions the current user has to the module.
            //
            canEdit = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
            canApprove = CurrentModule.Permissions.Allowed(OperationType.Approve, CurrentUser);
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
                    if (true)
                    {
                        StringBuilder sb = new StringBuilder();

                        if (!String.IsNullOrEmpty((String)Session["mitm_bac_seat_id"]))
                            sb.Append("id=" + (String)Session["mitm_bac_seat_id"]);
                        else
                            sb.Append("id=null");
                        if (!String.IsNullOrEmpty((String)Session["mitm_bac_choices"]))
                            sb.Append("choices=" + (String)Session["mitm_bac_choices"]);
                        else
                            sb.Append("choices=null");
                        if (!String.IsNullOrEmpty((String)Session["mitm_bac_amount"]))
                            sb.Append("amount=" + (String)Session["mitm_bac_amount"]);
                        else
                            sb.Append("amount=null");
                        tbBiography.Text = sb.ToString();
                        return;
                    }
                    throw new System.Exception("Seat pledge must be specified.");
                }

                //
                // Verify the user has permission to edit the current dedication.
                //
                if (_seatPledge.PersonID != -1 && canEdit == false && (CurrentArenaContext.Person == null || _seatPledge.PersonID != CurrentArenaContext.Person.PersonID))
                {
                    throw new System.Exception("Attempt to edit dedication for seat not owned by current user.");
                }

                //
                // Set default values.
                //
                tbDedication.Text = _seatPledge.Dedication.DedicatedTo;
                tbSponsor.Text = _seatPledge.Dedication.SponsoredBy;
                tbBiography.Text = _seatPledge.Dedication.Biography;
                cbAnonymous.Checked = _seatPledge.Dedication.Anonymous;
                cbApproved.Checked = !String.IsNullOrEmpty(_seatPledge.Dedication.ApprovedBy);

                //
                // Show the proper image information.
                //
                if (_seatPledge.Dedication.BlobID != -1)
                {
                    ltPicture.Text = String.Format("<a href=\"CachedBlob.aspx?guid={0}\" target=\"_blank\"><img src=\"CachedBlob.aspx?guid={0}&width=240\" /></a>", _seatPledge.Dedication.Blob.GUID.ToString());
                    lbChangePicture.Visible = true;
                }

                //
                // Hide the Approved checkbox if they don't have access to approve items.
                //
                pnlApproved.Visible = canApprove;
            }
		}


        /// <summary>
        /// User is ready to save the dedication information.
        /// </summary>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Boolean newSeatPledge, newDedication;


            //
            // Determine if these are new items being created.
            //
            newSeatPledge = (_seatPledge.SeatPledgeID == -1);
            newDedication = (_seatPledge.Dedication.DedicationID == -1);

            //
            // Create the seat pledge if we need to.
            //
            if (_seatPledge.SeatPledgeID == -1)
            {
                String[] choices = Session["mitm_bac_choices"].ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                _seatPledge.PersonID = CurrentArenaContext.Person.PersonID;
                _seatPledge.RequestedSeatID = Convert.ToInt32(Session["mitm_bac_seat_id"]);
                _seatPledge.Amount = Convert.ToInt32(Session["mitm_bac_amount"]);

                foreach (String choice in choices)
                {
                    String[] option = choice.Split(new char[] { '=' });
                    String element = option[0];
                    int count = Convert.ToInt32(option[1]);

                    if (element.Equals("full_seat"))
                        _seatPledge.RequestedFullSeat = count;
                    else if (element.Equals("back_rest"))
                        _seatPledge.RequestedBackRest = count;
                    else if (element.Equals("leg1"))
                        _seatPledge.RequestedLeg1 = count;
                    else if (element.Equals("leg2"))
                        _seatPledge.RequestedLeg2 = count;
                    else if (element.Equals("leg3"))
                        _seatPledge.RequestedLeg3 = count;
                    else if (element.Equals("leg4"))
                        _seatPledge.RequestedLeg4 = count;
                    else if (element.Equals("arm_left"))
                        _seatPledge.RequestedArmLeft = count;
                    else if (element.Equals("arm_right"))
                        _seatPledge.RequestedArmRight = count;
                }

                _seatPledge.Save(CurrentUser.Identity.Name);
            }

            //
            // Prep all the basic information for the dedication.
            //
            _seatPledge.Dedication.DedicatedTo = tbDedication.Text.Trim();
            _seatPledge.Dedication.SponsoredBy = tbSponsor.Text.Trim();
            _seatPledge.Dedication.Biography = tbBiography.Text.Trim();
            _seatPledge.Dedication.Anonymous = cbAnonymous.Checked;

            //
            // If they uploaded a picture then "deal with it".
            //
            if (fuPicture.HasFile)
            {
                if (_seatPledge.Dedication.BlobID != -1)
                {
                    //
                    // Replace the existing picture.
                    //
                    _seatPledge.Dedication.Blob.ByteArray = fuPicture.FileBytes;
                    _seatPledge.Dedication.Blob.SetFileInfo(fuPicture.FileName);

                    _seatPledge.Dedication.Blob.Save(CurrentUser.Identity.Name);
                }
                else
                {
                    ArenaImage blob = new ArenaImage();

                    //
                    // Create a new image.
                    //
                    blob.ByteArray = fuPicture.FileBytes;
                    blob.SetFileInfo(fuPicture.FileName);
                    blob.Save(CurrentUser.Identity.Name);

                    _seatPledge.Dedication.Blob = blob;
                }
            }

            //
            // If the person editing this is not an approver, then mark it non-approved.
            //
            if (canApprove == true && cbApproved.Checked == true)
                _seatPledge.Dedication.ApprovedBy = CurrentUser.Identity.Name;
            else
                _seatPledge.Dedication.ApprovedBy = String.Empty;

            //
            // Save the dedication.
            //
            _seatPledge.Dedication.Save(CurrentUser.Identity.Name);

            //
            // Send out an e-mail to the user, if they have an e-mail address, thanking
            // them for their pledge.
            //
            if (!String.IsNullOrEmpty(MailFromEmailSetting) && !String.IsNullOrEmpty(MailFromNameSetting) &&
                !String.IsNullOrEmpty(MailSubjectSetting) && !String.IsNullOrEmpty(MailBodySetting) &&
                newSeatPledge && !String.IsNullOrEmpty(CurrentArenaContext.Person.Emails.FirstActive))
            {
                new Arena.Utility.ArenaSendMail().SendMail(
                    MailFromEmailSetting,
                    MailFromNameSetting,
                    CurrentArenaContext.Person.Emails.FirstActive,
                    String.Empty,
                    String.Empty,
                    String.Empty,
                    MailSubjectSetting,
                    String.Format(MailBodySetting, CurrentArenaContext.Person.NickName),
                    String.Empty);
            }

            //
            // Send out an e-mail to the admin user if this is a new seat pledge and/or
            // new dedication message. Also send an e-mail if the user updated a dedication
            // and it now needs to be approved again.
            //
            if (!String.IsNullOrEmpty(AdminEmailSetting))
            {
                if (newSeatPledge || (newDedication || String.IsNullOrEmpty(_seatPledge.Dedication.ApprovedBy)))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine(String.Format("New pledge activity for {0}.<br />", CurrentArenaContext.Person.FullName));
                    sb.AppendLine("<br />");

                    if (newSeatPledge)
                    {
                        sb.AppendLine("A new seat pledge has been entered into the system and is waiting seat assignment.<br />");
                        if (!String.IsNullOrEmpty(ListSeatPledgesPageSetting))
                            sb.AppendLine(String.Format("Click <a href=\"https://arena.shepnet.org/default.aspx?page={0}&amp;unassigned=1\">here</a> to view unassigned seat pledges.<br />", ListSeatPledgesPageSetting));
                        sb.AppendLine("<br />");
                    }

                    if (newDedication)
                    {
                        sb.AppendLine("A new seat dedication message has been entered into the system and is waiting approval.<br />");
                        if (!String.IsNullOrEmpty(ListDedicationsPageSetting))
                            sb.AppendLine(String.Format("Click <a href=\"https://arena.shepnet.org/default.aspx?page={0}&amp;approved=0\">here</a> to view unapproved dedications.<br />", ListDedicationsPageSetting));
                        sb.AppendLine("<br />");
                    }
                    else
                    {
                        sb.AppendLine("A seat dedication message has been updated and is waiting approval.<br />");
                        if (!String.IsNullOrEmpty(ListDedicationsPageSetting))
                            sb.AppendLine(String.Format("Click <a href=\"https://arena.shepnet.org/default.aspx?page={0}&amp;approved=0\">here</a> to view unapproved dedications.<br />", ListDedicationsPageSetting));
                        sb.AppendLine("<br />");
                    }

                    new Arena.Utility.ArenaSendMail().SendMail(
                        CurrentOrganization.Settings["OrganizationEmail"],
                        "Miracle In The Making",
                        AdminEmailSetting,
                        String.Empty,
                        String.Empty,
                        String.Empty,
                        "New Pledge/Dedication Activity",
                        sb.ToString(),
                        String.Empty);
                }
            }

            //
            // Erase Session data.
            //
            Session.Remove("mitm_bac_seat_id");
            Session.Remove("mitm_bac_choices");
            Session.Remove("mitm_bac_amount");

            //
            // Redirect the user to either the redirect page or the dedication list.
            //
            if (!String.IsNullOrEmpty(RedirectPageSetting))
                Response.Redirect(String.Format("default.aspx?page={0}", RedirectPageSetting), false);
            else
                Response.Redirect(String.Format("default.aspx?page={0}", DedicationListSetting), false);
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
