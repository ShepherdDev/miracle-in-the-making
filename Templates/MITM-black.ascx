<%@ control language="c#" inherits="ArenaWeb.Templates.MITM" CodeFile="~/Templates/MITM-black.ascx.cs" %>
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
	  <div id="contentTop-black">
						<div id="toppane"><asp:PlaceHolder runat="server" ID="TopPane" /></div>
                        <asp:PlaceHolder runat="server" ID="MainPane" />
		</div>
		<div id="container">
					  <asp:PlaceHolder runat="server" ID="BottomPane" />
		</div>

    </div>
    <img src="Images/miracleinthemaking/MITM_bottom.png" /><br />
    <div id="footer" class="footer">©2014 Shepherd Church<br />
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
