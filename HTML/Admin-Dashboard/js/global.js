$("#theme-btn").click(function(){
    if($("html").attr("data-bs-theme")==="light"){
        $("html").attr("data-bs-theme","dark")
        $("#theme-btn .fa-moon").css("display","none")
        $("#theme-btn .fa-sun").css("display","inline")

    }else{
        $("html").attr("data-bs-theme","light")
        $("#theme-btn .fa-moon").css("display","inline")
        $("#theme-btn .fa-sun").css("display","none")
    }
})