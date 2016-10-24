<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeatPleadgeList.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.SeatPleadgeList" %>
<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlSeatPledgeList" CssClass="panel panel-block" runat="server">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-check-square-o"></i> Seat Pledge List</h1>
            </div>

            <div class="panel-body">
                <Rock:GridFilter ID="gfSettings" runat="server">
                    <Rock:RockTextBox ID="tbNameFilter" runat="server" Label="Name" />
                    <Rock:RockDropDownList ID="ddlAssignedSectionFilter" runat="server" Label="Assigned Section" />
                    <Rock:NumberBox ID="tbAssignedSeatFilter" runat="server" Label="Assigned Seat #" MinimumValue="-2147483648" MaximumValue="2147483647" NumberType="Integer" TextMode="Number" />
                    <Rock:RockRadioButtonList ID="rblAssignedStatusFilter" runat="server" Label="Assigned Status">
                        <asp:ListItem Value="0" Text="Both" />
                        <asp:ListItem Value="1" Text="Assigned" />
                        <asp:ListItem Value="2" Text="Unassigned" />
                    </Rock:RockRadioButtonList>
                </Rock:GridFilter>

                <Rock:ModalAlert ID="mdGridWarning" runat="server" />

                <Rock:Grid ID="gSeatPledges" runat="server" AllowSorting="true" OnRowSelected="gSeatPledges_Edit" TooltipField="Id" PersonIdField="PledgedPersonAlias.PersonId">
                    <Columns>
                        <asp:BoundField DataField="PledgedPersonAlias.Person.FullNameReversed" HeaderText="Name" SortExpression="PledgedPersonAlias.Person.LastName,PledgedPersonAlias.Person.FirstName" />
                        <asp:BoundField DataField="Amount" DataFormatString="{0:C2}" HeaderText="Amount" SortExpression="Amount" />
                        <asp:BoundField DataField="AssignedSeat.FriendlyName" HeaderText="Assigned Seat" SortExpression="AssignedSeat.Section,AssignedSeat.SeatNumber" />
                        <asp:BoundField DataField="RequestedSeat.FriendlyName" HeaderText="Requested Seat" SortExpression="RequestedSeat.Section,RequestedSeat.SeatNumber" />
                        <Rock:LinkButtonField HeaderText="Dedication" HeaderStyle-HorizontalAlign="Center" CssClass="btn btn-default btn-sm fa fa-file-text-o" OnClick="gSeatPledges_EditDedication" ExcelExportBehavior="NeverInclude" />
                        <Rock:DeleteField OnClick="gSeatPledges_Delete" />
                    </Columns>
                </Rock:Grid>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
