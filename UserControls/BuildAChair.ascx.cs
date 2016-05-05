using System;
using System.Collections.Generic;
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
	public partial class BuildAChair : PortalControl
	{
        #region Private Properties

        private string _dedicationPage;

        #endregion


        #region Module Settings

        [SmartPageSetting("Dedication Page", "The page for editing a dedication, which also creates all the records.", "UserControls/Custom/SOTHC/MiTM/EditDedication.ascx", RelatedModuleLocation.Beneath)]
        public string DedicationPageSetting { get { return _dedicationPage; } set { _dedicationPage = value; } }

		#endregion


		protected void Page_Load(object sender, System.EventArgs e)
		{
		}


        protected void btnAdopt_Click(object sender, System.EventArgs e)
        {
            if (String.IsNullOrEmpty(Request.QueryString["seat_id"]))
                throw new System.Exception("No seat specified.");

            Session["mitm_bac_seat_id"] = Request.QueryString["seat_id"];
            Session["mitm_bac_choices"] = hfChoices.Value;
            Session["mitm_bac_amount"] = hfAmount.Value;

            Response.Redirect(String.Format("default.aspx?page={0}", DedicationPageSetting), false);
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
