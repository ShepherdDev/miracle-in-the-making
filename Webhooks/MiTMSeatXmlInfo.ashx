<%@ WebHandler Language="C#" Class="MiTMSeatXmlInfo" %>

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

using Rock;
using com.shepherdchurch.MiracleInTheMaking.Data;
using com.shepherdchurch.MiracleInTheMaking.Model;

public class MiTMSeatXmlInfo : IHttpHandler
{
    public void ProcessRequest( HttpContext context )
    {
        var dbContext = new MiracleInTheMakingContext();
        XmlDocument xdoc = new XmlDocument();
        XmlDeclaration xdecl;
        XmlElement xroot;
        string section = "A";

        xdecl = xdoc.CreateXmlDeclaration( "1.0", "UTF-8", null );
        xdoc.InsertBefore( xdecl, xdoc.DocumentElement );
        xroot = xdoc.CreateElement( "Seats" );
        xdoc.AppendChild( xroot );

        //
        // Get the section is supplied, otherwise it will default to section A.
        //
        if ( !String.IsNullOrEmpty( context.Request.QueryString["section"] ) )
        {
            section = context.Request.QueryString["section"].ToUpper().Substring( 0, 1 );
        }

        //
        // Populate the XML document with the information about the seats in the
        // requested section.
        //
        var seats = new SeatService( dbContext ).GetBySection( section );
        foreach ( Seat seat in seats )
        {
            xroot.AppendChild( SeatToXml( seat, xdoc, dbContext ) );
        }

        //
        // Send the response to the browser as raw XML data.
        //
        StringBuilder sb = new StringBuilder();
        StringWriter writer = new StringWriter( sb );
        xdoc.Save( writer );
        context.Response.ContentType = "text/xml";
        context.Response.Write( sb.ToString() );
        context.Response.End();
    }

    /// <summary>
    /// Convert a seat into an XML element that will be added to the document.
    /// </summary>
    /// <returns></returns>
    private XmlElement SeatToXml( Seat seat, XmlDocument xdoc, MiracleInTheMakingContext dbContext )
    {
        XmlElement xseat;
        XmlAttribute xattrib;
        var spc = new SeatPledgeService( dbContext );
        decimal totalGift = 0;


        xseat = xdoc.CreateElement( "Seat" );

        xattrib = xdoc.CreateAttribute( "id" );
        xattrib.InnerText = seat.Id.ToString();
        xseat.Attributes.Append( xattrib );

        xattrib = xdoc.CreateAttribute( "section" );
        xattrib.InnerText = seat.Section;
        xseat.Attributes.Append( xattrib );

        xattrib = xdoc.CreateAttribute( "number" );
        xattrib.InnerText = seat.SeatNumber.ToString();
        xseat.Attributes.Append( xattrib );

        //
        // Add in all the adoptees.
        //
        foreach ( SeatPledge sp in spc.GetByAssignedSeatId( seat.Id ) )
        {
            xseat.AppendChild( SeatPledgeToXml( sp, xdoc, dbContext ) );
            totalGift += sp.Amount;
        }

        //
        // Check if this seat is available or not.
        //
        xattrib = xdoc.CreateAttribute( "available" );
        if ( totalGift == 0 )
            xattrib.InnerText = "1";
        else if ( totalGift >= 10000 )
            xattrib.InnerText = "0";
        else
            xattrib.InnerText = "-1";
        xseat.Attributes.Append( xattrib );

        return xseat;
    }

    /// <summary>
    /// Convert a pledge towards a seat into an XML element.
    /// </summary>
    /// <returns></returns>
    private XmlElement SeatPledgeToXml( SeatPledge sp, XmlDocument xdoc, MiracleInTheMakingContext dbContext )
    {
        XmlElement xadoptee, xvalue, xprayerlist, xprayer;
        XmlAttribute xanon, xattrib;
        Boolean anonymous;
        Dedication dedication = sp.Dedications.FirstOrDefault();


        xadoptee = xdoc.CreateElement( "Adoptee" );

        //
        // If no dedication exists, or it is an anonymous dedication, or it has not been approved...
        // then it is anonymous.
        //
        anonymous = dedication == null || dedication.IsAnonymous || String.IsNullOrEmpty( dedication.ApprovedBy );

        //
        // Set the anonymous flag.
        //
        xanon = xdoc.CreateAttribute( "anonymous" );
        xanon.InnerText = ( anonymous ? "1" : "0" );
        xadoptee.Attributes.Append( xanon );

        //
        // Include the first name only (or say anonymous).
        //
        xvalue = xdoc.CreateElement( "FirstName" );
        xvalue.InnerText = ( anonymous ? "Anonymous" : sp.PledgedPersonAlias.Person.NickName );
        xadoptee.AppendChild( xvalue );
        if ( !anonymous )
        {
            xvalue = xdoc.CreateElement( "LastName" );
            xvalue.InnerText = sp.PledgedPersonAlias.Person.LastName;
            xadoptee.AppendChild( xvalue );
        }

        //
        // If there is a valid dedicatio record (and it has been approved), then include
        // information about the dedication.
        //
        if ( dedication != null && !String.IsNullOrEmpty( dedication.ApprovedBy ) )
        {
            //
            // If there is a dedicate to value then set the element.
            //
            if ( !String.IsNullOrEmpty( dedication.DedicatedTo ) )
            {
                xvalue = xdoc.CreateElement( "DedicatedTo" );
                xvalue.InnerText = dedication.DedicatedTo;
                xadoptee.AppendChild( xvalue );
            }

            //
            // If there is a sponsored by value then set the element.
            //
            if ( !String.IsNullOrEmpty( dedication.SponsoredBy ) )
            {
                xvalue = xdoc.CreateElement( "SponsoredBy" );
                xvalue.InnerText = dedication.SponsoredBy;
                xadoptee.AppendChild( xvalue );
            }

            //
            // If there is a biography value then set the element.
            //
            if ( !String.IsNullOrEmpty( dedication.Biography ) )
            {
                xvalue = xdoc.CreateElement( "Biography" );
                xvalue.InnerText = dedication.Biography;
                xadoptee.AppendChild( xvalue );
            }

            //
            // If there is a image value then create a link to it.
            //
            if ( dedication.BinaryFile != null )
            {
                xvalue = xdoc.CreateElement( "Image" );
                xvalue.InnerText = string.Format( "{0}GetImage.ashx?guid={1}", Rock.Web.Cache.GlobalAttributesCache.Read().GetValue( "PublicApplicationRoot" ), dedication.BinaryFile.Guid );
                xadoptee.AppendChild( xvalue );
            }

            //
            // Include the person's prayer list.
            //
            var sc = new SalvationService( dbContext );
            var qry = sc.Queryable().Where( s => s.PersonAlias.PersonId == sp.PledgedPersonAlias.PersonId );
            xprayerlist = xdoc.CreateElement( "PrayerList" );
            foreach ( Salvation salvation in qry )
            {
                xprayer = xdoc.CreateElement( "Name" );
                xattrib = xdoc.CreateAttribute( "Type" );
                xattrib.InnerText = ( salvation.IsSaved ? "Dove" : "Cross" );
                xprayer.Attributes.Append( xattrib );
                xattrib = xdoc.CreateAttribute( "Name" );
                xattrib.InnerText = salvation.FirstName + " " + salvation.LastName;
                xprayer.Attributes.Append( xattrib );

                xprayerlist.AppendChild( xprayer );
            }
            xadoptee.AppendChild( xprayerlist );
        }

        return xadoptee;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
