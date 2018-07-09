
(function () {

    $('#contactus-response-success').hide();

    $('#contactForm').ajaxForm({
        beforeSerialize: function () {

        },
        success: function (d) {

        },
        complete: function (xhr) {

            var obj = jQuery.parseJSON(xhr.responseText);

            if (obj.status == "Success") {
                //hide contactus form
                $('#contactForm').hide();
                //show response form
                $('#contactus-response-success').show();
            }
            else {
                var errorMessage = "خطا در ارسال پیغام";
                alert(errorMessage);
            }

        }
    });


})();
