$(document).ready(function () {
    //product row
    $(".clickable-row").click(function () {
        window.location = $(this).data('href');
    });

})


$(".item-gallery img").click(function () {
    window.alert("is clicked.");
    var newSrc = $(this).attr('src');
    var oldSrc = $(this).closest('div').hasClass(".product-image img").attr('src');
    $("#my-primary-image").val = newSrc;
    $('img[src="' + oldSrc + '"]').attr('src', newSrc);
})