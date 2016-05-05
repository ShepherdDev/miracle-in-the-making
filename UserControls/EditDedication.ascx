<%@ Control Language="c#" CodeFile="EditDedication.ascx.cs" CodeBehind="EditDedication.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.MITM.EditDedication" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<style type="text/css">
span.cd_element
{
    display: block;
    margin-bottom: 4px;
}

span.cd_element > label { display: inline-block; width: 135px; vertical-align: top; }

span.cd_dedication > input { width: 140px; }
span.cd_sponsor > input { width: 200px; }
span.cd_biography > textarea { width: 400px; height: 80px; }
span.cd_changepicture { display: inline-block; width: 135px; }

span.cd_anonymous { display: inline; width: auto; }
span.cd_anonymous > label { width: auto; vertical-align: middle; }

div.cd_submit { margin-top: 20px; }
#dedicationInfo { margin-bottom: 30px; }
</style>

<span class="cd_element cd_dedication">
    <label for="<%= tbDedication.ClientID %>">Dedicate to</label>
    <asp:TextBox ID="tbDedication" runat="server" MaxLength="100" />
</span>

<span class="cd_element cd_picture">
    <label for="<%= fuPicture.ClientID %>">Picture</label>
    <asp:Literal ID="ltPicture" runat="server" />
    <asp:Label ID="lbChangePicture" runat="server" Visible="false"><br /><span class="cd_changepicture">&nbsp;</span>Change Picture...</asp:Label>
    <asp:FileUpload ID="fuPicture" runat="server" />
</span>

<span class="cd_element cd_biography">
    <label for="<%= tbBiography.ClientID %>">Biography</label>
    <asp:TextBox ID="tbBiography" runat="server" TextMode="MultiLine" />
</span>

<span class="cd_element cd_sponsor">
    <label for="<%= tbSponsor.ClientID %>">Sponsored by</label>
    <asp:TextBox ID="tbSponsor" runat="server" MaxLength="100" />
    <span class="cd_element cd_anonymous">
        <Arena:ArenaCheckBox ID="cbAnonymous" runat="server" />
        <label for="<%= cbAnonymous.ClientID %>">Remain Anonymous</label>
    </span>
</span>

<asp:Panel ID="pnlApproved" runat="server" Visible="false">
    <span class="cd_element cd_approved">
        <label for="<%= cbApproved.ClientID %>">Approved</label>
        <Arena:ArenaCheckBox ID="cbApproved" runat="server" />
    </span>
</asp:Panel>

<div class="cd_submit">
    <span class="cd_element">
        <label>&nbsp;</label>
        <Arena:ArenaButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Continue" />
    </span>
</div>
