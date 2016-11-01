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
    [DisplayName( "My Seat Pledges" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Lists existing seat pledges for the currently logged in user." )]
    [LinkedPage( "Seat Pledge Status Page", "Page to direct the user to when they select one of the seats from the list." )]

    public partial class MySeatPledges : Rock.Web.UI.RockBlock
    {
        int _currentPersonId = 0;
        protected Rock.Model.Page seatPledgeStatusPage = null;

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
                if ( UserCanAdministrate && !(string.IsNullOrEmpty( PageParameter( "personId" ) )) )
                {
                    _currentPersonId = PageParameter( "personId" ).AsInteger();
                }
                else if ( CurrentPerson != null )
                {
                    _currentPersonId = CurrentPerson.Id;
                }

                if ( _currentPersonId == 0 )
                {
                    nbWarningMessage.Text = "You must be logged in to view your seat pledges.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                seatPledgeStatusPage = new PageService( new RockContext() ).Get( GetAttributeValue( "SeatPledgeStatusPage" ).AsGuid() );

                pnlSeatPledges.Visible = true;
                BindRepeater();
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

        #endregion

        #region Methods

        /// <summary>
        /// Binds the repeater.
        /// </summary>
        private void BindRepeater()
        {
            var service = new SeatPledgeService( new MiracleInTheMakingContext() );

            var qry = service.Queryable().Where( sp => sp.PledgedPersonAlias.PersonId == _currentPersonId );

            // Sort results
            qry = qry.OrderBy( sp => sp.AssignedSeat != null ? sp.AssignedSeat.Section : sp.RequestedSeat.Section )
                     .ThenBy( sp => sp.AssignedSeat != null ? sp.AssignedSeat.SeatNumber : sp.RequestedSeat.SeatNumber );
            var results = qry.ToList();

            rpSeatPledges.DataSource = results;
            rpSeatPledges.DataBind();

            if ( results.Count == 0 )
            {
                nbInfoMessage.Text = "You do not appear to currently have any seat pledges.";
                nbInfoMessage.Visible = true;
            }

            if ( results.Count == 1 )
            {
                Dictionary<string, string> query = new Dictionary<string, string>();

                query.Add( "seatPledgeId", results[0].Id.ToString() );
                NavigateToLinkedPage( "SeatPledgeStatusPage", query );
            }
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
