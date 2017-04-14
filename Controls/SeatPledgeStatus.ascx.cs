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

namespace RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking
{
    [DisplayName( "Seat Pledge Status" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Gives the user details about their seat pledge and the status of their gifts and the chair itself." )]
    [LinkedPage( "Give Now Page", "The page that will be used for the pay remaining balance / give now button." )]
    [LinkedPage( "Dedication Details Page", "The page that will be used for editing seat dedications." )]
    [LinkedPage( "Adopt Full Seat Page", "The page that will be used for adopting the remainder of the seat." )]
    [AccountField( "Account", "The account to use for determining if contributions should match against the seat pledges." )]

    public partial class SeatPledgeStatus : Rock.Web.UI.RockBlock
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

            if ( !(string.IsNullOrEmpty( PageParameter( "seatPledgeId" ) )) )
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

                pnlSeatPledges.Visible = true;
                ShowDetails();
            }
        }

        /// <summary>
        /// Get the breadcrumb to display for this control.
        /// </summary>
        /// <param name="pageReference">The PageReference that this control is contained in.</param>
        /// <returns>List of breadcrumbs to be displayed.</returns>
        public override List<BreadCrumb> GetBreadCrumbs( Rock.Web.PageReference pageReference )
        {
            var breadCrumbs = new List<BreadCrumb>();
            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();
            SeatPledge seatPledge = null;
            string crumbName = ActionTitle.Edit( SeatPledge.FriendlyTypeName );

            if ( seatPledgeId.HasValue )
            {
                seatPledge = _seatPledge ?? new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId.Value );
                if ( seatPledge != null )
                {
                    crumbName = string.Format( "Seat {0}", seatPledge.RequestedSeat.FriendlyName );

                    if ( seatPledge.AssignedSeat != null )
                    {
                        crumbName = string.Format( "Seat {0}", seatPledge.AssignedSeat.FriendlyName );
                    }

                    pageReference.Parameters.Add( "seatPledgeId", seatPledge.Id.ToString() );
                }
            }

            breadCrumbs.Add( new BreadCrumb( crumbName, pageReference ) );

            return breadCrumbs;
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
        protected void btnPay_Click( object sender, EventArgs e )
        {
            NavigateToGiveNowPage();
        }

        /// <summary>
        /// Handles the Click event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAdoptFull_Click( object sender, EventArgs e )
        {
            NavigateToAdoptFullSeatPage( _seatPledge.Id );
        }

        /// <summary>
        /// Handles the Click event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnDedicate_Click( object sender, EventArgs e )
        {
            NavigateToDedicationDetailsPage( _seatPledge.Id );
        }

        #endregion

        #region Methods

