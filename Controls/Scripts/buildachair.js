(function ($) {
    /* Format a number for currency use. */
    Number.prototype.formatMoney = function (c, d, t) {
        var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    };

    function calculateTotals() {
        var amount = 0;
        $('table.mitm-bac input[type="checkbox"]:checked').each(function () {
            amount += $(this).data('amount');
        });

        //
        // Disable any checkboxes that are not checked but would put us over the 10,000 limit.
        //
        $('table.mitm-bac input[type="checkbox"]:not(":checked")').each(function () {
            if (amount + $(this).data('amount') > 10000) {
                $(this).closest('tr').addClass('disabled');
                $(this).prop('disabled', true);
            }
            else {
                $(this).closest('tr').removeClass('disabled');
                $(this).prop('disabled', false);
            }
        });

        //
        // Update the CSS.
        //
        $('table.mitm-bac input[type="checkbox"]:checked').closest('tr').addClass('success');
        $('table.mitm-bac input[type="checkbox"]:not(":checked")').closest('tr').removeClass('success');

        //
        // Set the total amount.
        //
        $('table.mitm-bac tfoot td:last span').text('$' + amount.formatMoney(0, '.', ','));
        if (amount > 0) {
            $('table.mitm-bac tfoot td:last').removeClass('danger').addClass('success');
            $('.mitm-bac-submit').prop('disabled', false);
        }
        else {
            $('table.mitm-bac tfoot td:last').removeClass('success').addClass('danger');
            $('.mitm-bac-submit').prop('disabled', true);
        }
    }

    function initialize() {
        $('table.mitm-bac input[type="checkbox"]').click(function (e) {
            calculateTotals();
            e.stopPropagation();
        });

        $('table.mitm-bac tbody tr').click(function (e) {
            if (e.target.nodeName == 'TD' || e.target.nodeName == 'TR') {
                $(this).find('input[type=checkbox]').click();
                e.stopPropagation();
            }
        });

        calculateTotals();
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        initialize();
    });
    $(document).ready(function () {
        initialize();
    });
})(jQuery);
