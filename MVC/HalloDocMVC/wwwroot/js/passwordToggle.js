const changePasswordView = ()=>{
    let floatingPassword = document.getElementById("Passwordhash");
    let eyeIcon = document.querySelector(".eye-icon");
     
    if (floatingPassword.type === "password") {
        floatingPassword.type = "text";
        eyeIcon.classList.remove('fa-eye')
        eyeIcon.classList.add('fa-eye-slash')
    }
    else {
        floatingPassword.type = "password";
        eyeIcon.classList.remove('fa-eye-slash')
        eyeIcon.classList.add('fa-eye')
    }
}