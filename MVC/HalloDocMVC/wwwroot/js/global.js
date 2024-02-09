
// Modal Will be Open For Family,Concierge,Business
$(document).ready(function(){
    $("#myModal").modal('show');
})


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

const hiddenInputMobile = document.querySelector("#hiddenInput");

const contactWithFlag = (fieldWithId)=>{
    const phoneResult = window.intlTelInput(fieldWithId, {
        preferredCountries: [ "in","us", "au", "ru"],
        hiddenInput: "full",
        utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
    return phoneResult;
}

const phoneInputField = document.querySelector("#Phone");
const phoneInputPatientField = document.querySelector("#patient-phone");
const MobileField = document.querySelector("#Mobile");

const flagAnswer = contactWithFlag(MobileField);
const flagAnswer1 = phoneInputField && contactWithFlag(phoneInputField)

const flagAnswer2 = contactWithFlag(phoneInputPatientField);







function submitForm(){
    
    
    if(document.getElementById('password-fields') && document.getElementById('password-fields').style.display == "block"){
        var confirmPasswordValid = document.querySelector('#ConfirmPasswordValid');
        var confirmPassword = document.querySelector('#ConfirmPassword').value;
        var password = document.querySelector('#Password').value;
        var PasswordhashValid = document.querySelector('#PasswordhashValid');

        if (password !== confirmPassword){
            confirmPasswordValid.innerText = "Password not matching"
            return;
        }else if(!password){
            PasswordhashValid.innerText = "Password is Required";
            return;
        }else if(!confirmPassword){
            confirmPasswordValid.innerText = "Confirm Password not matching"
            return;
        }else{
            PasswordhashValid.innerText = "";
            confirmPasswordValid.innerText = ""
        }
    }
    

    const countryCode = flagAnswer && flagAnswer.s.dialCode;
    const countryCodeForSecondPhone = flagAnswer1 && flagAnswer1.s.dialCode;
    const phoneNumber = $("#Mobile").val();
    const phoneNumber1 = $("#Phone").val();
    if(phoneNumber){
        const fullPhone = `+${countryCode}${phoneNumber}`;
        $("#Mobile").val(fullPhone);
    }
    if(phoneNumber1){
        const fullPhone1 = `+${countryCodeForSecondPhone}${phoneNumber1}`;
        $("#Phone").val(fullPhone1);
    }
    
    $("#myForm").submit();
}
