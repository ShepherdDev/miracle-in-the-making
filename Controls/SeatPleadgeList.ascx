<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeatPleadgeList.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.SeatPleadgeList" %>
<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlSeatPledgeList" CssClass="panel panel-block" runat="server">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-check-square-o"></i> Seat Pledge List</h1>
            </div>

            <div class="panel-body">
                <Rock:GridFilter ID="gfSettings" runat="server">
                    <Rock:CampusPicker ID="cpCampus" runat="server" Label="Campus" />
                    <Rock:RockDropDownList ID="ddlAgencyType" runat="server" Label="Agency Type" />
                </Rock:GridFilter>

                <Rock:ModalAlert ID="mdGridWarning" runat="server" />

                <Rock:Grid ID="gSeatPledges" runat="server" AllowSorting="true" OnRowSelected="gSeatPledges_Edit" TooltipField="Id">
                    <Columns>
                        <asp:BoundField DataField="PledgedPersonAlias.Person.FullNameReversed" HeaderText="Name" SortExpression="PledgedPersonAlias.Person.LastName,PledgedPersonAlias.Person.FirstName" />
                        <asp:BoundField DataField="Amount" DataFormatString="{0:C2}" HeaderText="Amount" SortExpression="Amount" />
                        <asp:BoundField DataField="AssignedSeat.FriendlyName" HeaderText="Assigned Seat" SortExpression="AssignedSeat.Section,AssignedSeat.SeatNumber" />
                        <asp:BoundField DataField="RequestedSeat.FriendlyName" HeaderText="Requested Seat" SortExpression="RequestedSeat.Section,RequestedSeat.SeatNumber" />
                        <Rock:DeleteField OnClick="gSeatPledges_Delete" />
                    </Columns>
                </Rock:Grid>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
