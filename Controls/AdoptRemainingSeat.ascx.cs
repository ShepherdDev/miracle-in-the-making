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
using Rock.Web.UI;
using Rock.Constants;
using System.Text;
using Rock.Communication;

namespace RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking
{
    [DisplayName( "Adopt Remaining Seat" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Gives the user a chance to request that their pledge be increased." )]
    [LinkedPage( "Return Page", "Page to return the user to after they have clicked Save or Cancel. If not set returns to parent page.", false )]
    [EmailField( "Admin Email", "The email address to send administration notice of new pledges.", false )]

    public partial class AdoptRemainingSeat : Rock.Web.UI.RockBlock
    {
        SeatPledge _seatPledge = null;

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

            if ( !( string.IsNullOrEmpty( PageParameter( "seatPledgeId" ) ) ) )
            {
                _seatPledge = new SeatPledgeService( new MiracleInTheMakingContext() ).Get( PageParameter( "seatPledgeId" ).AsInteger() );
            }
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
                if ( _seatPledge == null || _seatPledge.Id == 0 )
                {
                    nbWarningMessage.Text = "The link to this page seems to be broken as we did not receive the expected information.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                if ( !UserCanAdministrate && _seatPledge.PledgedPersonAlias.PersonId != CurrentPerson.Id )
                {
                    nbWarningMessage.Text = "Invalid seat pledge.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                pnlAdoptRemaining.Visible = true;
                ShowDetails();
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
        /// Handles the Click event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAdopt_Click( object sender, EventArgs e )
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine( String.Format( "{0} has submitted a request to increase their pledge.<br />", _seatPledge.PledgedPersonAlias.Person.FullName ) );
            sb.AppendLine( "<br />" );
            sb.AppendLine( String.Format( "Seat: {0}{1}<br />", _seatPledge.AssignedSeat.Section, _seatPledge.AssignedSeat.SeatNumber.ToString() ) );
            sb.AppendLine( String.Format( "Increase by: {0} (currently {1})<br />", hfAmount.Value, _seatPledge.Amount.ToString( "C" ) ) );
            sb.AppendLine( "<br />" );
            sb.AppendLine( "Please remember to update their contributions pledge record as well.<br />" );

            string fromName = GlobalAttributesCache.Value( "Organization Name" );
            string fromEmail = GlobalAttributesCache.Value( "Organization Email" );
            List<string> recipients = new List<string>();
            recipients.Add( GetAttributeValue( "AdminEmail" ) );
            Email.Send( fromEmail, fromName, "Request to Increase Pledge", recipients, sb.ToString() );

            NavigateToLinkedPage( "ReturnPage" );
        }

        /// <summary>
        /// Handles the Click event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "ReturnPage" );
        }

        #endregion

        #region Methods

        protected void ShowDetails()
        {
            var dataContext = new MiracleInTheMakingContext();
            var seatPledgeService = new SeatPledgeService( dataContext );
            var contributionService = new FinancialTransactionDetailService( new RockContext() );
            decimal pledgeTotal;

            //
            // Determine how much has been pledged towards this seat
            //
            pledgeTotal = seatPledgeService.GetByAssignedSeatId( _seatPledge.AssignedSeatId.Value )
                .Select( sp => sp.Amount )
                .DefaultIfEmpty( 0 )
                .Sum();
            ltNeeded.Text = hfAmount.Value = ( 10000 - pledgeTotal ).ToString( "C" );

            ltAlready.Text = _seatPledge.Amount.ToString( "C" );
        }

        #endregion
    }
}
