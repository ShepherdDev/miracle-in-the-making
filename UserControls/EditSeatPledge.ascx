<%@ Control Language="C#" CodeFile="EditSeatPledge.ascx.cs" CodeBehind="EditSeatPledge.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.SOTHC.MiTM.EditSeatPledge" %>
<%@ Register TagPrefix="Arena" Namespace="Arena.Portal.UI" Assembly="Arena.Portal.UI" %>
<%@ Register TagPrefix="MiTM" Namespace="Arena.Custom.SOTHC.MiTM.UI" Assembly="Arena.Custom.SOTHC.MiTM" %>

<style type="text/css">
    div.editSeatPledge > span { display: block; margin-bottom: 8px; }
    div.editSeatPledge > span > label { display: inline-block; width: 150px; }
</style>

<div class="editSeatPledge">
    <span>
        <label for="<%= psPerson.ClientID %>" class="formLabel">Person</label>
        <MiTM:PersonSelector ID="psPerson" runat="server" CustomRequired="true" />
        <asp:Label ID="lbPersonError" runat="server" CssClass="errorText"></asp:Label>
    </span>
    <span>
        <label for="<%= txtAmount.ClientID %>" class="formLabel">Amount</label>
        <asp:TextBox ID="txtAmount" runat="server" Width="75px" />
        <Arena:ArenaButton ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" ToolTip="Calculates the amount based on the requested seat parts." />
        <asp:Label ID="lbAmountError" runat="server" CssClass="errorText"></asp:Label>
    </span>
    <span>
        <label for="<%= txtAssignedSeat.ClientID %>" class="formLabel">Assigned Seat</label>
        <asp:TextBox ID="txtAssignedSeat" runat="server" Width="60px" />
        <Arena:ArenaButton ID="btnAutoSeat" runat="server" Text="Auto" OnClick="btnAutoSeat_Click" ToolTip="Automatically assigns an available seat in the following order: currently assigned (if any); requested seat; next available seat." />
        <asp:Label ID="lbAutoSeatMsg" runat="server" CssClass="errorText" style="color: #309030;"></asp:Label>
        <asp:Label ID="lbAutoSeatError" runat="server" CssClass="errorText"></asp:Label>
    </span>
    <span>
        <label for="<%= txtReqSeat.ClientID %>" class="formLabel">Requested Seat</label>
        <asp:TextBox ID="txtReqSeat" runat="server" Width="60px" />
        <asp:Label ID="lbReqSeatError" runat="server" CssClass="errorText"></asp:Label>
    </span>
    <span>
        <label for="<%= cbReqFullSeat.ClientID %>" class="formLabel">Requested Full Seat</label>
        <Arena:ArenaCheckBox ID="cbReqFullSeat" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqBackRest.ClientID %>" class="formLabel">Requested Back Rest</label>
        <Arena:ArenaCheckBox ID="cbReqBackRest" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqLeg1.ClientID %>" class="formLabel">Requested Leg 1</label>
        <Arena:ArenaCheckBox ID="cbReqLeg1" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqLeg2.ClientID %>" class="formLabel">Requested Leg 2</label>
        <Arena:ArenaCheckBox ID="cbReqLeg2" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqLeg3.ClientID %>" class="formLabel">Requested Leg 3</label>
        <Arena:ArenaCheckBox ID="cbReqLeg3" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqLeg4.ClientID %>" class="formLabel">Requested Leg 4</label>
        <Arena:ArenaCheckBox ID="cbReqLeg4" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqArmLeft.ClientID %>" class="formLabel">Requested Arm Left</label>
        <Arena:ArenaCheckBox ID="cbReqArmLeft" runat="server" />
    </span>
    <span>
        <label for="<%= cbReqArmRight.ClientID %>" class="formLabel">Requested Arm Right</label>
        <Arena:ArenaCheckBox ID="cbReqArmRight" runat="server" />
    </span>
    <span>
        <label for="<%= btnSave.ClientID %>" class="formLabel">&nbsp;</label>
        <Arena:ArenaButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
    </span>
</div>
