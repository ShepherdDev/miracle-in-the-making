//
// Convert any input.facheckbox elements into nice font awesome checkboxes.
//
(function ($) {
    function prepareCheckboxes() {
        $('.facheckbox').each(function () {
            var $cb = $(this);
            var $label = null;
            var $span = $('<span class="facheckbox"></span>').insertBefore($cb);

            if ($cb.attr('id')) {
                $label = $('label[for="' + $cb.attr('id') + '"]');
                if ($label.length != 1)
                    $label = null;
            }

            $vcb = $('<i class="fa fa-square-o fa-fw"></i>').appendTo($span);
            $vcb.data('checkbox', $cb);
            $cb.appendTo($span).hide();
            $cb.data('vcb', $vcb);
            if ($label)
                $label.appendTo($span);
            if ($cb.is(':checked'))
                $vcb.removeClass('fa-square-o').addClass('fa-check-square-o');

            $cb.change(function () {
                if ($cb.is(':checked'))
                    $(this).data('vcb').removeClass('fa-square-o').addClass('fa-check-square-o');
                else
                    $(this).data('vcb').removeClass('fa-check-square-o').addClass('fa-square-o');
            });
            $vcb.click(function () { $(this).data('checkbox').click(); });
        });
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        prepareCheckboxes();
    });
    $(document).ready(function () {
        prepareCheckboxes();
    });
})(jQuery);
