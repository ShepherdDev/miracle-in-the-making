<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalvationList.ascx.cs" CodeBehind="SalvationList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.SalvationList" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<style type="text/css">
    div.mitm_sl_row { margin-bottom: 6px; }
    span.mitm_sl_status { display: inline-block; }
    span.mitm_sl_name { display: inline-block; width: 300px; font-size: large; }
    div.mitm_sl_row > input { padding: 0px; vertical-align: middle; }

    div.mitm_sl_Add
    {
        width: 400px;
        border: 2px solid #B0B0DF;
        margin-top: 30px;
        padding: 15px;
    }
    div.mitm_sl_Add > label { display: inline-block; width: 150px; text-align: right; }
    div.mitm_sl_Add > input { margin-bottom: 5px; }
    div.mitm_sl_Add > div { margin-bottom: 20px; }
</style>

<asp:Panel ID="pnlNames" runat="server" CssClass="mitm_sl_Names">
    <asp:Repeater ID="rptNames" runat="server" OnItemDataBound="rptNames_ItemDataBound">
        <HeaderTemplate></HeaderTemplate>

        <ItemTemplate>
            <div class="mitm_sl_row">
                <asp:ImageButton ID="imgStatus" runat="server" CommandName="ToggleStatus" OnCommand="imgStatus_Click" Width="48px" />
                <span class="mitm_sl_name"><%# DataBinder.Eval(Container.DataItem, "first_name") %> <%# DataBinder.Eval(Container.DataItem, "last_name") %></span>
            </div>
        </ItemTemplate>

        <FooterTemplate></FooterTemplate>
    </asp:Repeater>
</asp:Panel>

<asp:Panel ID="pnlAdd" runat="server" Visible="true" CssClass="mitm_sl_Add">
    <div>Add another name to your prayer list...</div>
    <label for="<%= tbFirstName.ClientID %>">First Name: </label>
    <asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="tbFirstNameValidator" runat="server" ValidationGroup="Add" CssClass="errorText" ControlToValidate="tbFirstName" SetFocusOnError="true" ErrorMessage="First name is required."> *</asp:RequiredFieldValidator>
    <br />
    <label for="<%= tbLastName.ClientID %>">Last Name: </label>
    <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="tbLastNameValidator" runat="server" ValidationGroup="Add" CssClass="errorText" ControlToValidate="tbLastName" SetFocusOnError="true" ErrorMessage="Last name is required."> *</asp:RequiredFieldValidator>
    <br />
    <label>&nbsp;</label>
    <Arena:ArenaButton ID="btnAdd" runat="server" Text="Add..." OnClick="btnAdd_Submit" ValidationGroup="Add" />
</asp:Panel>
