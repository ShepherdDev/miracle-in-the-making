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
    [DisplayName( "Build A Chair" )]
    [Category( "com_shepherdchurch > Miracle In The Making" )]
    [Description( "Allows users to build a chair pledge amount before being redirected to the page that saves their pledge." )]
    [LinkedPage( "Dedication Details", "The dedication details page that allows the user to enter dedication information for this pledge." )]

    public partial class BuildAChair : Rock.Web.UI.RockBlock
    {
        #region Fields

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddCSSLink( ResolveUrl( "~/Plugins/com_shepherdchurch/MiracleInTheMaking/Styles/buildachair.css" ) );
            RockPage.AddScriptLink( ResolveUrl( "~/Plugins/com_shepherdchurch/MiracleInTheMaking/Scripts/buildachair.js" ) );
            RockPage.AddScriptLink( ResolveUrl( "~/Plugins/com_shepherdchurch/MiracleInTheMaking/Scripts/facheckbox.js" ) );

            //
            // This event gets fired after block settings are updated. it's nice to repaint the
            // screen if these settings would alter it.
            //
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            btnSubmit.Click += btnSubmit_Click;
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
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSubmit_Click( object sender, EventArgs e )
        {
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
        }

        #endregion
    }
}