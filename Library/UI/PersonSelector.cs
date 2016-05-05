using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal.UI;

namespace Arena.Custom.SOTHC.MiTM.UI
{
    public class PersonSelector : PersonPicker
    {
        #region Public Properties

        public Boolean CustomRequired { get; set; }

        #endregion


        public PersonSelector()
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            System.Reflection.FieldInfo fi;
            ImageButton delButton;
            
            
            base.OnLoad(e);
            SetValues();

            //
            // Set the hidden state of the delete button if this is a required field.
            //
            if (CustomRequired)
            {
                fi = typeof(PersonPicker).GetField("_deleteButton", BindingFlags.NonPublic | BindingFlags.Instance);
                delButton = (ImageButton)fi.GetValue(this);
                delButton.Style["width"] = "0px";
            }
        }
    }
}
