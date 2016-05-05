using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Portal;
using Arena.Custom.SOTHC.MiTM;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class AdminUtils : PortalControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnCreate_Click(object sender, EventArgs e)
        {
            int start = Convert.ToInt32(tbCreateStart.Text);
            int end = Convert.ToInt32(tbCreateEnd.Text);
            int i;


            if (tbCreateSection.Text.Length != 1)
                throw new System.Exception("Invalid section specified");
            if (end < start || start < 0 || end < 0)
                throw new System.Exception("Invalid start/end values for seat numbers.");

            for (i = start; i <= end; i++)
            {
                Seat seat = new Seat();

                seat.Section = tbCreateSection.Text;
                seat.Number = i;

                seat.Save(ArenaContext.Current.User.Identity.Name);
            }
        }
    }
}