        protected void ShowDetails()
        {
            var dataContext = new MiracleInTheMakingContext();
            var seatPledgeService = new SeatPledgeService( dataContext );
            var contributionService = new FinancialTransactionDetailService( new RockContext() );
            decimal pledgeTotal;
            decimal pledgeGiven;
            decimal percentage;
            int accountId = new FinancialAccountService( new RockContext() ).Get( GetAttributeValue( "Account" ).AsGuid() ).Id;

            //
            // Determine how much this person has pledged across all seats.
            //
            pledgeTotal = seatPledgeService.Queryable()
                .Where( sp => sp.PledgedPersonAlias.PersonId == _seatPledge.PledgedPersonAlias.PersonId )
                .Select( sp => sp.Amount )
                .DefaultIfEmpty( 0 )
                .Sum();

            //
            // Sum up all of the giving this person has made towards the appropriate account.
            //
            pledgeGiven = contributionService.Queryable()
                .Where( c => c.Transaction.AuthorizedPersonAlias.PersonId == _seatPledge.PledgedPersonAlias.PersonId )
                .Where( c => c.AccountId == accountId )
                .Select( c => c.Amount )
                .DefaultIfEmpty( 0 )
                .Sum();

            //
            // Add in any contributions from other members of the giving unit.
            //
            if ( _seatPledge.PledgedPersonAlias.Person.GivingGroup != null )
            {
                var givingGroupMembers = _seatPledge.PledgedPersonAlias.Person.GivingGroup.Members
                    .Where( g => g.Person.GivingGroupId == g.GroupId )
                    .Where( g => g.Person.Id != _seatPledge.PledgedPersonAlias.PersonId )
                    .Select( g => g.Person.Id );

                pledgeGiven += contributionService.Queryable()
                    .Where( c => givingGroupMembers.Contains( c.Transaction.AuthorizedPersonAlias.PersonId ) )
                    .Where( c => c.AccountId == accountId )
                    .Select( c => c.Amount )
                    .DefaultIfEmpty( 0 )
                    .Sum();
            }

            //
            // Process the actual values for this seat. Work as a percentage of the totals because
            // we don't know how much they have given towards this seat.
            //
            percentage = (pledgeGiven / pledgeTotal);
            decimal fillAmount = percentage; /* (pledgeGiven / (decimal)10000.0); */
            decimal paidByMe = (_seatPledge.Amount * percentage);
            decimal remainingForMe = 0;
            if ( paidByMe < _seatPledge.Amount )
            {
                remainingForMe = (_seatPledge.Amount - paidByMe);
            }

            //
            // Process how much has been pledged by other people.
            //
            decimal pledgeByOthers = seatPledgeService.Queryable()
                .Where( sp => sp.AssignedSeatId == _seatPledge.AssignedSeatId && sp.PledgedPersonAlias.PersonId != _seatPledge.PledgedPersonAlias.PersonId )
                .Select( sp => sp.Amount )
                .DefaultIfEmpty( 0 )
                .Sum();

            //
            // Determine how much still needs to be pledged.
            //
            decimal needAdopted = (10000 - pledgeTotal - pledgeByOthers);
            if (needAdopted < 0)
            {
                needAdopted = 0;
            }

            tdPledgedByMe.InnerText = _seatPledge.Amount.ToString( "C" );
            tdPaidByMe.InnerText = paidByMe.ToString( "C" );
            tdRemainingForMe.InnerText = remainingForMe.ToString( "C" );
            tdPledgedByOthers.InnerText = pledgeByOthers.ToString( "C" );
            tdNeedsToBeAdopted.InnerText = needAdopted.ToString( "C" );

            if ( remainingForMe > 0)
            {
                tdRemainingForMe.AddCssClass( "warning" );
            }
            else
            {
                tdRemainingForMe.AddCssClass( "success" );
            }

            if ( needAdopted > 0 )
            {
                tdNeedsToBeAdopted.AddCssClass( "danger" );
            }
            else
            {
                tdNeedsToBeAdopted.AddCssClass( "success" );
            }

            int fillPercent = (int)Math.Floor( fillAmount * 100 );
            fillPercent = (fillPercent > 100 ? 100 : fillPercent);
            tdFillAmount.Attributes.Add( "style", string.Format( "height: {0}%; top: {1}%;", fillPercent, (100 - fillPercent) ) );

            ltChairName.Text = (_seatPledge.AssignedSeat != null ? _seatPledge.AssignedSeat.FriendlyName : "(unassigned)");
        }

        /// <summary>
        /// Navigates to detail page.
        /// </summary>
        private void NavigateToGiveNowPage()
        {
            NavigateToLinkedPage( "GiveNowPage" );
        }

        /// <summary>
        /// Navigates to adopt full seat page.
        /// </summary>
        private void NavigateToAdoptFullSeatPage( int seatPledgeId )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "seatPledgeId", seatPledgeId.ToString() );
            NavigateToLinkedPage( "AdoptFullSeatPage", qryParams );
        }

        /// <summary>
        /// Navigates to detail page.
        /// </summary>
        /// <param name="dedicationId">The seat pledge identifier.</param>
        private void NavigateToDedicationDetailsPage( int seatPledgeId )
        {
            var qryParams = new Dictionary<string, string>();
            qryParams.Add( "seatPledgeId", seatPledgeId.ToString() );
            NavigateToLinkedPage( "DedicationDetailsPage", qryParams );
        }

        #endregion
    }
}
