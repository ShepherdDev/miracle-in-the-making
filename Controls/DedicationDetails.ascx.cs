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
            NavigateToGrandParentPage();
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            int? seatPledgeId = PageParameter( "seatPledgeId" ).AsIntegerOrNull();
            int? oldBinaryFileId = null;
            var dataContext = new MiracleInTheMakingContext();
            var service = new DedicationService( dataContext );
            Dedication dedication;

            if ( !UserCanEdit )
            {
                nbWarningMessage.Text = "You must have done something strange to get here, you don't have edit permission.";
                nbWarningMessage.Visible = true;
                nbInfoMessage.Visible = false;

                return;
            }

            if ( _dedication.Id == 0 )
            {
                dedication = new Dedication();
                service.Add( dedication );

                dedication.SeatPledgeId = seatPledgeId.Value;
            }
            else
            {
                dedication = service.Get( _dedication.Id );
            }

            dedication.DedicatedTo = tbDedicatedTo.Text.Trim();
            dedication.SponsoredBy = tbSponsoredBy.Text.Trim();
            dedication.Biography = tbBiography.Text.Trim();
            dedication.IsAnonymous = cbAnonymous.Checked;
            dedication.ApprovedBy = CurrentUser.UserName;
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

            NavigateToGrandParentPage();
        }

        #endregion

        #region Methods

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
        /// It's kind of a hack, but for the way this is setup it works. Seat Pledge List is linked to the
        /// Dedication Details page, which is a child of the Seat Pledge Details page. So we need to jump
        /// all the way back to the Seat Pledge List page.
        /// </summary>
        /// <param name="queryString">Parameters to pass to the grand-parent page.</param>
        /// <returns>true if the redirect has happened, false otherwise.</returns>
        protected bool NavigateToGrandParentPage(Dictionary<string, string> queryString = null)
        {
            var pageCache = PageCache.Read( RockPage.PageId );

            if ( pageCache != null )
            {
                var page = (pageCache.ParentPage != null ? pageCache.ParentPage.ParentPage : null);
                if ( page != null )
                {
                    return NavigateToPage( page.Guid, queryString );
                }
            }

            return false;
        }

        #endregion
    }
}