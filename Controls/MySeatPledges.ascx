<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MySeatPledges.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.MySeatPledges" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
        <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

        <asp:Panel ID="pnlSeatPledges" runat="server" Visible="false" CssClass="mitm">
            <div class="panel panel-default list-as-blocks clearfix">
                <div class="panel-body">
                    <ul>
                        <asp:Repeater ID="rpSeatPledges" runat="server">
                            <ItemTemplate>
                                <li>
                                    <a href="/page/<%= seatPledgeStatusPage.Id %>?seatPledgeId=<%# Eval( "Id" ) %>">
                                        <i><asp:Image ID="imgChair" runat="server" ImageUrl="~/Plugins/com_shepherdchurch/MiracleInTheMaking/Assets/mitm_chair-128.png" /></i>
                                        <h3><%# Eval( "AssignedSeat" ) != null ? Eval( "AssignedSeat.FriendlyName" ) : Eval( "RequestedSeat.FriendlyName" ) %></h3>
                                        <%# Eval( "AssignedSeat" ) == null ? "<h2>(requested)</h2>" : "" %>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
