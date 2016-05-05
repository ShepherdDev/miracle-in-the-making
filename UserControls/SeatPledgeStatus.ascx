<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeatPledgeStatus.ascx.cs" CodeBehind="SeatPledgeStatus.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.SeatPledgeStatus" %>

<style type="text/css">
    div.pnlButtons 
    {
        margin-top: 50px;
        margin-bottom: 30px;
        text-align: center;
    }
    
    a.btnLink
    {
        margin-left: 10px;
    }
    
    div.pnlBreakdown { min-height: 280px; }
    div.chairthermo { width: 300px; float: left; }
    div.pnlBrekadownContent { margin-left: 300px; }

    td.breakdown { padding: 5px; }
    span.breakdownImage { display: inline-block; width: 18px; height: 18px; border: 1px solid black; }
    td.paidme > span.breakdownImage { background-color: Yellow; }
    td.remainingme > span.breakdownImage { background-color: Gray; }
    td.pledgedothers > span.breakdownImage { background-color: #7070e0; }
    td.needadopted > span.breakdownImage { background-color: Red; }
</style>

<asp:Panel ID="pnlBreakdown" runat="server" CssClass="pnlBreakdown">
    <div class="chairthermo">
		<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="300" height="280" id="progressModule" align="middle">
    		<param name="movie" value="UserControls/Custom/SOTHC/MiTM/Images/progressModule_300x280.swf" />
    		<param name="quality" value="high" />
    		<param name="bgcolor" value="#000000" />
    		<param name="play" value="true" />
    		<param name="loop" value="true" />
    		<param name="wmode" value="window" />
    		<param name="scale" value="showall" />
    		<param name="menu" value="true" />
    		<param name="devicefont" value="false" />
    		<param name="salign" value="" />
    		<param name="allowScriptAccess" value="sameDomain" />
    		<param name="flashVars" value="FillAmount=<%= fillAmount %>" />
    		<!--[if !IE]>-->
    		<object type="application/x-shockwave-flash" data="UserControls/Custom/SOTHC/MiTM/Images/progressModule_300x280.swf" width="300" height="280">
    			<param name="movie" value="UserControls/Custom/SOTHC/MiTM/Images/progressModule_300x280.swf" />
    			<param name="quality" value="high" />
    			<param name="bgcolor" value="#000000" />
    			<param name="play" value="true" />
    			<param name="loop" value="true" />
    			<param name="wmode" value="window" />
    			<param name="scale" value="showall" />
    			<param name="menu" value="true" />
    			<param name="devicefont" value="false" />
    			<param name="salign" value="" />
    			<param name="allowScriptAccess" value="sameDomain" />
    			<param name="flashVars" value="FillAmount=<%= fillAmount %>" />
    		<!--<![endif]-->
    			<a href="http://www.adobe.com/go/getflash">
    				<img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" />
    			</a>
    		<!--[if !IE]>-->
    		</object>
    		<!--<![endif]-->
    	</object>
    </div>
    
    <div class="breakdownContent">
        <table border="0">
            <tbody>
                <tr>
                    <td class="breakdown paidme">
                        <span class="breakdownImage">&nbsp;</span>
                        <asp:Label ID="lbPaidByMe" runat="server" CssClass="breakdownText">Paid by me ($0)</asp:Label>
                    </td>
                    <td class="breakdown remainingme">
                        <span class="breakdownImage">&nbsp;</span>
                        <asp:Label ID="lbRemainingForMe" runat="server" CssClass="breakdownText">Remaining for me ($0)</asp:Label>
                    </td>
                </tr>
                <asp:PlaceHolder ID="phBreakdownOthers" runat="server" Visible="true">
                    <tr>
                        <td class="breakdown pledgedothers">
                            <span class="breakdownImage">&nbsp;</span>
                            <asp:Label ID="lbPledgedByOthers" runat="server" CssClass="breakdownText">Pledged by others ($0)</asp:Label>
                        </td>
                    <td class="breakdown needadopted">
                        <span class="breakdownImage">&nbsp;</span>
                        <asp:Label ID="lbNeedAdopted" runat="server" CssClass="breakdownText">Needs to be adopted ($0)</asp:Label>
                    </td>
                    </tr>
                </asp:PlaceHolder>
            </tbody>
        </table>

        <asp:Panel ID="pnlButtons" runat="server" CssClass="pnlButtons">
            <table border="0">
                <tbody>
                    <tr>
                        <td><asp:HyperLink ID="hlPayLink" runat="server" CssClass="btnLink" NavigateUrl="default.aspx"><img src="UserControls/Custom/SOTHC/MiTM/Images/pay_remain_btn.png" alt="Pay Remaining Balance" /></asp:HyperLink></td>
                        <td><asp:HyperLink ID="hlAdoptFull" runat="server" CssClass="btnLink" NavigateUrl="default.aspx"><img src="UserControls/Custom/SOTHC/MiTM/Images/adopt_fullSeat_btn.png" alt="Adopt Full Seat" /></asp:HyperLink></td>
                        <td><asp:HyperLink ID="hlDedicate" runat="server" CssClass="btnLink" NavigateUrl="default.aspx"><img src="UserControls/Custom/SOTHC/MiTM/Images/dedicate_seat_btn.png" alt="Dedicate Seat" /></asp:HyperLink></td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </div>
</asp:Panel>

