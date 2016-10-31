<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SalvationList.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.SalvationList" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />

        <asp:Panel ID="pnlSalvationList" CssClass="panel panel-block" runat="server" Visible="false">
            <div class="panel-heading">
                <h1 class="panel-title"><i class="fa fa-check-square-o"></i> Salvation List</h1>
            </div>

            <div class="panel-body">
                <Rock:ModalAlert ID="mdGridWarning" runat="server" />

                <Rock:Grid ID="gSalvations" runat="server" AllowSorting="true" OnRowSelected="gSalvations_ToggleStatus" CssClass="mitm-sl" AllowPaging="false">
                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="LastName,FirstName" />
                        <Rock:BoolField DataField="IsSaved" HeaderText="Saved?" HeaderStyle-HorizontalAlign="Center" SortExpression="IsSaved" />
                        <Rock:DeleteField OnClick="gSalvations_Delete" />
                    </Columns>
                </Rock:Grid>
            </div>
        </asp:Panel>

        <Rock:ModalDialog ID="mdlNewSalvation" runat="server" Title="Add Salvation" ValidationGroup="vgNewSalvation" OnSaveClick="mdlNewSalvation_SaveClick" SaveButtonText="Save">
            <Content>
                <Rock:NotificationBox ID="nbNewSalvationInfo" runat="server" Text="Please enter the name of the person you are praying for." NotificationBoxType="Info" />

                <Rock:RockTextBox ID="tbNewSalvationFirstName" runat="server" Label="First Name" Required="true" ValidationGroup="vgNewSalvation" />
                <Rock:RockTextBox ID="tbNewSalvationLastName" runat="server" Label="Last Name" Required="true" ValidationGroup="vgNewSalvation" />
            </Content>
        </Rock:ModalDialog>
    </ContentTemplate>
</asp:UpdatePanel>
