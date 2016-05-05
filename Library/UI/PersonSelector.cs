using Arena.Portal.UI;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Arena.Custom.SOTHC.MiTM.UI
{
	public class PersonSelector : PersonPicker
	{
		public bool CustomRequired
		{
			get;
			set;
		}

		public PersonSelector()
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			base.SetValues();
			if (this.CustomRequired)
			{
				FieldInfo field = typeof(PersonPicker).GetField("_deleteButton", BindingFlags.Instance | BindingFlags.NonPublic);
				ImageButton value = (ImageButton)field.GetValue(this);
				value.Style["width"] = "0px";
			}
		}
	}
}