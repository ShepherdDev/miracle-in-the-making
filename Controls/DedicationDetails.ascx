﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DedicationDetails.ascx.cs" Inherits="RockWeb.Plugins.com_shepherdchurch.MiracleInTheMaking.DedicationDetails" %>
<%@ Register Assembly="com.shepherdchurch.MiracleInTheMaking" Namespace="com.shepherdchurch.MiracleInTheMaking.UI" TagPrefix="MITM" %>

<asp:UpdatePanel ID="upnlContent" runat="server">
    <ContentTemplate>
        <Rock:NotificationBox ID="nbWarningMessage" runat="server" NotificationBoxType="Warning" />
        <Rock:NotificationBox ID="nbEditModeMessage" runat="server" NotificationBoxType="Info" />
        <Rock:NotificationBox ID="nbInfoMessage" runat="server" NotificationBoxType="Info" />

        <asp:Panel ID="pnlDetails" runat="server" CssClass="panel panel-block mitm-dedication-details" Visible="false">
            <asp:HiddenField ID="hfDedicationId" runat="server" />

            <div class="panel-heading">
                <h1 class="panel-title">
                    <i class="fa fa-check-square-o"></i>
                    <asp:Literal ID="lActionTitle" runat="server" />
                </h1>
            </div>

            <div class="panel-body">
                <asp:ValidationSummary ID="valSummaryTop" runat="server" HeaderText="Please Correct the Following" CssClass="alert alert-danger" />

                <div class="row">
                    <div class="col-md-6">
                        <div class="row">
                            <div class="col-md-12">
                                <Rock:RockTextBox ID="tbDedicatedTo" runat="server" Label="Dedicated To" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <Rock:RockTextBox ID="tbSponsoredBy" runat="server" Label="Sponsored By" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <Rock:RockTextBox ID="tbBiography" runat="server" TextMode="MultiLine" Label="Biography" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <Rock:RockCheckBox ID="cbAnonymous" runat="server" Label="Remain Anonymous" SelectedIconCssClass="fa fa-check-square-o fa-lg" UnSelectedIconCssClass="fa fa-square-o fa-lg" />
                            </div>
                        </div>

                        <asp:Panel ID="pnlApproved" runat="server" CssClass="row" Visible="false">
                            <div class="col-md-12">
                                <Rock:RockCheckBox ID="cbApproved" runat="server" Label="Approved" SelectedIconCssClass="fa fa-check-square-o fa-lg" UnSelectedIconCssClass="fa fa-square-o fa-lg" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="col-md-6">
                        <Rock:ImageUploader ID="imgupPhoto" runat="server" Label="Photo" ThumbnailHeight="200" ThumbnailWidth="200" />
                    </div>
                </div>

                <div class="mitm-buttons">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-link" CausesValidation="false" OnClick="btnCancel_Click" />
                </div>

                <div id="divAdminActions" runat="server" Visible="false" class="mitm-buttons">
                    <asp:LinkButton ID="btnTestAdminEmail" runat="server" Text="Test Admin Email" CssClass="btn btn-default" OnClick="btnTestAdminEmail_Click" />
                    <asp:LinkButton ID="btnTestConfirmationEmail" runat="server" Text="Test Confirmation Email" CssClass="btn btn-default" OnClick="btnTestConfirmationEmail_Click" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
