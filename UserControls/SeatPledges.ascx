<%@ Control Language="C#" CodeFile="SeatPledges.ascx.cs" CodeBehind="SeatPledges.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.SeatPledges" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<div class="listFilter">
<asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnApplyFilter">
	<table cellpadding="0" cellspacing="3" border="0" width="100%">
	<tr>
		<td valign="top" rowspan="4" align="left" style="padding-left:10px;padding-top:10px; width: 50px;"><img src="images/filter.gif" border="0"></td>
		<td valign="top" class="formLabel" style="width: 175px;">Assigned Section</td>
		<td><asp:TextBox ID="txtAssignedSection" runat="server" CssClass="formItem" Width="20"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
   		<td valign="top" class="formLabel">Assigned Seat <small>("A23")</small></td>
		<td><asp:TextBox ID="txtAssignedSeat" runat="server" CssClass="formItem" Width="45"></asp:TextBox></td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td valign="top" class="formLabel">Unassigned only</td>
		<td><asp:CheckBox ID="cbUnassignedOnly" runat="server" CssClass="formItem" /></td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td style="padding-top: 15px;">
			<asp:Button ID="btnApplyFilter" Runat="server" CssClass="smallText" Text="Apply Filter" OnClick="btnApplyFilter_Click"></asp:Button>
		</td>
		<td colspan="3">&nbsp;</td>
	</tr>
	</table>
</asp:Panel>
</div>

<asp:Panel ID="pnlDataResults" runat="server">
    <Arena:DataGrid ID="dgResults" runat="server">
        <Columns>
            <asp:BoundColumn DataField="seat_pledge_id" ReadOnly="true" Visible="false"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
		    <a href='<%# DataBinder.Eval(Container.DataItem, "person_guid","default.aspx?page=7&guid={0}")%>'><%# DataBinder.Eval(Container.DataItem, "last_name") %>, <%# DataBinder.Eval(Container.DataItem, "nick_name") %></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn SortExpression="amount" DataField="amount" DataFormatString="{0:C2}" HeaderText="Amount"></asp:BoundColumn>
            <asp:BoundColumn SortExpression="assigned_seat" DataField="assigned_seat" HeaderText="Assigned Seat"></asp:BoundColumn>
            <asp:BoundColumn SortExpression="requested_seat" DataField="requested_seat" HeaderText="Requested Seat"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Dedication" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblDedication" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Unbalanced<br />Pledge" SortExpression="is_balanced">
                <ItemTemplate>
                    <asp:Label ID="lblUnbalanced" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </Arena:DataGrid>
</asp:Panel>

