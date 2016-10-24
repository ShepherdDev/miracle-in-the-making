<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeatPledgeDetails.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.SeatPledgeDetails" %>
<%@ Register Assembly="com.shepherdchurch.MiracleInTheMaking" Namespace="com.shepherdchurch.MiracleInTheMaking.UI" TagPrefix="MITM" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" CssClass="panel panel-block" Visible="false">
            <asp:HiddenField ID="hfSeatPledgeId" runat="server" />

            <div class="panel-heading">
                <h1 class="panel-title">
                    <i class="fa fa-check-square-o"></i>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </h1>
            </div>

            <div class="panel-body">
                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
                <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
                <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

                <div class="row">
                    <div class="col-md-6">
                        <Rock:PersonPicker ID="ppPledger" runat="server" Label="Person" Required="true" />
                    </div>
                    <div class="col-md-6">
                        <MiTM:TextBox ID="tbAmount" runat="server" Label="Amount" PrependText="$" />
                        <asp:RegularExpressionValidator ID="revAmount" runat="server"
                            ControlToValidate="tbAmount" ValidationExpression="^(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$"
                            ErrorMessage="Invalid amount entered. Please enter a valid dollar amount."
                            Display="Dynamic" CssClass="validation-error help-inline" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <MITM:TextBox ID="tbAssignedSeat" runat="server" Label="Assigned Seat" />
                        <asp:RegularExpressionValidator ID="revAssignedSeat" runat="server"
                            ControlToValidate="tbAssignedSeat" ValidationExpression="^[a-zA-Z][0-9]+$"
                            ErrorMessage="Invalid seat assigned. Please enter a valid seat name."
                            Display="Dynamic" CssClass="validation-error help-inline" />
                    </div>
                    <div class="col-md-6">
                        <Rock:RockTextBox ID="tbRequestedSeat" runat="server" Label="Requested Seat" Required="true" />
                        <asp:RegularExpressionValidator ID="revRequestedSeat" runat="server"
                            ControlToValidate="tbRequestedSeat" ValidationExpression="^[a-zA-Z][0-9]+$"
                            ErrorMessage="Invalid seat requested. Please enter a valid seat name."
                            Display="Dynamic" CssClass="validation-error help-inline" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <Rock:RockCheckBoxList ID="cblRequestedParts" runat="server" Label="Requested Parts">
                            <asp:ListItem Text="Full Seat" Value="full" data-amount="10000" />
                            <asp:ListItem Text="Back Rest" Value="back" data-amount="5000" />
                            <asp:ListItem Text="Leg 1" Value="leg1" data-amount="1000" />
                            <asp:ListItem Text="Leg 2" Value="leg2" data-amount="1000" />
                            <asp:ListItem Text="Leg 3" Value="leg3" data-amount="1000" />
                            <asp:ListItem Text="Leg 4" Value="leg4" data-amount="1000" />
                            <asp:ListItem Text="Arm Left" Value="armleft" data-amount="500" />
                            <asp:ListItem Text="Arm Right" Value="armright" data-amount="500 "/>
                        </Rock:RockCheckBoxList>
                    </div>
                    <div class="col-md-6"></div>
                </div>

                <div class="actions" id="divActions" runat="server">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
