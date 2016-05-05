using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Custom.SOTHC.MiTM;
using Arena.Custom.SOTHC.MiTM.DataLayer;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class SalvationList : PortalControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentPerson.PersonID == -1)
                throw new System.Exception("Module requires user to be logged in.");

            if (!IsPostBack)
            {
                rptNames_Rebind();
            }
        }


        /// <summary>
        /// Add the given person to the list of people this person is praying for.
        /// </summary>
        protected void btnAdd_Submit(object sender, EventArgs e)
        {
            Salvation salvation = new Salvation();


            salvation.PersonID = CurrentPerson.PersonID;
            salvation.FirstName = tbFirstName.Text;
            salvation.LastName = tbLastName.Text;

            salvation.Save(CurrentUser.Identity.Name);

            tbFirstName.Text = String.Empty;
            tbLastName.Text = String.Empty;

            rptNames_Rebind();
        }


        /// <summary>
        /// Bind the repeater to the data.
        /// </summary>
        void rptNames_Rebind()
        {
            rptNames.DataSource = new SalvationData().GetSalvation_DT(CurrentPerson.PersonID);
            rptNames.DataBind();
        }


        protected void imgStatus_Click(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                Salvation salvation = new Salvation(Convert.ToInt32(e.CommandArgument));

                salvation.Status = !salvation.Status;
                salvation.Save(CurrentUser.Identity.Name);

                rptNames_Rebind();
            }
        }


        protected void rptNames_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                ImageButton img = (ImageButton)e.Item.FindControl("imgStatus");

                img.CommandArgument = row["salvation_id"].ToString();
                if (Convert.ToBoolean(row["status"]) == true)
                    img.ImageUrl = "Images/dove.gif";
                else
                    img.ImageUrl = "Images/cross.jpg";
            }
        }
    }
}