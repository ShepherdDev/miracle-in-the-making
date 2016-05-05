<%@ Control Language="C#" CodeFile="ChairDedications.ascx.cs" CodeBehind="ChairDedications.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.ChairDedications" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>

<div class="listFilter">
<asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnApplyFilter">
	<table cellpadding="0" cellspacing="3" border="0" width="100%">
	<tr>
		<td valign="top" rowspan="4" align="left" style="padding-left:10px;padding-top:10px; width: 50px;"><img src="images/filter.gif" border="0"></td>
		<td valign="top" class="formLabel" style="width: 175px;">Approved Status</td>
		<td>
            <asp:DropDownList ID="ddlApprovedStatus" runat="server" CssClass="formItem">
                <asp:ListItem Text="Any" Value="-1" />
                <asp:ListItem Text="Unapproved" Value="0" />
                <asp:ListItem Text="Approved" Value="1" />
            </asp:DropDownList>
        </td>
		<td></td>
	</tr>
	<tr>
		<td>
			<asp:Button ID="btnApplyFilter" Runat="server" CssClass="smallText" Text="Apply Filter" OnClick="btnApplyFilter_Click"></asp:Button>
		</td>
		<td colspan="3">&nbsp;</td>
	</tr>
	</table>
</asp:Panel>
</div>

<asp:Panel ID="pnlDataResults" runat="server">
    <Arena:DataGrid ID="dgResults" runat="server"
        AllowSorting="true"
        AllowPaging="true"
        AddEnabled="false"
        BulkUpdateEnabled="false"
        DeleteEnabled="false"
        EditEnabled="true"
        ExportEnabled="true"
        MergeEnabled="false"
        ShowFooter="true" ShowHeader="true"
        OnReBind="dgResults_ReBind"
        OnItemDataBound="dgResults_ItemDataBound"
        OnEditCommand="dgResults_Edit">
        <Columns>
            <asp:BoundColumn DataField="seat_pledge_id" ReadOnly="true" Visible="false"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Name" SortExpression="last_name,nick_name">
                <ItemTemplate>
                    <a href="default.aspx?page=7&guid=<%# DataBinder.Eval(Container.DataItem, "person_guid") %>"><%# DataBinder.Eval(Container.DataItem, "last_name") %>, <%# DataBinder.Eval(Container.DataItem, "nick_name") %></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Anonymous" SortExpression="anonymous">
                <ItemTemplate><asp:Label ID="lblAnonymous" runat="server"></asp:Label></ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="assigned_seat" SortExpression="assigned_seat" HeaderText="Seat"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Approved" SortExpression="is_approved">
                <ItemTemplate><asp:Label ID="lblApproved" runat="server"></asp:Label></ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="approved_by" SortExpression="approved_by" HeaderText="Approved By"></asp:BoundColumn>
        </Columns>
    </Arena:DataGrid>
</asp:Panel>

