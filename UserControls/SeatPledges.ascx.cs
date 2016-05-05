using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Portal.UI;
using Arena.Custom.SOTHC.MiTM;
using Arena.Custom.SOTHC.MiTM.DataLayer;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class SeatPledges : PortalControl
    {
        #region Private Properties

        private string _editorPage;

        #endregion


        #region Module Settings

        [SmartPageSetting("Seat Pledge Editor", "The page that will be used for editing existing/new seat pledges.", "UserControls/Custom/SOTHC/MiTM/EditSeatPledge.ascx", RelatedModuleLocation.Beneath)]
        public string SeatPledgeEditorSetting { get { return _editorPage; } set { _editorPage = value; } }

        [PageSetting("Dedication Editor", "The page that will be used for editing associated dedications.", false)]
        public string DedicationEditorSetting { get { return Setting("DedicationEditor", "", false); } }

        #endregion


        /// <summary>
        /// Initial page load (or possibly postback). Prepare the initial information for the
        /// filter.
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //
                // Show/hide columns.
                //
                foreach (DataGridColumn dgc in dgResults.Columns)
                {
                    if (dgc.HeaderText == "Dedication")
                        dgc.Visible = !String.IsNullOrEmpty(DedicationEditorSetting);
                }

                //
                // Set initial filter options.
                //
                if (!String.IsNullOrEmpty(Request.QueryString["section"]))
                    txtAssignedSection.Text = Request.QueryString["section"];
                if (!String.IsNullOrEmpty(Request.QueryString["assigned_seat"]))
                    txtAssignedSeat.Text = Request.QueryString["assigned_seat"];
                if (!String.IsNullOrEmpty(Request.QueryString["unassigned"]))
                    cbUnassignedOnly.Checked = true;

                btnApplyFilter_Click(this, null);
            }
        }



        protected void Page_Init(object sender, EventArgs e)
        {
            dgResults.EditCommand += new DataGridCommandEventHandler(dgResults_Edit);
    dgResults.ReBind += new DataGridReBindEventHandler(dgResults_ReBind);
    dgResults.AddItem += new AddItemEventHandler(dgResults_Add);
    dgResults.DeleteCommand += new DataGridCommandEventHandler(dgResults_Delete);
    dgResults.ItemDataBound += new DataGridItemEventHandler(dgResults_ItemDataBound);
        }


        /// <summary>
        /// User wants to modify the filter.
        /// </summary>
        protected void btnApplyFilter_Click(object sender, EventArgs e)
        {
            ShowResults();
        }


        private void ShowResults()
        {
            String section = txtAssignedSection.Text;
            int seatNumber = -1;


            if (txtAssignedSeat.Text.Length >= 2)
            {
                section = txtAssignedSeat.Text.Substring(0, 1).ToUpper();
                seatNumber = Convert.ToInt32(txtAssignedSeat.Text.Substring(1));
            }

            dgResults.AllowSorting = true;
            dgResults.AllowPaging = true;
            dgResults.AddEnabled = true;
            dgResults.AddImageUrl = "~/Images/add_activity.gif";
            dgResults.BulkUpdateEnabled = false;
            dgResults.DeleteEnabled = true;
            dgResults.ExportEnabled = true;
            dgResults.MergeEnabled = false;
            dgResults.ShowFooter = true;
            dgResults.ShowHeader = true;
            dgResults.EditEnabled = true;
            dgResults.EditColumnText = "Edit";
            dgResults.ItemType = "Seat Pledge";

            dgResults.DataSource = new SeatPledgeData().GetSeatPledge_DT(
                section,
                seatNumber,
                cbUnassignedOnly.Checked);

            dgResults.DataBind();
        }


        protected void dgResults_ReBind(object sender, EventArgs e)
        {
            ShowResults();
        }


        /// <summary>
        /// Redirect the user to the "Edit Seat Pledge" page defined in the module settings in
        /// order to add a new seat pledge.
        /// </summary>
        protected void dgResults_Add(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("default.aspx?page={0}", SeatPledgeEditorSetting), true);
        }

        
        /// <summary>
        /// Redirect the user to the "Edit Seat Pledge" page to edit the selected seat pledge.
        /// </summary>
        protected void dgResults_Edit(object sender, DataGridCommandEventArgs e)
        {
            Response.Redirect(String.Format("default.aspx?page={0}&seat_pledge_id={1}", SeatPledgeEditorSetting, e.Item.Cells[0].Text), true);
        }


        /// <summary>
        /// Delete the pledge from the system.
        /// </summary>
        protected void dgResults_Delete(object sender, DataGridCommandEventArgs e)
        {
            SeatPledge.Delete(Convert.ToInt32(e.Item.Cells[0].Text));
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
                        if (!String.IsNullOrEmpty(DedicationEditorSetting))
                        {
                            label = (Label)e.Item.FindControl("lblDedication");
                            label.Text = String.Format("<a href=\"default.aspx?page={0}&seat_pledge_id={1}\">Edit Dedication</a>",
                                DedicationEditorSetting, dataItem["seat_pledge_id"]);
                        }

                        //
                        // Check if we are balanced with the pledge system.
                        //
                        label = (Label)e.Item.FindControl("lblUnbalanced");
                        if (Convert.ToBoolean(dataItem["is_balanced"]) == true)
                        {
                            label.Text = "";
                        }
                        else
                        {
                            label.Text = String.Format("<img src=\"{0}\">", ResolveUrl("~/images/cancel.gif"));
                        }

                        break;
                    }
            }
        }
    }
}