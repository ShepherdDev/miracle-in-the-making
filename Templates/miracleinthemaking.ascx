<%@ control language="c#" inherits="ArenaWeb.Templates.miracleinthemaking" CodeFile="~/Templates/miracleinthemaking.ascx.cs" %>
 
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
<script type="text/javascript" src="Images/miracleinthemaking/js/jquery-1.4.1.js"></script>
<script type="text/javascript" src="Images/miracleinthemaking/js/jquery.lwtCountdown-0.9.5.js"></script>
<div id="wrapper"><div id="body">
	<div id="header"><img src="Images/miracleinthemaking/MiracleTitle.png" alt="Miracle in the Making" /></div>
	<img src="Images/miracleinthemaking/box-top.png" />
	<div id="content">
	  <div id="contentTop">
					  <asp:PlaceHolder runat="server" ID="TopPane" />

					  <asp:PlaceHolder runat="server" ID="MainPane" />
		</div>
		<div id="container">
					  <asp:PlaceHolder runat="server" ID="BottomPane" />
		</div>
	</div>
	<img src="Images/miracleinthemaking/box-bottom.png" /><br />
      <div id="footer"><a href="http://www.theshepherd.org"><img src="Images/miracleinthemaking/logo.png" alt="Shepherd Church" width="160" height="50" border="0" /></a></div>
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