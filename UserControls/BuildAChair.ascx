<%@ control language="c#" inherits="ArenaWeb.UserControls.Custom.MITM.BuildAChair" CodeFile="BuildAChair.ascx.cs" CodeBehind="BuildAChair.ascx.cs" %>

<script type="text/javascript">
    $(document).ready(function () {
        /* Setup all the initial total values. */
        $('span.bac_totalvalue').data('amount', 0);

        /* Put quantity values of 1-9 for each line item. */
        $('select.bac_quantity').each(function () {
            var n;
            for (n = 1; n <= 9; n++) {
                $(this).append($('<option>', { value: n }).text(n));
            }
        });

        /* Setup the checkbox handlers. */
        $('img.checkbox').click(function () {
            var line = $(this).parent().parent();
            var qty = $(line).find('select.bac_quantity');
            var total = $(line).find('span.bac_totalvalue');

            if ($(this).attr('disabled') == 'true')
                return;

            if ($(this).data("checked") == "1") {
                $(this).attr("src", "UserControls/Custom/SOTHC/MiTM/Images/unchecked.png");
                $(this).data("checked", "0")
                $(qty).hide();
                $(total).hide();
                $(total).data('amount', 0);
                updateGrandTotal();
                updateCheckStates();
            }
            else {
                $(this).attr("src", "UserControls/Custom/SOTHC/MiTM/Images/checked.png");
                $(this).data("checked", "1")
                $(qty).show();
                $(total).show();
                $(qty).change();
                updateCheckStates();
            }
        });

        /* Onchange handlers for when the quantity changes. */
        $('select.bac_quantity').change(function () {
            var line = $(this).parent().parent();
            var qty = $(line).find('select.bac_quantity');
            var total = $(line).find('span.bac_totalvalue');

            /* Update the amount total for this line item. */
            var amount = $(qty).val() * $(line).attr('data-amount');
            $(total).text("$" + amount.formatMoney(0, '.', ','));
            $(total).data('amount', amount);

            updateGrandTotal();
        });
    });

    /* Update the check button states. */
    function updateCheckStates() {
        var value = $('span.bac_grandvalue').data('amount');
        var state = 0; /* 0 = nothing checked, -1 = full chair checked, 1 = anything else checked */

        var cb = $('span.bac_line[data-element="full_seat"] > span.bac_type > img');
        if (value == 10000 && cb.data('checked') == 1)
            state = -1;
        else if (value > 0)
            state = 1;

        $('img.checkbox').each(function () {
            var line = $(this).parent().parent();

            if ($(line).attr('data-element') == 'full_seat') {
                $(this).attr('disabled', (state == 1));
            }
            else {
                $(this).attr('disabled', (state == -1));
            }

            if ($(this).data('checked') != 1) {
                if ($(this).attr('disabled') == 'true')
                    $(this).attr("src", "UserControls/Custom/SOTHC/MiTM/Images/disabled.png");
                else
                    $(this).attr("src", "UserControls/Custom/SOTHC/MiTM/Images/unchecked.png");
            }
        });
    }

    /* Update the grand total value. */
    function updateGrandTotal() {
        var amount = 0;
        var data = "";

        $('span.bac_totalvalue').each(function () { amount += $(this).data('amount'); });
        $('span.bac_grandvalue').text("$" + amount.formatMoney(0, '.', ','));
        $('span.bac_grandvalue').data('amount', amount);

        $("[id$='btnAdopt']").attr('disabled', (amount == 0));
        $("[id$='hfAmount']").val(amount);

        $('img.checkbox').each(function () {
            var line = $(this).parent().parent();
            var qty = $(line).find('select.bac_quantity');

            if ($(this).data("checked") == 1) {
                data = data + $(line).attr('data-element') + '=' + $(qty).val() + ',';
            }

            $("[id$='hfChoices']").val(data);
        });
    }

    /* Format a number for currency use. */
    Number.prototype.formatMoney = function (c, d, t)
    {
        var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    };
</script>

