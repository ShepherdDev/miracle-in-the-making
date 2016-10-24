using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.shepherdchurch.MiracleInTheMaking.Data;
using com.shepherdchurch.MiracleInTheMaking.Model;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking
{
    [DisplayName( "Seat Pledge List" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Lists all the seat pledges." )]
    [LinkedPage( "Pledge Detail Page", "The page the user will be taken to when clicking on a row to edit the pledge." )]
    [LinkedPage( "Dedication Detail Page", "The page the user will be taken to when clicking on the dedication button." )]

    public partial class SeatPleadgeList : Rock.Web.UI.RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            bool canEdit = IsUserAuthorized( Rock.Security.Authorization.EDIT );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            gfSettings.ApplyFilterClick += gfSettings_ApplyFilterClick;
            gfSettings.DisplayFilterValue += gfSettings_DisplayFilterValue;

            gSeatPledges.RowItemText = "Seat Pledge";
            gSeatPledges.DataKeyNames = new string[] { "Id" };
            gSeatPledges.IsDeleteEnabled = canEdit;
            gSeatPledges.Actions.ShowAdd = canEdit;
            gSeatPledges.Actions.ShowBulkUpdate = false;
            gSeatPledges.Actions.ShowMergePerson = false;
            gSeatPledges.Actions.AddClick += gSeatPledges_Add;
            gSeatPledges.GridRebind += gSeatPledges_GridRebind;
            gSeatPledges.RowDataBound += gSeatPledges_RowDataBound;

            BindFilter();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                tbNameFilter.Text = gfSettings.GetUserPreference( "Name" );
                ddlAssignedSectionFilter.SelectedValue = gfSettings.GetUserPreference( "Assigned Section" );
                tbAssignedSeatFilter.Text = gfSettings.GetUserPreference( "Assigned Seat" );
                rblAssignedStatusFilter.SelectedValue = gfSettings.GetUserPreference( "Assigned Status" );

                BindGrid();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
        }

        /// <summary>
        /// Handles the ApplyFilterClick event of the gfSettings control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gfSettings_ApplyFilterClick( object sender, EventArgs e )
        {
            gfSettings.SaveUserPreference( "Name", tbNameFilter.Text );
            gfSettings.SaveUserPreference( "Assigned Section", ddlAssignedSectionFilter.SelectedValue );
            gfSettings.SaveUserPreference( "Assigned Seat", tbAssignedSeatFilter.Text );
            gfSettings.SaveUserPreference( "Assigned Status", rblAssignedStatusFilter.SelectedValue );

            BindGrid();
        }

        /// <summary>
        /// Gfs the settings_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void gfSettings_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Name":
                case "Assigned Section":
                case "Assigned Seat":
                    break;

                case "Assigned Status":
                    {
                        int? status = e.Value.AsIntegerOrNull();

                        e.Value = string.Empty;
                        if (status != null && status == 1)
                        {
                            e.Value = "Assigned";
                        }
                        else if (status != null && status == 2)
                        {
                            e.Value = "Unassigned";
                        }

                        break;
                    }

                default:
                    {
                        e.Value = string.Empty;
                        break;
                    }
            }

        }

        /// <summary>
        /// Handles the Add event of the gSeatPledges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gSeatPledges_Add( object sender, EventArgs e )
        {
            NavigateToDetailPage( 0 );
        }

        /// <summary>
        /// Handles the Edit event of the gSeatPledges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gSeatPledges_Edit( object sender, RowEventArgs e )
        {
            NavigateToDetailPage( e.RowKeyId );
        }

        /// <summary>
        /// Handles the LinkButton to edit a dedication in the gSeatPledges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gSeatPledges_EditDedication( object sender, RowEventArgs e )
        {
            //
            // Dedications are edited by passing the seat pledge ID.
            //
            NavigateToDedicationDetailPage( e.RowKeyId );
        }

        /// <summary>
        /// Handles the Delete event of the gSeatPledges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gSeatPledges_Delete( object sender, RowEventArgs e )
        {
            var dataContext = new MiracleInTheMakingContext();
            var service = new SeatPledgeService( dataContext );
            var seatPledge = service.Get( ( int )e.RowKeyValue );

            if ( seatPledge != null )
            {
                string errorMessage;
                if ( !service.CanDelete( seatPledge, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                service.Delete( seatPledge );
                dataContext.SaveChanges();
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gSeatPledges control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gSeatPledges_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        protected void gSeatPledges_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                SeatPledge seatPledge = ( SeatPledge )e.Row.DataItem;

                if ( !IsUserAuthorized( Rock.Security.Authorization.EDIT ) || !seatPledge.Dedications.Any() )
                {
                    var lb = e.Row.Cells.Cast<DataControlFieldCell>()
                        .Where( c => c.ContainingField.HeaderText == "Dedication" ).FirstOrDefault()
                        .ControlsOfTypeRecursive<LinkButton>().FirstOrDefault();

                    lb.Visible = false;
                }

                if ( !IsUserAuthorized( Rock.Security.Authorization.EDIT ) )
                {
                    var lb = e.Row.Cells.Cast<DataControlFieldCell>()
                        .Where( c => c.ContainingField.GetType() == typeof(DeleteField) ).FirstOrDefault()
                        .ControlsOfTypeRecursive<LinkButton>().FirstOrDefault();

                    lb.Visible = false;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the filter.
        /// </summary>
        private void BindFilter()
        {
            ddlAssignedSectionFilter.Items.Clear();
            ddlAssignedSectionFilter.DataSource = new SeatService( new MiracleInTheMakingContext() ).Queryable().GroupBy( s => s.Section, (k, g) => new { Section = k } ).ToList();
            ddlAssignedSectionFilter.DataTextField = "Section";
            ddlAssignedSectionFilter.DataValueField = "Section";
            ddlAssignedSectionFilter.DataBind();
            ddlAssignedSectionFilter.Items.Insert( 0, new ListItem() );
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var service = new SeatPledgeService( new MiracleInTheMakingContext() );
            SortProperty sortProperty = gSeatPledges.SortProperty;

            var qry = service.Queryable( "PledgedPersonAlias,RequestedSeat,AssignedSeat" );

            string nameFilter = gfSettings.GetUserPreference( "Name" );
            if ( !string.IsNullOrEmpty( nameFilter ) )
            {
                qry = qry.Where( sp => sp.PledgedPersonAlias.Person.FirstName.Contains( nameFilter ) || sp.PledgedPersonAlias.Person.NickName.Contains( nameFilter ) || sp.PledgedPersonAlias.Person.LastName.Contains( nameFilter ) );
            }

            string assignedSectionFilter = gfSettings.GetUserPreference( "Assigned Section" );
            if ( !string.IsNullOrEmpty( assignedSectionFilter ) )
            {
                qry = qry.Where( sp => sp.AssignedSeat != null && sp.AssignedSeat.Section == assignedSectionFilter );
            }

            int? assignedSeatFilter = gfSettings.GetUserPreference( "Assigned Seat" ).AsIntegerOrNull();
            if ( assignedSeatFilter != null )
            {
                qry = qry.Where( sp => sp.AssignedSeat != null && sp.AssignedSeat.SeatNumber == assignedSeatFilter );
            }

            int? assignedStatusFilter = gfSettings.GetUserPreference( "Assigned Status" ).AsIntegerOrNull();
            if ( assignedStatusFilter != null )
            {
                if ( assignedStatusFilter == 1 )
                {
                    qry = qry.Where( sp => sp.AssignedSeat != null );
                }
                else if ( assignedStatusFilter == 2 )
                {
                    qry = qry.Where( sp => sp.AssignedSeat == null );
                }
            }

            // Sort results
            if ( sortProperty != null )
            {
                gSeatPledges.DataSource = qry.Sort( sortProperty ).ToList();
            }
            else
            {
                gSeatPledges.DataSource = qry.OrderBy( sp => sp.PledgedPersonAlias.Person.LastName ).ThenBy( sp => sp.PledgedPersonAlias.Person.FirstName ).ToList();
            }

            //
            // Disable columns if we don't have edit access.
            //
            bool readOnly = !IsUserAuthorized( Rock.Security.Authorization.EDIT );
            gSeatPledges.Columns.Cast<DataControlField>().Where( f => f.HeaderText == "Dedication" ).FirstOrDefault().Visible = !readOnly;
            gSeatPledges.Columns.Cast<DataControlField>().Where( f => f.GetType() == typeof( DeleteField ) ).FirstOrDefault().Visible = !readOnly;

            gSeatPledges.DataBind();
        }

        /// <summary>
        /// Navigates to detail page.
        /// </summary>
        /// <param name="seatPledgeId">The seat pledge identifier.</param>
        private void NavigateToDetailPage( int seatPledgeId )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "seatPledgeId", seatPledgeId.ToString() );
            NavigateToLinkedPage( "PledgeDetailPage", qryParams );
        }

        /// <summary>
        /// Navigates to detail page.
        /// </summary>
        /// <param name="dedicationId">The seat pledge identifier.</param>
        private void NavigateToDedicationDetailPage( int dedicationId )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "seatPledgeId", dedicationId.ToString() );
            NavigateToLinkedPage( "DedicationDetailPage", qryParams );
        }

        #endregion
    }
}
