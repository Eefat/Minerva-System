(function ($) {
    $(function () {
        //-----> Sidebar toggle handler
        $("#menu-toggle").click(function (e) {
            e.preventDefault();
            $("#wrapper").toggleClass("active");
            $(this).find("i").toggleClass("fa-toggle-on").toggleClass("fa-toggle-off");
        });

        //-----> set spinner with ajax requests
        $(document).ajaxStart(function () {
            $.blockUI({ message: "<div><img src='../Images/Icons/ajax-loader.gif'><div>" });
        }).ajaxStop($.unblockUI);

        //-----> Scroll top control
        $("body").append("<div class='scroll-top-wrapper'><i class='fa fa-arrow-circle-up'></i></div>");
        $(document).on("scroll", function () {
            $(window).scrollTop() > 200 ? $(".scroll-top-wrapper").addClass("show") : $(".scroll-top-wrapper").removeClass("show");
        });

        $(".scroll-top-wrapper").click(function () {
            verticalOffset = typeof (verticalOffset) != "undefined" ? verticalOffset : 0;
            offset = $("body").offset();
            offsetTop = offset.top;
            $("html, body").animate({ scrollTop: offsetTop }, 500, "linear");
        });

        //check if kendo is loaded and render default controls
        if ($("script[src*='/kendo.all.js']").length > 0 || $("script[src*='/kendo.all.min.js']").length > 0 || $("script[src*='/kendo.datepicker.min.js']").length > 0) {
            $(".kendo-datepicker").kendoDatePicker(
                { format: "dd/MM/yyyy" }
            );
            $(".kendo-datetimepicker").kendoDateTimePicker({
                value: new Date(),
                format: "dd/MM/yyyy hh:mm tt",
                interval: 15
            });
        }
        
    });
})(jQuery);