<style type="text/css">
span.bac_header
{
    display: inline-block;
    font-size: large;
    text-align: center;
    border: 1px solid black;
    min-height: 30px;
    width: 450px;
}
span.bac_header > span { font-weight: bold; }
span.bac_line
{
    display: table-cell;
    font-size: large;
    border-left: 1px solid black;
    border-right: 1px solid black;
    border-bottom: 1px solid black;
    width: 450px;
}
.bac_type { display: inline-block; width: 200px; }
.bac_amount { display: inline-block; width: 115px; }
.bac_qty { display: none; width: 75px; }
.bac_total { display: inline-block; width: 115px; }
img.checkbox { vertical-align: middle; margin: 2px; }
select.bac_quantity { margin: 0px 0px 0px 5px; display: none; }
span.bac_totalvalue { margin: 0px 0px 0px 5px; display: none; }

span.bac_grandline
{
    display: inline-block;
    width: 452px;
}
span.bac_grandtotal
{
    display: inline-block;
    font-weight: bold;
    font-size: large;
    text-align: right;
    width: 325px;
    padding-top: 5px;
}
span.bac_grandvalue
{
    display: inline-block;
    position: absolute;
    right: 0px;
    top: 0px;
    width: 110px;
    text-align: center;
    font-weight: bold;
    font-size: large;
    padding-top: 4px;
    padding-bottom: 3px;
    border-left: 1px solid black;
    border-right: 1px solid black;
    border-bottom: 1px solid black;
}
</style>


<div id="bac_content" style="min-height: 400px;">
    <div class="bac_header">
        <span class="bac_header">
            <span class="bac_type">SPONSORSHIP TYPE</span>
            <span class="bac_amount">AMOUNT $</span>
            <span class="bac_qty">QTY</span>
            <span class="bac_total">TOTAL</span>
        </span>
    </div>
    <div class="bac_line">
        <span class="bac_line" data-amount="10000" data-element="full_seat">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Full Seat
            </span>
            <span class="bac_amount">$10,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    
    <div class="bac_line">
        <span class="bac_line" data-amount="5000" data-element="back_rest">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Back Rest
            </span>
            <span class="bac_amount">$5,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    
    <div class="bac_line">
        <span class="bac_line" data-amount="1000" data-element="leg1">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Leg 1
            </span>
            <span class="bac_amount">$1,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    <div class="bac_line">
        <span class="bac_line" data-amount="1000" data-element="leg2">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Leg 2
            </span>
            <span class="bac_amount">$1,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    <div class="bac_line">
        <span class="bac_line" data-amount="1000" data-element="leg3">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Leg 3
            </span>
            <span class="bac_amount">$1,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    <div class="bac_line">
        <span class="bac_line" data-amount="1000" data-element="leg4">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Leg 4
            </span>
            <span class="bac_amount">$1,000</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>

    <div class="bac_line">
        <span class="bac_line" data-amount="500" data-element="arm_left">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Arm Left
            </span>
            <span class="bac_amount">$500</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>
    <div class="bac_line">
        <span class="bac_line" data-amount="500" data-element="arm_right">
            <span class="bac_type">
                <img src="UserControls/Custom/SOTHC/MiTM/Images/unchecked.png" class="checkbox" alt="" />
                Arm Right
            </span>
            <span class="bac_amount">$500</span>
            <span class="bac_qty">X <select class="bac_quantity"></select></span>
            <span class="bac_total">= <span class="bac_totalvalue"></span></span>
        </span>
    </div>

    <div class="bac_line">
        <span class="bac_grandline" style="position: relative; height: 30px; overflow: hidden;">
            <span class="bac_grandtotal">GRAND TOTAL</span>
            <span class="bac_grandvalue">$0</span>
        </span>
    </div>

    <div class="bac_line">
        <asp:HiddenField ID="hfChoices" Value="" runat="server" />
        <asp:HiddenField ID="hfAmount" Value="" runat="server" />
        <asp:Button ID="btnAdopt" Enabled="false" runat="server" Text="Sponsor Seat" OnClick="btnAdopt_Click" />
    </div>
</div>
