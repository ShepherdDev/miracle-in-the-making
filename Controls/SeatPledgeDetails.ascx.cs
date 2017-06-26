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
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Constants;
using System.Text;

namespace RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking
{
    /// <summary>
    /// Displays the details of a Referral Agency.
    /// </summary>
    [DisplayName( "Seat Pledge Detail" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Displays the details of a Seat Pledge." )]

    public partial class SeatPledgeDetails : Rock.Web.UI.RockBlock
    {
        #region Fields

        private SeatPledge _seatPledge = null;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            //
            // This event gets fired after block settings are updated. it's nice to repaint the
            // screen if these settings would alter it.
            //
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            //
            // Create a button addon for the AssignedSeat control.
            //
            LinkButton button = new LinkButton();
            button.ID = "btnAuto";
            button.AddCssClass( "btn" ).AddCssClass( "btn-default" );
            button.Text = "Auto";
            button.Click += btnAuto_Click;
            button.CausesValidation = false;
            tbAssignedSeat.AppendControl( button );

            //
            // Create a button addon for the Amount control. Also inject client-side onclick event
            // javascript to calculate the amount rather than use a postback.
            //
            string calculateJs = string.Format( "{{ var amount = 0; $('[id^=\"{0}\"][type=\"checkbox\"]:checked').each(function() {{ if ($(this).data('amount')) amount += parseInt($(this).data('amount')); }}); $('#{1}').val(amount.toFixed(2).replace(/(\\d)(?=(\\d{{3}})+\\.)/g, '$1,')); return false; }}",
                cblRequestedParts.ClientID, tbAmount.ClientID );
            button = new LinkButton();
            button.ID = "btnCalculate";
            button.AddCssClass( "btn" ).AddCssClass( "btn-default" );
            button.Text = "Calculate";
            button.Attributes.Add( "onclick", calculateJs );
            button.CausesValidation = false;
            tbAmount.AppendControl( button );

            //
            // Load the seat pledge for use in other methods.
            //
            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();
            if ( seatPledgeId.HasValue )
            {
                _seatPledge = new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId.Value );
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
                ShowDetail();
            }
        }

        /// <summary>
        /// Before the page is rendered take a moment to do some final tweaks to the UI.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender( EventArgs e )
        {
            bool readOnly = false;

            base.OnPreRender( e );

            if ( !IsUserAuthorized( Rock.Security.Authorization.EDIT ) )
            {
                readOnly = true;
            }

            //
            // If the user has readonly access or this is an existing pledge then disable the
            // requested parts check boxes. This is a workaround for an ASP.NET bug in which
            // disabling a CheckBoxList does not disable the items within it.
            //
            if ( readOnly || (_seatPledge != null && _seatPledge.Id != 0) )
            {
                foreach ( ListItem li in cblRequestedParts.Items )
                {
                    li.Attributes.Add( "disabled", "disabled" );
                }
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

            string crumbName = ActionTitle.Add( SeatPledge.FriendlyTypeName );

            if ( seatPledgeId.HasValue )
            {
                seatPledge = _seatPledge ?? new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId.Value );
                if ( seatPledge != null )
                {
                    crumbName = seatPledge.PledgedPersonAlias.Person.FullName;
                    pageReference.Parameters.Add( "seatPledgeId", seatPledgeId.ToString() );
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
        /// User wants to auto-assign the next available seat to this pledge.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnAuto_Click( object sender, EventArgs e )
        {
            SeatService seatService = new SeatService( new MiracleInTheMakingContext() );
            SeatPledgeService seatPledgeService = new SeatPledgeService( new MiracleInTheMakingContext() );
            SeatPledge seatPledge = (_seatPledge ?? new SeatPledge());
            Seat seat = null;
            string seatName;
            int seatId;
            decimal amount = 0;
            decimal assignedAmount;

            if ( !IsValid(verifyAssignedSeat: false) )
            {
                return;
            }

            nbWarningMessage.Visible = false;
            nbInfoMessage.Visible = false;

            decimal.TryParse( tbAmount.Text, out amount );

            //
            // Check the currently assigned seat, if there is one, to see if there is enough room.
            //
            if ( seatPledge.Id != 0 && tbAssignedSeat.Text.Trim().Length > 0 )
            {
                try
                {
                    seatName = tbAssignedSeat.Text.Trim().ToUpper();
                    seatId = seatService.GetBySectionAndNumber( seatName.Substring( 0, 1 ), Convert.ToInt32( seatName.Substring( 1 ) ) ).Id;

                    assignedAmount = (seatPledgeService.Queryable()
                        .Where( sp => sp.AssignedSeatId == seatId && sp.Id != seatPledge.Id )
                        .Sum( sp => ( decimal? )sp.Amount ) ?? 0.0M) + amount;
                }
                catch
                {
                    seatId = 0;
                    assignedAmount = decimal.MaxValue;
                }

                if (seatId != 0 && assignedAmount <= 10000)
                {
                    tbAssignedSeat.Text = seatService.Get( seatId ).FriendlyName;

                    nbInfoMessage.Text = "Existing seat assignment will be kept.";
                    nbInfoMessage.Visible = true;

                    return;
                }
            }

            //
            // Check the requested seat to see if there is enough room.
            //
            seatName = tbRequestedSeat.Text.Trim().ToUpper();
            seatId = seatService.GetBySectionAndNumber( seatName.Substring( 0, 1 ), Convert.ToInt32( seatName.Substring( 1 ) ) ).Id;

            assignedAmount = (seatPledgeService.Queryable()
                .Where( sp => sp.AssignedSeatId == seatId && sp.Id != seatPledge.Id )
                .Sum( sp => ( decimal? )sp.Amount ) ?? 0.0M) + amount;

            if ( assignedAmount <= 10000 )
            {
                tbAssignedSeat.Text = seatService.Get( seatId ).FriendlyName;

                nbInfoMessage.Text = "Requested seat will be used.";
                nbInfoMessage.Visible = true;

                return;
            }

            //
            // Look for a new seat to assign.
            //
            seat = seatService.GetNextAvailableForSeatPledge( amount, seatPledge.Id );
            if ( seat != null )
            {
                tbAssignedSeat.Text = seat.FriendlyName;

                nbInfoMessage.Text = string.Format( "Assigned seat {0}", seat.FriendlyName );
                nbInfoMessage.Visible = true;
            }
            else
            {
                nbWarningMessage.Visible = true;
                nbWarningMessage.Text = "Could not find any seat to accomodate the desired amount.";
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            SeatPledge seatPledge;
            var dataContext = new MiracleInTheMakingContext();
            var service = new SeatPledgeService( dataContext );
            var seatService = new SeatService( dataContext );
            int seatPledgeId = int.Parse( hfSeatPledgeId.Value );

            if ( !IsUserAuthorized( Rock.Security.Authorization.EDIT ) )
            {
                nbWarningMessage.Text = "You must have done something strange to get here, you don't have edit permission.";
                nbWarningMessage.Visible = true;
                nbInfoMessage.Visible = false;
                return;
            }

            //
            // Get either the existing seat pledge or a new one.
            //
            if ( seatPledgeId == 0 )
            {
                seatPledge = new SeatPledge();
                service.Add( seatPledge );
            }
            else
            {
                seatPledge = service.Get( seatPledgeId );
            }

            seatPledge.PledgedPersonAliasId = (int)ppPledger.PersonAliasId;
            seatPledge.Amount = decimal.Parse( tbAmount.Text );
            if ( tbAssignedSeat.Text.Trim().Length == 0 )
            {
                seatPledge.AssignedSeatId = null;
            }
            else
            {
                if ( char.IsDigit( tbAssignedSeat.Text[0] ) )
                {
                    int seatNumber = tbAssignedSeat.Text.AsInteger();

                    seatPledge.AssignedSeatId = seatService.Queryable()
                        .Where( s => s.SeatNumber == seatNumber )
                        .FirstOrDefault().Id;
                }
                else
                {
                    seatPledge.AssignedSeatId = seatService.GetBySectionAndNumber( tbAssignedSeat.Text.Substring( 0, 1 ), int.Parse( tbAssignedSeat.Text.Substring( 1 ) ) ).Id;
                }
            }

            //
            // Only save changes to the requested information if this is a new pledge.
            //
            if ( seatPledge.Id == 0 )
            {
                seatPledge.RequestedSeatId = seatService.GetBySectionAndNumber( tbRequestedSeat.Text.Substring( 0, 1 ), int.Parse( tbRequestedSeat.Text.Substring( 1 ) ) ).Id;
                seatPledge.RequestedFullSeat = (cblRequestedParts.Items.FindByValue( "full" ).Selected ? 1 : 0);
                seatPledge.RequestedBackRest = (cblRequestedParts.Items.FindByValue( "back" ).Selected ? 1 : 0);
                seatPledge.RequestedLeg1 = (cblRequestedParts.Items.FindByValue( "leg1" ).Selected ? 1 : 0);
                seatPledge.RequestedLeg2 = (cblRequestedParts.Items.FindByValue( "leg2" ).Selected ? 1 : 0);
                seatPledge.RequestedLeg3 = (cblRequestedParts.Items.FindByValue( "leg3" ).Selected ? 1 : 0);
                seatPledge.RequestedLeg4 = (cblRequestedParts.Items.FindByValue( "leg4" ).Selected ? 1 : 0);
                seatPledge.RequestedArmLeft = (cblRequestedParts.Items.FindByValue( "armleft" ).Selected ? 1 : 0);
                seatPledge.RequestedArmRight = (cblRequestedParts.Items.FindByValue( "armright" ).Selected ? 1 : 0);
            }

            if ( !seatPledge.IsValid || !Page.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            dataContext.SaveChanges();

            NavigateToParentPage();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check if the form data is valid for normal processing.
        /// </summary>
        /// <param name="verifyAssignedSeat">If true, verify that the assigned seat is valid.</param>
        /// <param name="verifyAmount">If true, verify that the amount is valid.</param>
        /// <returns>True if the form elements were valid for processing.</returns>
        protected bool IsValid( bool verifyAssignedSeat = true, bool verifyAmount = true )
        {
            SeatService seatService = new SeatService( new MiracleInTheMakingContext() );
            List<string> errors = new List<string>();
            decimal amount = 0;

            //
            // Verify that the amount was a valid number.
            //
            if ( verifyAmount && (tbAmount.Text.Trim().Length == 0 || decimal.TryParse( tbAmount.Text.Trim(), out amount ) == false) )
            {
                errors.Add( "Invalid amount specified." );
            }

            //
            // Verify that the assigned seat is a valid seat.
            //
            if ( verifyAssignedSeat && tbAssignedSeat.Text.Trim().Length > 0 )
            {
                string seatName = tbAssignedSeat.Text.Trim().ToUpper();
                Seat seat = seatService.GetBySectionAndNumber( seatName.Substring( 0, 1 ), Convert.ToInt32( seatName.Substring( 1 ) ) );

                if ( seat == null )
                {
                    errors.Add( "Unknown seat specified for the assigned seat." );
                }
            }

            //
            // Verify that the requested seat is a valid seat.
            //
            if ( tbRequestedSeat.Text.Trim().Length > 0 )
            {
                string seatName = tbRequestedSeat.Text.Trim().ToUpper();
                Seat seat = seatService.GetBySectionAndNumber( seatName.Substring( 0, 1 ), Convert.ToInt32( seatName.Substring( 1 ) ) );

                if ( seat == null )
                {
                    errors.Add( "Unknown seat specified for the requested seat." );
                }
            }

            //
            // If there were errors then display them, otherwise continue on.
            //
            if ( errors.Count > 0 )
            {
                nbWarningMessage.Visible = true;
                nbWarningMessage.Text = errors.Aggregate( new StringBuilder( "<ul>" ), ( sb, s ) => sb.AppendFormat( "<li>{0}</li>", s ) ).Append( "</ul>" ).ToString();

                return false;
            }
            else
            {
                nbWarningMessage.Visible = false;

                return true;
            }
        }

        /// <summary>
        /// Show the details of the current SeatPledge, or an empty one if one has not been
        /// selected via the URL.
        /// </summary>
        private void ShowDetail()
        {
            pnlDetails.Visible = true;

            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();

            SeatPledge seatPledge = null;
            if ( seatPledgeId.HasValue )
            {
                seatPledge = _seatPledge ?? new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId.Value );
            }

            if ( seatPledge != null )
            {
                RockPage.PageTitle = seatPledge.PledgedPersonAlias.Person.FullName;
                lActionTitle.Text = ActionTitle.Edit( seatPledge.PledgedPersonAlias.Person.FullName ).FormatAsHtmlTitle();
            }
            else
            {
                seatPledge = new SeatPledge { Id = 0 };
                RockPage.PageTitle = ActionTitle.Add( SeatPledge.FriendlyTypeName );
                lActionTitle.Text = ActionTitle.Add( SeatPledge.FriendlyTypeName ).FormatAsHtmlTitle();
            }

            hfSeatPledgeId.Value = seatPledge.Id.ToString();
            if ( seatPledge.PledgedPersonAlias != null )
            {
                ppPledger.SetValue( seatPledge.PledgedPersonAlias.Person );
            }

            tbAmount.Text = seatPledge.Amount.ToString( "N" );

            if ( seatPledge.AssignedSeat != null )
            {
                tbAssignedSeat.Text = seatPledge.AssignedSeat.FriendlyName;
            }

            if ( seatPledge.Id != 0)
            {
                tbRequestedSeat.Text = seatPledge.RequestedSeat.FriendlyName;
                cblRequestedParts.Items.FindByValue( "full" ).Selected = (seatPledge.RequestedFullSeat > 0);
                cblRequestedParts.Items.FindByValue( "back" ).Selected = (seatPledge.RequestedBackRest > 0);
                cblRequestedParts.Items.FindByValue( "leg1" ).Selected = (seatPledge.RequestedLeg1 > 0);
                cblRequestedParts.Items.FindByValue( "leg2" ).Selected = (seatPledge.RequestedLeg2 > 0);
                cblRequestedParts.Items.FindByValue( "leg3" ).Selected = (seatPledge.RequestedLeg3 > 0);
                cblRequestedParts.Items.FindByValue( "leg4" ).Selected = (seatPledge.RequestedLeg4 > 0);
                cblRequestedParts.Items.FindByValue( "armleft" ).Selected = (seatPledge.RequestedArmLeft > 0);
                cblRequestedParts.Items.FindByValue( "armright" ).Selected = (seatPledge.RequestedArmRight > 0);
            }

            bool readOnly = false;

            //
            // Check if the user has edit permissions or not.
            //
            nbEditModeMessage.Text = string.Empty;
            if ( !IsUserAuthorized( Rock.Security.Authorization.EDIT ) )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( SeatPledge.FriendlyTypeName );
            }

            if ( readOnly )
            {
                lActionTitle.Text = ActionTitle.View( SeatPledge.FriendlyTypeName );
                btnCancel.Text = "Close";
            }

            //
            // Disable controls if we are read only.
            //
            ppPledger.Enabled = !readOnly;
            tbAmount.ReadOnly = readOnly;
            tbAssignedSeat.ReadOnly = readOnly;
            tbRequestedSeat.ReadOnly = (readOnly || seatPledge.Id != 0);

            btnSave.Visible = !readOnly;
        }

        #endregion
    }
}