<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BuildAChair.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.BuildAChair" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server" CssClass="panel panel-block" Visible="true">
            <div class="panel-heading">
                <h1 class="panel-title">
                    <i class="fa fa-check-square-o"></i>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </h1>
            </div>

            <div class="panel-body">
                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />
                <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
                <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

                <div class="row">
                    <div class="col-md-12">
                        <table class="table table-bordered mitm-bac">
                            <thead>
                                <tr>
                                    <th>Sponsorship Type</th>
                                    <th>Amount</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbFullSeat" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Full Seat</span>
                                    </td>
                                    <td>$10,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbBackRest" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Back Rest</span>
                                    </td>
                                    <td>$5,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbLeg1" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Leg 1</span>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbLeg2" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Leg 2</span>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbLeg3" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Leg 3</span>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbLeg4" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Leg 4</span>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbArmLeft" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Left Arm</span>
                                    </td>
                                    <td>$500</td>
                                </tr>

                                <tr>
                                    <td>
                                        <Rock:RockCheckBox ID="cbArmRight" runat="server" ContainerCssClass="inline-block" SelectedIconCssClass="fa fa-fw fa-check-square-o" UnSelectedIconCssClass="fa fa-fw fa-square-o" />
                                        <span>Right Arm</span>
                                    </td>
                                    <td>$500</td>
                                </tr>
                            </tbody>

                            <tfoot>
                                <tr>
                                    <td>Grand Total:</td>
                                    <td>
                                        <span></span>
                                        <asp:HiddenField ID="hfTotal" runat="server" />
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>
                </div>

                <div class="row mitm-buttons">
                    <div class="col-md-12">
                        <asp:Button ID="btnSubmit" runat="server" Text="Next" CssClass="btn btn-primary mitm-bac-submit" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
