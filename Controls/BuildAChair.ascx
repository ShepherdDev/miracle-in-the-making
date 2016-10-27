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
                                        <input type="checkbox" class="facheckbox" id="cbFullSeat" runat="server" data-amount="10000" />
                                        <label for="<%= cbFullSeat.ClientID %>">Full Seat</label>
                                    </td>
                                    <td>$10,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbBackRest" runat="server" data-amount="5000" />
                                        <label for="<%= cbBackRest.ClientID %>">Back Rest</label>
                                    </td>
                                    <td>$5,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbLeg1" runat="server" data-amount="1000" />
                                        <label for="<%= cbLeg1.ClientID %>">Leg 1</label>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbLeg2" runat="server" data-amount="1000" />
                                        <label for="<%= cbLeg2.ClientID %>">Leg 2</label>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbLeg3" runat="server" data-amount="1000" />
                                        <label for="<%= cbLeg3.ClientID %>">Leg 3</label>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbLeg4" runat="server" data-amount="1000" />
                                        <label for="<%= cbLeg4.ClientID %>">Leg 4</label>
                                    </td>
                                    <td>$1,000</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbArmLeft" runat="server" data-amount="500" />
                                        <label for="<%= cbArmLeft.ClientID %>">Left Arm</label>
                                    </td>
                                    <td>$500</td>
                                </tr>

                                <tr>
                                    <td>
                                        <input type="checkbox" class="facheckbox" id="cbArmRight" runat="server" data-amount="500" />
                                        <label for="<%= cbArmRight.ClientID %>">Right Arm</label>
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
