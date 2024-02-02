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

const contactWithFlag = (fieldWithId)=>{
    const phoneInput = window.intlTelInput(fieldWithId, {
        preferredCountries: [ "in","us", "au", "ru"],
        utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
}

const phoneInputField = document.querySelector("#phone");
const phoneInputPatientField = document.querySelector("#patient-phone");
contactWithFlag(phoneInputField)
contactWithFlag(phoneInputPatientField)