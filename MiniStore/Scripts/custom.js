$(document).ready(function () {
    //$('.fa-search').click(function () {
    //    $('.search-bar').fadeToggle("slow", "linear");
    //})

    $(".search-bar").hide();
})

$(".fa-search").click(function () {
    $(".search-bar").animate({
        width: "toggle"
    });
});