
// Modal Will be Open For Family,Concierge,Business
$(document).ready(function(){
    $("#myModal").modal('show');
})

function setThemePreference(theme) {
    localStorage.setItem('theme', theme);
}

// Function to retrieve the theme preference
function getThemePreference() {
    return localStorage.getItem('theme');
}

$("#theme-btn").click(function(){
    if($("html").attr("data-bs-theme")==="light"){
        $("html").attr("data-bs-theme","dark")
        $("#theme-btn .fa-moon").css("display","none")
        $("#theme-btn .fa-sun").css("display","inline")
        setThemePreference('dark');
    }else{
        $("html").attr("data-bs-theme","light")
        $("#theme-btn .fa-moon").css("display","inline")
        $("#theme-btn .fa-sun").css("display","none")
        setThemePreference('light');
    }
})

// var storedTab = localStorage.getItem("tab");
// if(storedTab){
//     $("#adminDashTab li").each(function(){
//         if($(this).data("id")==storedTab){
//             $("#adminDashTab li a").removeClass("active");
//             $(this).addClass("active").find("a").addClass("active");
//         }
//     })
// }

// $("#adminDashTab li").click(function(e) {
//     localStorage.setItem('tab', $(this).data("id"));
// })

var themePreference = getThemePreference();
    if (themePreference) {
        $("html").attr("data-bs-theme", themePreference);
        if (themePreference === "dark") {
            $("#theme-btn .fa-moon").css("display", "none");
            $("#theme-btn .fa-sun").css("display", "inline");
        } else {
            $("#theme-btn .fa-moon").css("display", "inline");
            $("#theme-btn .fa-sun").css("display", "none");
        }
    }


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
            confirmPasswordValid.innerText = "Confirm Password is Required"
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
        if(phoneNumber.includes("+")){
            const fullPhone = `${phoneNumber}`;
            $("#Mobile").val(fullPhone);
            
        }else{
            const fullPhone = `+${countryCode}${phoneNumber}`;
            $("#Mobile").val(fullPhone);
        }
    }
    if(phoneNumber1){
        if(phoneNumber.includes("+")){
            const fullPhone = `${phoneNumber1}`;
            $("#Mobile").val(fullPhone);
        }else{
            const fullPhone = `+${countryCodeForSecondPhone}${phoneNumber1}`;
            $("#Mobile").val(fullPhone);
        }
    }
    
    $("#myForm").submit();
}


window.addEventListener('popstate', function () {
    window.location.reload(true); // Reload the page, forcing server reload
});

function sendAjaxRequest(url, method="GET", data=null, content=false, process=false) {
    return new Promise(function(resolve, reject) {
        $.ajax({
            type: method,
            url: url,
            data: data,
            contentType: content,
            processData: process,
            success: function(response) {
                resolve(response);
            },
            error: function(xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/admin/login';
                } else {
                    reject(xhr.responseText || error);
                }
            }
        });
    });
}

function ToastSuccess(message){
    Toastify({
        text: message,
        duration : 3000,
        className: "success", // Use "success" class for styling success message
        close: true,
        style: {
             background: "linear-gradient(to right, #28a745, #218838)"
        }
    }).showToast();
}
function ToastError(message){
    Toastify({
        text: message,
        duration : 3000,
        close : true,
        className: "error", // Use "error" class for styling error message
        style: {
            background: "linear-gradient(to right, #dc3545, #c82333)", // Red background for error message
        }
    }).showToast();
}



function AddCountryCode(){
    const countryCode = flagAnswer && flagAnswer.s.dialCode;
    const countryCodeForSecondPhone = flagAnswer1 && flagAnswer1.s.dialCode;
    const phoneNumber = $("#Mobile").val();
    const phoneNumber1 = $("#Phone").val();
    if(phoneNumber){
        if(phoneNumber.includes("+")){
            const fullPhone = `${phoneNumber}`;
            $("#Mobile").val(fullPhone);
            
        }else{
            const fullPhone = `+${countryCode}${phoneNumber}`;
            $("#Mobile").val(fullPhone);
        }
    }
    if(phoneNumber1){
        if(phoneNumber1.includes("+")){
            const fullPhone = `${phoneNumber1}`;
            $("#Phone").val(fullPhone);
        }else{
            const fullPhone = `+${countryCodeForSecondPhone}${phoneNumber1}`;
            $("#Phone").val(fullPhone);
        }
    }
}

const SubmitFormWithPhone = (FormId) => {
    AddCountryCode();
    $(`#${FormId}`).submit();
}