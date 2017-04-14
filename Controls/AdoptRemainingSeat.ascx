<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdoptRemainingSeat.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.AdoptRemainingSeat" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
        <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

        <asp:HiddenField ID="hfAmount" runat="server" />
        <asp:Panel ID="pnlAdoptRemaining" CssClass="row" runat="server" Visible="false">
            <div class="col-sm-3">
                <asp:Image ID="imgChair" runat="server" ImageUrl="~/Plugins/com_shepherdchurch/MiracleInTheMaking/Assets/mitm_chair-128.png" />
            </div>
            
            <div class="col-sm-9 text-center">
                <p>
                    You have already pledged <asp:Literal ID="ltAlready" runat="server" /> towards
                    this seat. <asp:Literal ID="ltNeeded" runat="server" /> more is needed to adopt
                    the remainder of this seat. Clicking the <i>Adopt</i> button will request your
                    pledge to be increased by that amount. It may take a few days for this to be
                    reflected on the website so please be patient.
                </p>
                
                <p style="margin-top: 30px;">
                    <asp:LinkButton ID="hlCancel" runat="server" CssClass="btn btn-default" OnClick="btnCancel_Click">Cancel</asp:LinkButton>
                    <asp:LinkButton ID="hlAdopt" runat="server" CssClass="btn btn-primary" OnClick="btnAdopt_Click">Adopt</asp:LinkButton>
                </p>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
