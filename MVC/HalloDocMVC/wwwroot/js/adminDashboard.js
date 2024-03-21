
$(".InnerNav").click(function(){
    $(this).find("ul").toggle();
})
$(document).on("click", function(event){
    var $target = $(event.target);
    if(!$target.closest('.InnerNav').length){
        $(".InnerNav ul").hide();
    }
});