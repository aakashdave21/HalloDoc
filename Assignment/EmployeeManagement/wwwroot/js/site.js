// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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