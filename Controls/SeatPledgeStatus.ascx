<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SeatPledgeStatus.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.SeatPledgeStatus" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
        <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

        <asp:Panel ID="pnlSeatPledges" runat="server" Visible="false" CssClass="mitm-pledge-status">
            <div class="row">
                <div class="col-md-6">
                    <div class="mitm-chair-thermo">
                        <div class="outline">
                            <asp:Image ID="imgChairOutline" runat="server" ImageUrl="~/Plugins/com_shepherdchurch/MiracleInTheMaking/Assets/chair-outline.png" />
                        </div>
                        <div class="fillcontainer">
                            <div id="tdFillAmount" runat="server" class="fill">
                                <asp:Image ID="imgChairFill" runat="server" ImageUrl="~/Plugins/com_shepherdchurch/MiracleInTheMaking/Assets/chair-fill.png" />
                            </div>
                        </div>
                    </div>

                    <div class="mitm-chair-name">
                        <asp:Literal ID="ltChairName" runat="server" />
                    </div>
                </div>

                <div class="col-md-6">
                    <table class="table table-bordered table-hover">
                        <tbody>
                            <tr>
                                <td>Pledged by me</td>
                                <td id="tdPledgedByMe" runat="server"></td>
                            </tr>

                            <tr>
                                <td>Paid by me</td>
                                <td id="tdPaidByMe" runat="server"></td>
                            </tr>

                            <tr>
                                <td>Remaining for me</td>
                                <td id="tdRemainingForMe" runat="server"></td>
                            </tr>

                            <tr>
                                <td>Pledged by others</td>
                                <td id="tdPledgedByOthers" runat="server"></td>
                            </tr>

                            <tr>
                                <td>Needs to be adopted</td>
                                <td id="tdNeedsToBeAdopted" runat="server"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <div class="row mitm-buttons">
                <div class="col-md-12">
                    <asp:Button ID="btnPay" runat="server" CssClass="btn btn-primary" OnClick="btnPay_Click" Text="Give Now" />
                    <asp:Button ID="btnAdoptFull" runat="server" CssClass="btn btn-default" OnClick="btnAdoptFull_Click" Text="Adopt Full Seat" />
                    <asp:Button ID="btnDedicate" runat="server" CssClass="btn btn-default" OnClick="btnDedicate_Click" Text="Dedicate Seat" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
