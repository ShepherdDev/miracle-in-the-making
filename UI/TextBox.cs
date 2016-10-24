using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;

using Rock;
using Rock.Web.UI.Controls;

namespace com.shepherdchurch.MiracleInTheMaking.UI
{
    public class TextBox : RockTextBox
    {
        /// <summary>
        /// Controls to be prepended to the input-group.
        /// </summary>
        List<Control> _prependedControls;

        /// <summary>
        /// Controls to be appended to the input-group.
        /// </summary>
        List<Control> _appendedControls;

        /// <summary>
        /// Initialize a new instance of TextBox.
        /// </summary>
        public TextBox() : base()
        {
            _prependedControls = new List<Control>();
            _appendedControls = new List<Control>();
        }

        /// <summary>
        /// Add a new control to the list that will be appended to the textbox input-group.
        /// </summary>
        /// <param name="control">The control to append.</param>
        public void AppendControl(Control control)
        {
            _appendedControls.Add( control );
            Controls.Add( control );
        }

        /// <summary>
        /// Add a new control to the list that will be prepended to the textbox input-group.
        /// </summary>
        /// <param name="control">The control to prepend.</param>
        public void PrependControl( Control control )
        {
            _prependedControls.Add( control );
            Controls.Add( control );
        }

        /// <summary>
        /// Allow the base to create it's child controls (this also clears the Controls list) and then re-add
        /// our own controls to it.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _appendedControls.ForEach( ( c ) => Controls.Add( c ) );
            _prependedControls.ForEach( ( c ) => Controls.Add( c ) );
        }

        /// <summary>
        /// Renders the control into the writer.
        /// </summary>
        /// <param name="writer">Where to write the rendered HTML.</param>
        public override void RenderBaseControl( HtmlTextWriter writer )
        {
            if ( _prependedControls.Count == 0 && _appendedControls.Count == 0 )
            {
                base.RenderBaseControl( writer );
            }
            else
            {
                string content;

                //
                // Create placeholders if we need them.
                //
                if ( string.IsNullOrEmpty( PrependText ) )
                {
                    PrependText = "__PLACEHOLDER__";
                }
                if ( string.IsNullOrEmpty( AppendText ) )
                {
                    AppendText = "__PLACEHOLDER__";
                }

                using ( TextWriter tw = new StringWriter() )
                {
                    using ( HtmlTextWriter htw = new HtmlTextWriter( tw ) )
                    {
                        //
                        // Render the base control into our own HtmlTextWriter so we can modify the
                        // contents.
                        //
                        base.RenderBaseControl( htw );
                        content = tw.ToString();
                    }
                }

                //
                // Find the existing addons (or placeholders) so we can add in our controls.
                //
                Match match = Regex.Match( content, "^<div[^>]+>\\s*(<span.*<\\/span>){0,1}\\s*<input[^>]+>(<span.*<\\/span>){0,1}\\s*<\\/div>$", RegexOptions.Singleline );
                if ( match != null && match.Success )
                {
                    string replaceContent;

                    //
                    // Process appended conrols.
                    //
                    replaceContent = string.Empty;

                    //
                    // Restore the AppendText HTML.
                    //
                    if ( AppendText != "__PLACEHOLDER__" )
                    {
                        replaceContent += match.Groups[2].Value;
                    }

                    //
                    // Walk each AppendControls and get the rendered HTML for it.
                    //
                    foreach ( Control control in _appendedControls )
                    {
                        if ( control is System.Web.UI.WebControls.WebControl )
                        {
                            (( System.Web.UI.WebControls.WebControl )control).AddCssClass( "input-group-addon" );
                        }
                        else if ( control is System.Web.UI.HtmlControls.HtmlControl )
                        {
                            (( System.Web.UI.HtmlControls.HtmlControl )control).AddCssClass( "input-group-addon" );
                        }

                        using ( TextWriter ctw = new StringWriter() )
                        {
                            using ( HtmlTextWriter chtw = new HtmlTextWriter( ctw ) )
                            {
                                control.RenderControl( chtw );

                                replaceContent += ctw.ToString();
                            }
                        }
                    }

                    content = content.Substring( 0, match.Groups[2].Index ) + replaceContent + content.Substring( match.Groups[2].Index + match.Groups[2].Length );

                    //
                    // Process prepended controls.
                    //
                    replaceContent = string.Empty;

                    //
                    // Walk each AppendControls and get the rendered HTML for it.
                    //
                    foreach ( Control control in _prependedControls )
                    {
                        if ( control is System.Web.UI.WebControls.WebControl )
                        {
                            (( System.Web.UI.WebControls.WebControl )control).AddCssClass( "input-group-addon" );
                        }
                        else if ( control is System.Web.UI.HtmlControls.HtmlControl )
                        {
                            (( System.Web.UI.HtmlControls.HtmlControl )control).AddCssClass( "input-group-addon" );
                        }

                        using ( TextWriter ctw = new StringWriter() )
                        {
                            using ( HtmlTextWriter chtw = new HtmlTextWriter( ctw ) )
                            {
                                control.RenderControl( chtw );

                                replaceContent += ctw.ToString();
                            }
                        }
                    }

                    //
                    // Restore the PrependText HTML.
                    //
                    if ( PrependText != "__PLACEHOLDER__" )
                    {
                        replaceContent += match.Groups[1].Value;
                    }

                    content = content.Substring( 0, match.Groups[1].Index ) + replaceContent + content.Substring( match.Groups[1].Index + match.Groups[1].Length );
                }

                //
                // Restore the pre/append text.
                //
                if ( PrependText == "__PLACEHOLDER__" )
                {
                    PrependText = string.Empty;
                }
                if ( AppendText == "__PLACEHOLDER__" )
                {
                    AppendText = string.Empty;
                }

                //
                // Write out the finalized output.
                //
                writer.Write( content );
            }
        }
    }
}
