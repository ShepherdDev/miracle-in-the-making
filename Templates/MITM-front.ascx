<%@ control language="c#" inherits="ArenaWeb.Templates.MITMfront" CodeFile="~/Templates/MITM-front.ascx.cs" %>
<link href='https://fonts.googleapis.com/css?family=Cuprum&subset=latin' rel='stylesheet' type='text/css'>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.4.3/jquery.min.js" type="text/javascript"></script>
<script language='javascript'>
<!--

	function setLayer(showFlag, innnerHtml)
	{
		if(document.divPopup)   
		{
			if (showFlag) 
			{
				document.divPopup.style.visibility='visible';
				document.divPopup.innerHTML = innerHtml;
			}
			else 
			{
				document.divPopup.style.visibility='hidden'; 
			}
		}
		else
		{
			if(document.getElementById("divPopup"))
			{
				if (showFlag)
				{
					document.getElementById("divPopup").style.visibility = 'visible';
					document.getElementById("divPopup").innerHTML = innerHtml;
				}
				else
				{
					document.getElementById("divPopup").style.visibility = 'hidden';
				}
			}
		}
	}

-->
</script>
<div id="wrapper"><div id="body">
	<div id="header"><div id="thenumbers"><a href="/"><img src="Images/miracleinthemaking/header_Billy_blank.png" border="0"/></a><a href="/default.aspx?page=4069"><img src="Images/miracleinthemaking/header_Billy.png" border="0" style=" position: relative; z-index:1000;" /></a></div></div>
	<div style="z-index:-1;"><img src="Images/miracleinthemaking/MITM_top.png" style="z-index:-1;" /></div>
	<div id="content">
    	<div id="nav"><asp:PlaceHolder runat="server" ID="Nav" /></div>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" id="frontfour">
          <tr>
            <td valign="top"><div id="slider">
            <asp:PlaceHolder runat="server" ID="slider" /></div></td>
            <td valign="top"><div id="threebuttons" class="threebutton"><!--<img src="Images/miracleinthemaking/give.jpg" alt="Give" width="280" height="80" /><img src="Images/miracleinthemaking/ebay.jpg" alt="Ebay" width="280" height="80" /><img src="Images/miracleinthemaking/tableside.jpg" width="280" height="80" alt="tableside" />--><asp:PlaceHolder runat="server" ID="threebuttons" /></div>
			</td>
          </tr>
          <tr>
            <td valign="top"  colspan="2"><div id="adslider"><asp:PlaceHolder runat="server" ID="adslider" /></div></td>
          </tr>
         <!-- <tr>
            <td valign="top"><div id="welcome"><img src="Images/miracleinthemaking/welcome.png" width="238" height="29" alt="Welcome" id="imgwelcome" />
            <asp:PlaceHolder runat="server" ID="welcome" /></div></td>
            <td valign="top"><div id="newsletter"><img src="Images/miracleinthemaking/newsletter.png" width="267" height="26" alt="newsletter" id="imgnewsletter" />
            <asp:PlaceHolder runat="server" ID="newsletter" /></div></td>
          </tr>//-->
        </table>
        <asp:PlaceHolder runat="server" ID="socialbar" /><!--
        <div id="social">
        <table width="840" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="top"><div id="twitter"><a href="http://twitter.com/shepherd_MITM" target="_blank"><img src="Images/miracleinthemaking/twitter.png" alt="twitter" width="379" height="31" border="0" /></a>
            <asp:PlaceHolder runat="server" ID="twitter" /></div></td>
            <td valign="top"> <div id="facebook"><a href="http://www.facebook.com/MIRACLEinthemaking" target="_blank"><img src="Images/miracleinthemaking/facebook.png" alt="facebook" width="379" height="31" border="0" /></a>
            <asp:PlaceHolder runat="server" ID="facebook" /></div></td>
          </tr>
        </table>
      </div>//-->
    </div>
    <img src="Images/miracleinthemaking/MITM_bottom.png" /><br />
    <div id="footer" class="footer"><a href="/default.aspx?page=4012">©</a>2015 Shepherd Church<br />
<a href="http://shepherdchurch.com" target="_blank">shepherdchurch.com</a> | 818.831.9333<asp:PlaceHolder runat="server" ID="footer" /></div>
</div>
</div>
<script type="text/javascript">

  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', 'UA-16420952-1']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();

</script>
