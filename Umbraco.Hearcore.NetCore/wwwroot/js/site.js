// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(document).ready(function() {
    $('.spicy-button').on("click",
        function(e) {
            e.preventDefault();
            var spanToUpdate = $(this).parent().children('span');
            $.ajax({
                type: 'POST',
                url: '/Home/ThatsSpicy',
                data: {
                    'id': $(this).data("id")
                },
                success: function(msg) {
                    spanToUpdate.text(msg.spiceFactor);
                    console.log($(this).parent().children('span').text());
                }
            });

        });
});
