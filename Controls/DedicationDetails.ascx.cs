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
using Rock.Communication;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Rock.Constants;
using Rock.Security;
using System.Text;

namespace RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking
{
    /// <summary>
    /// Displays the details of a Referral Agency.
    /// </summary>
    [DisplayName( "Dedication Detail" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Displays the details of a Dedication." )]
    [SecurityAction( Authorization.APPROVE, "The roles and/or users that have access to approve dedications." )]
    [LinkedPage( "Return Page", "Page to return the user to after they have clicked Save or Cancel. If not set returns to parent page.", false )]
    [SystemEmailField( "Confirmation Email", "The email to send the person to confirm their pledge information", false )]
    [EmailField( "Admin Email", "The email address to send administration notice of new pledges.", false )]
    [LinkedPage( "Seat Pledge Details Page", "Page in the administrative site for seeing the details of a Seat Pledge.", false )]
    [LinkedPage( "Dedication Details Page", "Page in the administrative site for seeing the details of a Dedication.", false )]

    public partial class DedicationDetails : Rock.Web.UI.RockBlock
    {
        #region Fields

        private Dedication _dedication = null;

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddCSSLink( ResolveUrl( "~/Plugins/com_shepherdchurch/MiracleInTheMaking/Styles/mitm.css" ) );

            //
            // This event gets fired after block settings are updated. it's nice to repaint the
            // screen if these settings would alter it.
            //
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            //
            // Load the dedication for use in other methods.
            //
            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();
            if ( seatPledgeId.HasValue )
            {
                var seatPledge = new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId.Value );
                if ( seatPledge != null )
                {
                    _dedication = seatPledge.Dedications.FirstOrDefault();
                }
            }
            if (_dedication == null)
            {
                _dedication = new Dedication();
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
                divAdminActions.Visible = UserCanAdministrate;
                btnTestAdminEmail.Visible = !string.IsNullOrWhiteSpace( GetAttributeValue( "AdminEmail" ) );
                btnTestConfirmationEmail.Visible = !string.IsNullOrWhiteSpace( GetAttributeValue( "ConfirmationEmail" ) );

                if ( _dedication.Id != 0 && !UserCanEdit && (CurrentUser == null || _dedication.SeatPledge.PledgedPersonAlias.PersonId != CurrentUser.PersonId) )
                {
                    nbWarningMessage.Text = "Attempt to edit dedication for seat not owned by current user.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                if ( string.IsNullOrWhiteSpace( PageParameter( "seatPledgeId" ) ) && (string.IsNullOrWhiteSpace( PageParameter( "seatId" ) ) || string.IsNullOrWhiteSpace( PageParameter( "amount" ) ) || string.IsNullOrWhiteSpace( PageParameter( "choices" ) )) )
                {
                    nbWarningMessage.Text = "Configuration error. No pledge information or build-a-chair information found.";
                    nbWarningMessage.Visible = true;

                    return;
                }

                ShowDetail();
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
            string crumbName = ActionTitle.Edit( Dedication.FriendlyTypeName );

            breadCrumbs.Add( new BreadCrumb( crumbName, pageReference, true ) );

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
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            NavigateToReturnPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();
            int? seatId = PageParameter( "seatId" ).AsIntegerOrNull();
            string choices = PageParameter( "choices" );
            int? amount = PageParameter( "amount" ).AsIntegerOrNull();
            int? oldBinaryFileId = null;
            var dataContext = new MiracleInTheMakingContext();
            var dedicationService = new DedicationService( dataContext );
            var seatPledgeService = new SeatPledgeService( dataContext );
            Dedication dedication;
            SeatPledge seatPledge;
            bool newSeatPledge = false;
            bool newDedication = false;

            if ( _dedication.Id != 0 && !UserCanEdit && (CurrentUser == null || _dedication.SeatPledge.PledgedPersonAlias.PersonId != CurrentUser.PersonId) )
            {
                nbWarningMessage.Text = "You must have done something strange to get here, you don't have edit permission.";
                nbWarningMessage.Visible = true;
                nbInfoMessage.Visible = false;

                return;
            }

            if ( !IsValid() )
            {
                return;
            }

            if ( seatId.HasValue )
            {
                var seat = new SeatService( dataContext ).Queryable().Where( s => s.SeatNumber == seatId.Value ).FirstOrDefault();
                seatId = seat != null ? (int?)seat.Id : null;
            }

            if ( seatPledgeId.HasValue )
            {
                seatPledge = seatPledgeService.Get( seatPledgeId.Value );
            }
            else if ( seatId.HasValue && !string.IsNullOrWhiteSpace( choices ) && amount.HasValue && amount.Value > 0 )
            {
                //
                // We need to create a new seat pledge based on the information passed from the URL.
                //
                seatPledge = new SeatPledge();
                seatPledgeService.Add( seatPledge );
                seatPledge.PledgedPersonAliasId = new PersonAliasService( new RockContext() ).GetPrimaryAliasId( CurrentUser.PersonId.Value ).Value;
                seatPledge.RequestedSeatId = seatId.Value;
                seatPledge.Amount = amount.Value;

                foreach ( string choice in choices.Split( ',' ) )
                {
                    if ( choice == "full" )
                    {
                        seatPledge.RequestedFullSeat = 1;
                    }
                    else if ( choice == "back" )
                    {
                        seatPledge.RequestedBackRest = 1;
                    }
                    else if ( choice == "leg1" )
                    {
                        seatPledge.RequestedLeg1 = 1;
                    }
                    else if ( choice == "leg2" )
                    {
                        seatPledge.RequestedLeg2 = 1;
                    }
                    else if ( choice == "leg3" )
                    {
                        seatPledge.RequestedLeg3 = 1;
                    }
                    else if ( choice == "leg4" )
                    {
                        seatPledge.RequestedLeg4 = 1;
                    }
                    else if ( choice == "armleft" )
                    {
                        seatPledge.RequestedArmLeft = 1;
                    }
                    else if ( choice == "armright" )
                    {
                        seatPledge.RequestedArmRight = 1;
                    }
                }

                dataContext.SaveChanges();
                newSeatPledge = true;
            }
            else
            {
                nbWarningMessage.Text = "Passed parameters are not recognized.";
                nbWarningMessage.Visible = true;
                nbInfoMessage.Visible = false;

                return;
            }

            //
            // Load for edit or create a new Dedication.
            //
            if ( _dedication.Id == 0 )
            {
                dedication = new Dedication();
                dedicationService.Add( dedication );

                dedication.SeatPledgeId = seatPledge.Id;
                newDedication = true;
            }
            else
            {
                dedication = dedicationService.Get( _dedication.Id );
            }

            dedication.DedicatedTo = tbDedicatedTo.Text.Trim();
            dedication.SponsoredBy = tbSponsoredBy.Text.Trim();
            dedication.Biography = tbBiography.Text.Trim();
            dedication.IsAnonymous = cbAnonymous.Checked;
            dedication.ApprovedBy = (cbApproved.Checked ? CurrentUser.UserName : string.Empty);
            oldBinaryFileId = dedication.BinaryFileId;
            dedication.BinaryFileId = imgupPhoto.BinaryFileId;

            if ( !dedication.IsValid || !Page.IsValid )
            {
                // Controls will render the error messages
                return;
            }

            //
            // Mark the BinaryFile as non temporary if we need to.
            //
            if (dedication.BinaryFileId.HasValue)
            {
                using ( RockContext rockContext = new RockContext() )
                {
                    BinaryFile bf = new BinaryFileService( rockContext ).Get( dedication.BinaryFileId.Value );

                    if ( bf.IsTemporary )
                    {
                        bf.IsTemporary = false;
                        rockContext.SaveChanges();
                    }
                }
            }

            //
            // Save our changes.
            //
            dataContext.SaveChanges();

            //
            // Cleanup the old binary file type if we need to.
            //
            if ( oldBinaryFileId.HasValue )
            {
                if ( !dedication.BinaryFileId.HasValue || oldBinaryFileId.Value != dedication.BinaryFileId.Value )
                {
                    using ( RockContext rockContext = new RockContext() )
                    {
                        BinaryFile bf = new BinaryFileService( rockContext ).Get( oldBinaryFileId.Value );

                        if ( !bf.IsTemporary )
                        {
                            bf.IsTemporary = true;
                            rockContext.SaveChanges();
                        }
                    }
                }
            }

            //
            // Send confirmation e-mail to the logged in user.
            //
            if ( newSeatPledge )
            {
                SendConfirmationEmail( seatPledge, dedication );
            }

            //
            // Send e-mail to the administrator.
            //
            SendAdminEmail( newSeatPledge, newDedication, seatPledge.Id );

            NavigateToReturnPage();
        }

        /// <summary>
        /// Test the end-user confirmation e-mail.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnTestConfirmationEmail_Click( object sender, EventArgs e )
        {
            Dedication dedication;

            dedication = new DedicationService( new MiracleInTheMakingContext() )
                .Queryable()
                .Where( d => d.SponsoredBy != "" && d.DedicatedTo != "" ).FirstOrDefault();
            
            if (dedication == null)
            {
                dedication = new DedicationService( new MiracleInTheMakingContext() ).Queryable().FirstOrDefault();

                if ( dedication == null )
                {
                    nbInfoMessage.Text = "Could not find a dedication to test with.";
                    nbInfoMessage.Visible = true;

                    return;
                }
            }

            SendConfirmationEmail( dedication.SeatPledge, dedication );

            nbInfoMessage.Text = "Sent a confirmation e-mail to your primary e-mail address for testing purposes.";
            nbInfoMessage.Visible = true;
        }

        /// <summary>
        /// Test the admin e-mail.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnTestAdminEmail_Click( object sender, EventArgs e )
        {
            Dedication dedication;

            dedication = new DedicationService( new MiracleInTheMakingContext() )
                .Queryable()
                .Where( d => d.SponsoredBy != "" && d.DedicatedTo != "" )
                .FirstOrDefault();

            if ( dedication == null )
            {
                dedication = new DedicationService( new MiracleInTheMakingContext() ).Queryable().FirstOrDefault();

                if ( dedication == null )
                {
                    nbInfoMessage.Text = "Could not find a dedication to test with.";
                    nbInfoMessage.Visible = true;

                    return;
                }
            }

            SendAdminEmail( true, true, dedication.SeatPledgeId );

            nbInfoMessage.Text = "Sent an admin notice e-mail to your primary e-mail address for testing purposes.";
            nbInfoMessage.Visible = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends a confirmation e-mail to the user if they have a valid e-mail address and
        /// a e-mail template has been selected.
        /// </summary>
        /// <param name="seatPledge">The SeatPledge object to pass to the lava parser.</param>
        /// <param name="dedication">The Dedication object to pass to the lava parser.</param>
        protected void SendConfirmationEmail( SeatPledge seatPledge, Dedication dedication )
        {
            if ( CurrentPerson.IsEmailActive && !string.IsNullOrWhiteSpace( CurrentPerson.Email ) )
            {
                Guid confirmationEmailTemplateGuid = Guid.Empty;
                if ( Guid.TryParse( GetAttributeValue( "ConfirmationEmail" ), out confirmationEmailTemplateGuid ) )
                {
                    var mergeFields = Rock.Lava.LavaHelper.GetCommonMergeFields( RockPage, CurrentPerson );
                    var recipients = new List<RecipientData>();

                    mergeFields.Add( "SeatPledge", seatPledge );
                    mergeFields.Add( "Dedication", dedication );
                    recipients.Add( new RecipientData( CurrentPerson.Email, mergeFields ) );

                    Email.Send( confirmationEmailTemplateGuid, recipients, ResolveRockUrl( "~/" ), ResolveRockUrl( "~~/" ) );
                }
            }
        }

        /// <summary>
        /// Send an e-mail to the administrative contact about what has happened.
        /// </summary>
        /// <param name="newSeatPledge">true if a new seat pledge was created.</param>
        /// <param name="newDedication">true if a new dedication was created.</param>
        /// <param name="seatPledgeId">The ID of the seat pledge for linking to.</param>
        protected void SendAdminEmail(bool newSeatPledge, bool newDedication, int seatPledgeId)
        {
            if ( !string.IsNullOrWhiteSpace( GetAttributeValue( "AdminEmail" ) ) )
            {
                StringBuilder sb = new StringBuilder();
                SeatPledge seatPledge = new SeatPledgeService( new MiracleInTheMakingContext() ).Get( seatPledgeId );

                sb.AppendFormat( "New Pledge activity for {0}.<br />", seatPledge.PledgedPersonAlias.Person.FullName );
                sb.Append( "<br />" );

                if ( newSeatPledge )
                {
                    sb.AppendLine( "A new seat pledge has been entered into the system and is waiting seat assignment.<br />" );

                    if ( !string.IsNullOrWhiteSpace( GetAttributeValue( "SeatPledgeDetailsPage" ) ) )
                    {
                        var pageParams = new Dictionary<string, string>();
                        pageParams.Add( "seatPledgeId", seatPledgeId.ToString() );

                        var pageReference = new Rock.Web.PageReference( GetAttributeValue( "SeatPledgeDetailsPage" ), pageParams );

                        sb.AppendFormat( "Click <a href=\"{0}{1}\">here</a> to view the seat pledge.<br />", GlobalAttributesCache.Value( "InternalApplicationRoot" ), pageReference.BuildUrl() );
                    }

                    sb.AppendLine( "<br />" );
                }

                if ( newDedication )
                {
                    sb.AppendLine( "A new seat dedication message has been entered into the system and is waiting approval.<br />" );

                    if ( !string.IsNullOrWhiteSpace( GetAttributeValue( "DedicationDetailsPage" ) ) )
                    {
                        var pageParams = new Dictionary<string, string>();
                        pageParams.Add( "seatPledgeId", seatPledgeId.ToString() );

                        var pageReference = new Rock.Web.PageReference( GetAttributeValue( "DedicationDetailsPage" ), pageParams );

                        sb.AppendFormat( "Click <a href=\"{0}{1}\">here</a> to view the dedication message.<br />", GlobalAttributesCache.Value( "InternalApplicationRoot" ), pageReference.BuildUrl() );
                    }

                    sb.AppendLine( "<br />" );
                }
                else
                {
                    sb.AppendLine( "A seat dedication message has been updated and is waiting approval.<br />" );

                    if ( !string.IsNullOrWhiteSpace( GetAttributeValue( "DedicationDetailsPage" ) ) )
                    {
                        var pageParams = new Dictionary<string, string>();
                        pageParams.Add( "seatPledgeId", seatPledgeId.ToString() );

                        var pageReference = new Rock.Web.PageReference( GetAttributeValue( "DedicationDetailsPage" ), pageParams );

                        sb.AppendFormat( "Click <a href=\"{0}{1}\">here</a> to view the dedication message.<br />", GlobalAttributesCache.Value( "InternalApplicationRoot" ), pageReference.BuildUrl() );
                    }

                    sb.AppendLine( "<br />" );
                }

                string fromName = GlobalAttributesCache.Value( "Organization Name" );
                string fromEmail = GlobalAttributesCache.Value( "Organization Email" );
                List<string> recipients = new List<string>();
                recipients.Add( GetAttributeValue( "AdminEmail" ) );
                Email.Send( fromEmail, fromName, "New Pledge/Dedication Activity", recipients, sb.ToString() );
            }
        }

        /// <summary>
        /// Check if the form data is valid for normal processing.
        /// </summary>
        /// <returns>True if the form elements were valid for processing.</returns>
        protected bool IsValid()
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(tbDedicatedTo.Text) && string.IsNullOrWhiteSpace(tbSponsoredBy.Text) && string.IsNullOrWhiteSpace(tbBiography.Text) && !imgupPhoto.BinaryFileId.HasValue)
            {
                errors.Add( "Must provide at least one of Dedicated To, Sponsored By, Biography or Photo" );
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
        /// Show the details of the current Dedication, or an empty one if one has not been
        /// selected via the URL.
        /// </summary>
        private void ShowDetail()
        {
            bool readOnly = false;

            pnlDetails.Visible = true;

            if ( _dedication.Id == 0 )
            {
                lActionTitle.Text = ActionTitle.Add( Dedication.FriendlyTypeName ).FormatAsHtmlTitle();
            }
            else
            {
                lActionTitle.Text = ActionTitle.Edit( Dedication.FriendlyTypeName ).FormatAsHtmlTitle();
            }

            hfDedicationId.Value = _dedication.Id.ToString();
            tbDedicatedTo.Text = _dedication.DedicatedTo;
            tbSponsoredBy.Text = _dedication.SponsoredBy;
            tbBiography.Text = _dedication.Biography;
            imgupPhoto.BinaryFileId = _dedication.BinaryFileId;
            cbAnonymous.Checked = _dedication.IsAnonymous;
            cbApproved.Checked = !string.IsNullOrEmpty( _dedication.ApprovedBy );

            //
            // Check if the user has edit permissions or not.
            //
            nbEditModeMessage.Text = string.Empty;
            if ( !UserCanEdit )
            {
                readOnly = true;
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( Dedication.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnCancel.Text = "Close";
            }

            //
            // Disable controls if we are read only.
            //
            tbDedicatedTo.ReadOnly = readOnly;
            tbSponsoredBy.ReadOnly = readOnly;
            tbBiography.ReadOnly = readOnly;
            imgupPhoto.Enabled = !readOnly;
            cbAnonymous.Enabled = !readOnly;
            cbApproved.Enabled = (!readOnly && IsUserAuthorized( Authorization.APPROVE ));
            pnlApproved.Visible = IsUserAuthorized( Authorization.APPROVE );

            btnSave.Visible = !readOnly;
        }

        /// <summary>
        /// Navigate to either the return page if set or the parent page.
        /// </summary>
        void NavigateToReturnPage()
        {
            if ( !NavigateToLinkedPage( "ReturnPage" ) )
            {
                Response.Redirect( RockPage.BreadCrumbs[RockPage.BreadCrumbs.Count - 2].Url );
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        #endregion
    }
}