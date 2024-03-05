const changePasswordView = (passwordField,eyeIcon)=>{
    // let floatingPassword = document.getElementById("Passwordhash");
    // let eyeIcon = document.querySelector(".eye-icon");
     console.log(passwordField);
     console.log(eyeIcon);
    if (passwordField.type === "password") {
        passwordField.type = "text";
        eyeIcon.classList.remove('fa-eye')
        eyeIcon.classList.add('fa-eye-slash')
    }
    else {
        passwordField.type = "password";
        eyeIcon.classList.remove('fa-eye-slash')
        eyeIcon.classList.add('fa-eye')
    }
}