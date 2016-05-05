namespace ArenaWeb.Templates
{
	using System;
	using System.Text;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.IO;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Arena.Portal;

	/// <summary>
	///		Summary description for ArenaMain.
	/// </summary>
	public partial class miracleinthemaking : PortalControl
	{
		#region Template Settings

		// Template Settings
		[SettingAttribute("Show Heading", "Optional Flag indicating if the Page Heading should be displayed on this page (True*/False).", false)]
		public string ShowHeadingSetting { get { return CurrentPortalPage.Setting("ShowHeading", "true", false); } }

		//[ModuleAttribute("Show Bread Crumbs", "Optional Flag indicating if the Bread Crumb Trail should be displayed on this page (True*/False).", false)]
		//public string ShowBreadCrumbsSetting { get { return CurrentPortalPage.Setting("ShowBreadCrumbs", "true", false); } }

        	//[ModuleAttribute("Email Subject", "Optional text of the email when a user click on the Email This Page link. If nothing, the email link will not display.", false)]
       	//public string emailSubjectSetting { get { return CurrentPortalPage.Setting("emailSubject", "", false); } }

		#endregion

		#region Content Areas

		public System.Web.UI.WebControls.PlaceHolder Top { get { return TopPane; } }
		public System.Web.UI.WebControls.PlaceHolder Main { get { return MainPane; } }
		public System.Web.UI.WebControls.PlaceHolder Bottom { get { return BottomPane; } }

		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection attributes = new System.Collections.Specialized.NameValueCollection();
			attributes.Add("rel", "shortcut icon");
			attributes.Add("href", "~/images/miracleinthemaking/favicon.ico");
			Utilities.AddHeadLink(Page, attributes);
			
			//phBreadCrumbs.Controls.Clear();
			//phBreadCrumbs.Controls.Add(new LiteralControl(CurrentPortalPage.BreadCrumbs.ToString()));
            //if (emailSubjectSetting != "")
            {
                //aMailThisPage.HRef = string.Format("mailto:?subject={0}&body={1}", HttpUtility.UrlEncode(emailSubjectSetting), HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
                //aMailThisPage.Visible = true;
            }
            //else
                //aMailThisPage.Visible = false;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			//tblTitle.Visible = (ShowHeadingSetting.ToLower() != "false");
			//trTitle.Visible = tblTitle.Visible;
			//lblPageTitle.Text = this.Title;

			//phBreadCrumbs.Visible = (ShowBreadCrumbsSetting.ToLower() != "false");

			base.Render(writer);
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
