$(document).ready(function () {
    //product row
    $(".clickable-row").click(function () {
        window.location = $(this).data('href');
    });



})


$(".item-gallery img").click(function () {
    var thumb = this;
    var src = this.src;
    $('.primary-img img').fadeOut(400, function () {
       // thumb.src = this.src;
        $(this).fadeIn(400)[0].src = src;
    });
})
