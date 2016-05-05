using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;

using Arena.Custom.SOTHC.MiTM;
using Arena.Custom.SOTHC.MiTM.DataLayer;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class SeatXmlInfo : System.Web.UI.Page
    {
        private String _arenaURL = "https://www.miracleinthemaking.com";


        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            XmlDeclaration xdecl;
            XmlElement xroot;
            String section = "A";


            xdecl = xdoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xdoc.InsertBefore(xdecl, xdoc.DocumentElement);
            xroot = xdoc.CreateElement("Seats");
            xdoc.AppendChild(xroot);

            //
            // Get the section is supplied, otherwise it will default to section A.
            //
            if (!String.IsNullOrEmpty(Request.QueryString["section"]))
                section = Request.QueryString["section"].ToUpper().Substring(0, 1);

            //
            // Populate the XML document with the information about the seats in the
            // requested section.
            //
            SeatCollection seats = new SeatCollection(section);
            foreach (Seat seat in seats)
            {
                xroot.AppendChild(SeatToXml(seat, xdoc));
            }

            //
            // Send the response to the browser as raw XML data.
            //
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            xdoc.Save(writer);
            Page.Response.Clear();
            Page.Response.ContentType = "text/xml";
            Page.Response.Write(sb.ToString());
            Page.Response.End();
        }


        /// <summary>
        /// Convert a seat into an XML element that will be added to the document.
        /// </summary>
        /// <returns></returns>
        private XmlElement SeatToXml(Seat seat, XmlDocument xdoc)
        {
            XmlElement xseat;
            XmlAttribute xattrib;
            SeatPledgeCollection spc = new SeatPledgeCollection();
            decimal totalGift = 0;


            xseat = xdoc.CreateElement("Seat");

            xattrib = xdoc.CreateAttribute("id");
            xattrib.InnerText = seat.SeatID.ToString();
            xseat.Attributes.Append(xattrib);
            
            xattrib = xdoc.CreateAttribute("section");
            xattrib.InnerText = seat.Section;
            xseat.Attributes.Append(xattrib);
            
            xattrib = xdoc.CreateAttribute("number");
            xattrib.InnerText = seat.Number.ToString();
            xseat.Attributes.Append(xattrib);

            //
            // Add in all the adoptees.
            //
            spc.LoadByAssignedSeat(seat.SeatID);
            foreach (SeatPledge sp in spc)
            {
                xseat.AppendChild(SeatPledgeToXml(sp, xdoc));
                totalGift += sp.Amount;
            }

            //
            // Check if this seat is available or not.
            //
            xattrib = xdoc.CreateAttribute("available");
            spc.LoadByAssignedSeat(seat.SeatID);
            if (totalGift == 0)
                xattrib.InnerText = "1";
            else if (totalGift >= 10000)
                xattrib.InnerText = "0";
            else
                xattrib.InnerText = "-1";
            xseat.Attributes.Append(xattrib);

            return xseat;
        }


        /// <summary>
        /// Convert a pledge towards a seat into an XML element.
        /// </summary>
        /// <returns></returns>
        private XmlElement SeatPledgeToXml(SeatPledge sp, XmlDocument xdoc)
        {
            XmlElement xadoptee, xvalue, xprayerlist, xprayer;
            XmlAttribute xanon, xattrib;
            Boolean anonymous;

            
            xadoptee = xdoc.CreateElement("Adoptee");

            //
            // If no dedication exists, or it is an anonymous dedication, or it has not been approved...
            // then it is anonymous.
            //
            anonymous = sp.Dedication.DedicationID == -1 || sp.Dedication.Anonymous || String.IsNullOrEmpty(sp.Dedication.ApprovedBy);

            //
            // Set the anonymous flag.
            //
            xanon = xdoc.CreateAttribute("anonymous");
            xanon.InnerText = (anonymous ? "1" : "0");
            xadoptee.Attributes.Append(xanon);

            //
            // Include the first name only (or say anonymous).
            //
            xvalue = xdoc.CreateElement("FirstName");
            xvalue.InnerText = (anonymous ? "Anonymous" : sp.Person.NickName);
            xadoptee.AppendChild(xvalue);
            if (!anonymous)
            {
                xvalue = xdoc.CreateElement("LastName");
                xvalue.InnerText = sp.Person.LastName;
                xadoptee.AppendChild(xvalue);
            }

            //
            // If there is a valid dedicatio record (and it has been approved), then include
            // information about the dedication.
            //
            if (sp.Dedication.DedicationID != -1 && !String.IsNullOrEmpty(sp.Dedication.ApprovedBy))
            {
                //
                // If there is a dedicate to value then set the element.
                //
                if (!String.IsNullOrEmpty(sp.Dedication.DedicatedTo))
                {
                    xvalue = xdoc.CreateElement("DedicatedTo");
                    xvalue.InnerText = sp.Dedication.DedicatedTo;
                    xadoptee.AppendChild(xvalue);
                }

                //
                // If there is a sponsored by value then set the element.
                //
                if (!String.IsNullOrEmpty(sp.Dedication.SponsoredBy))
                {
                    xvalue = xdoc.CreateElement("SponsoredBy");
                    xvalue.InnerText = sp.Dedication.SponsoredBy;
                    xadoptee.AppendChild(xvalue);
                }

                //
                // If there is a biography value then set the element.
                //
                if (!String.IsNullOrEmpty(sp.Dedication.Biography))
                {
                    xvalue = xdoc.CreateElement("Biography");
                    xvalue.InnerText = sp.Dedication.Biography;
                    xadoptee.AppendChild(xvalue);
                }

                //
                // If there is a image value then create a link to it.
                //
                if (sp.Dedication.BlobID != -1)
                {
                    xvalue = xdoc.CreateElement("Image");
                    xvalue.InnerText = String.Format("{0}/CachedBlob.aspx?guid={1}", _arenaURL, sp.Dedication.Blob.GUID.ToString());
                    xadoptee.AppendChild(xvalue);
                }

                //
                // Include the person's prayer list.
                //
                SalvationCollection sc = new SalvationCollection();
                sc.LoadByPersonID(sp.PersonID);
                xprayerlist = xdoc.CreateElement("PrayerList");
                foreach (Salvation salvation in sc)
                {
                    xprayer = xdoc.CreateElement("Name");
                    xattrib = xdoc.CreateAttribute("Type");
                    xattrib.InnerText = (salvation.Status ? "Dove" : "Cross");
                    xprayer.Attributes.Append(xattrib);
                    xattrib = xdoc.CreateAttribute("Name");
                    xattrib.InnerText = salvation.FirstName + " " + salvation.LastName;
                    xprayer.Attributes.Append(xattrib);

                    xprayerlist.AppendChild(xprayer);
                }
                xadoptee.AppendChild(xprayerlist);
            }

            return xadoptee;
        }
    }
}