<%@ Control Language="C#" CodeFile="AdminUtils.ascx.cs" CodeBehind="AdminUtils.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.AdminUtils" %>


<div style="margin-bottom: 20px; border: 1px solid black;">
    <div>Create seats:</div>
    Section: <asp:TextBox ID="tbCreateSection" runat="server" Width="20px">A</asp:TextBox><br />
    Starting Seat Number: <asp:TextBox ID="tbCreateStart" runat="server" Width="60px">1</asp:TextBox><br />
    Ending Seat Number: <asp:TextBox ID="tbCreateEnd" runat="server" Width="60px">100</asp:TextBox><br />
    <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click" />
</div>

