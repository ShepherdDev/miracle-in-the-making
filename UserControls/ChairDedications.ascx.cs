using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Custom.SOTHC.MiTM.DataLayer;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class ChairDedications : PortalControl
    {
        #region Private Properties

        private string _editorPage;

        #endregion


        #region Module Settings

        [SmartPageSetting("Dedication Editor", "The page that will be used for editing existing dedications.", "UserControls/Custom/SOTHC/MiTM/EditDedication.ascx", RelatedModuleLocation.Beneath)]
        public string DedicationEditorSetting { get { return _editorPage; } set { _editorPage = value; } }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            //
            // Set initial filter options.
            //
            if (!String.IsNullOrEmpty(Request.QueryString["approved"]))
                ddlApprovedStatus.SelectedValue = Request.QueryString["approved"];

            btnApplyFilter_Click(this, null);
        }


        protected void dgResults_ReBind(object sender, EventArgs e)
        {
            ShowResults();
        }


        protected void dgResults_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            switch (e.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    {
                        DataRowView dataItem = (DataRowView)e.Item.DataItem;
                        Label label;

                        //
                        // Set is_approved status.
                        //
                        label = (Label)e.Item.FindControl("lblApproved");
                        if (Convert.ToBoolean(dataItem["is_approved"]) != true)
                        {
                            label.Text = "";
                        }
                        else
                        {
                            label.Text = string.Format("<img src='{0}' border='0'>", ResolveUrl("~/images/check.gif"));
                        }

                        //
                        // Set anonymous status.
                        //
                        label = (Label)e.Item.FindControl("lblAnonymous");
                        if (Convert.ToBoolean(dataItem["anonymous"]) != true)
                        {
                            label.Text = "";
                        }
                        else
                        {
                            label.Text = string.Format("<img src='{0}' border='0'>", base.ResolveUrl("~/images/check.gif"));
                        }

                        break;
                    }
            }
        }


        /// <summary>
        /// User wants to modify the filter.
        /// </summary>
        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ShowResults();
        }


        /// <summary>
        /// Redirect the user to the "Edit Seat Pledge" page to edit the selected seat pledge.
        /// </summary>
        protected void dgResults_Edit(object sender, DataGridCommandEventArgs e)
        {
            Response.Redirect(String.Format("default.aspx?page={0}&seat_pledge_id={1}", DedicationEditorSetting, e.Item.Cells[0].Text), true);
        }


        private void ShowResults()
        {
            dgResults.DataSource = new DedicationData().GetDedication_DT(
                Convert.ToInt32(ddlApprovedStatus.SelectedValue)
                );

            dgResults.DataBind();
        }
    }
}