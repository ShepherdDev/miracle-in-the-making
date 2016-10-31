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
    [DisplayName( "Salvation List" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Lists existing salvation prayer requests and allows user to modify or add new ones." )]

    public partial class SalvationList : Rock.Web.UI.RockBlock
    {
        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddCSSLink( ResolveUrl( "~/Plugins/com_shepherdchurch/MiracleInTheMaking/Styles/mitm.css" ) );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            gSalvations.RowItemText = "Salvation";
            gSalvations.DataKeyNames = new string[] { "Id" };
            gSalvations.IsDeleteEnabled = true;
            gSalvations.Actions.ShowAdd = true;
            gSalvations.Actions.ShowBulkUpdate = false;
            gSalvations.Actions.ShowMergePerson = false;
            gSalvations.Actions.ShowCommunicate = false;
            gSalvations.Actions.ShowExcelExport = false;
            gSalvations.Actions.ShowMergeTemplate = false;
            gSalvations.Actions.AddClick += gSalvations_Add;
            gSalvations.GridRebind += gSalvations_GridRebind;
            gSalvations.RowDataBound += gSalvations_RowDataBound;

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
                if ( CurrentPerson == null )
                {
                    nbWarningMessage.Text = "You must be logged in to view your salvation prayer list.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                pnlSalvationList.Visible = true;
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
        /// Handles the Add event of the gSalvations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gSalvations_Add( object sender, EventArgs e )
        {
            tbNewSalvationFirstName.Text = string.Empty;
            tbNewSalvationLastName.Text = string.Empty;

            mdlNewSalvation.Show();
        }

        /// <summary>
        /// Handles the SaveClick event of the mdlNewSalvation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdlNewSalvation_SaveClick( object sender, EventArgs e )
        {
            var dataContext = new MiracleInTheMakingContext();
            var service = new SalvationService( dataContext );
            Salvation salvation = new Salvation();

            mdlNewSalvation.Hide();

            salvation.PersonAliasId = CurrentPerson.PrimaryAliasId.Value;
            salvation.FirstName = tbNewSalvationFirstName.Text.Trim();
            salvation.LastName = tbNewSalvationLastName.Text.Trim();
            salvation.IsSaved = false;

            service.Add( salvation );
            dataContext.SaveChanges();

            BindGrid();
        }

        /// <summary>
        /// Handles the Edit event of the gSalvations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gSalvations_ToggleStatus( object sender, RowEventArgs e )
        {
            var dataContext = new MiracleInTheMakingContext();
            var service = new SalvationService( dataContext );
            var salvation = service.Get( ( int )e.RowKeyValue );

            if ( salvation != null )
            {
                salvation.IsSaved = !salvation.IsSaved;
                dataContext.SaveChanges();
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the Delete event of the gSalvations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs"/> instance containing the event data.</param>
        protected void gSalvations_Delete( object sender, RowEventArgs e )
        {
            var dataContext = new MiracleInTheMakingContext();
            var service = new SalvationService( dataContext );
            var salvation = service.Get( ( int )e.RowKeyValue );

            if ( salvation != null )
            {
                string errorMessage;
                if ( !service.CanDelete( salvation, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                service.Delete( salvation );
                dataContext.SaveChanges();
            }

            BindGrid();
        }

        /// <summary>
        /// Handles the GridRebind event of the gSalvations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gSalvations_GridRebind( object sender, EventArgs e )
        {
            BindGrid();
        }

        protected void gSalvations_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                Salvation salvation = ( Salvation )e.Row.DataItem;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the filter.
        /// </summary>
        private void BindFilter()
        {
        }

        /// <summary>
        /// Binds the grid.
        /// </summary>
        private void BindGrid()
        {
            var service = new SalvationService( new MiracleInTheMakingContext() );
            SortProperty sortProperty = gSalvations.SortProperty;

            var qry = service.Queryable().Where( s => s.PersonAlias.PersonId == CurrentPerson.Id );

            // Sort results
            if ( sortProperty != null )
            {
                gSalvations.DataSource = qry.Sort( sortProperty ).ToList();
            }
            else
            {
                gSalvations.DataSource = qry.OrderBy( s => s.LastName ).ThenBy( s => s.FirstName ).ToList();
            }

            gSalvations.DataBind();
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
