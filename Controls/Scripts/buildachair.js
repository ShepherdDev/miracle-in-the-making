(function ($) {
    /* Format a number for currency use. */
    Number.prototype.formatMoney = function (c, d, t) {
        var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
        return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
    };

    function calculateTotals() {
        var amount = 0;
        $('table.mitm-bac input[type="checkbox"]:checked').each(function () {
            amount += parseInt($(this).closest('td').next().text().replace('$', '').replace(',', ''));
        });

        //
        // Update the CSS.
        //
        $('table.mitm-bac input[type="checkbox"]:checked').closest('tr').addClass('text-bold');
        $('table.mitm-bac input[type="checkbox"]:not(":checked")').closest('tr').removeClass('text-bold');

        //
        // Set the total amount.
        //
        $('table.mitm-bac tfoot td:last span').text('$' + amount.formatMoney(0, '.', ','));
        $('table.mitm-bac tfoot td:last input').val(amount);
        if (amount > 0 && amount <= 10000) {
            $('table.mitm-bac tfoot td:last').removeClass('danger').addClass('success');
            $('.mitm-bac-submit').prop('disabled', false);
        }
        else {
            $('table.mitm-bac tfoot td:last').removeClass('success').addClass('danger');
            $('.mitm-bac-submit').prop('disabled', true);
        }
    }

    function initialize() {
        $('table.mitm-bac tbody tr').click(function (e) {
            if (e.target.nodeName != 'I') {
                $(this).find('i.fa').click();
                calculateTotals();
                e.stopPropagation();
            }
            else {
                calculateTotals();
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
