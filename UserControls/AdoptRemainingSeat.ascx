<%@ Control Language="c#" Inherits="ArenaWeb.UserControls.Custom.MITM.AdoptRemainingSeat" CodeFile="AdoptRemainingSeat.ascx.cs" CodeBehind="AdoptRemainingSeat.ascx.cs" %>

<style type="text/css">
    a.btnLink
    {
        padding: 8px;
        margin-left: 20px;
        margin-right: 20px;
        border: 1px solid black;
        width: 100px;
        height: 35px;
        display: inline-block;
        background-color: #e7e7e7;
        text-align: center;
    }
    a.btnLink:hover
    {
        background-color: #c7c7c7;
    }
    div.adoptContent > p
    {
    	margin-right: 40px;
    }
    p.buttons
    {
    	margin-top: 40px;
    }
    
    div.pnlAdoptRemaining { min-height: 280px; }
    div.adoptImage { width: 250px; float: left; }
    div.adoptContent { margin-left: 250px; }
</style>

<asp:HiddenField ID="hfAmount" runat="server" />

<asp:Panel ID="pnlAdoptRemaining" CssClass="pnlAdoptRemaining" runat="server">
	<div class="adoptImage">
		<img src="UserControls/Custom/SOTHC/MiTM/Images/mitm_chair.jpg" alt="Chair" />
	</div>
	
	<div class="adoptContent">
		<p>
		You have already pledged <asp:Literal ID="ltAlready" runat="server" /> towards
		this seat. <asp:Literal ID="ltNeeded" runat="server" /> more is needed to adopt
		the remainder of this seat. Clicking the <i>Adopt</i> button will request your
		pledge to be increased by that amount. It may take a few days for this to be
		reflected on the website so please be patient.
		</p>
		
		<p class="buttons">
			<asp:HyperLink ID="hlCancel" runat="server" CssClass="btnLink cancel" NavigateUrl="default.aspx">Cancel</asp:HyperLink>
			<asp:LinkButton ID="hlAdopt" runat="server" CssClass="btnLink adopt" OnClick="btnAdopt_Click">Adopt</asp:LinkButton>
		</p>
	</div>
</asp:Panel>
