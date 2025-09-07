function isNumber(event) {
    var ASCIICode = (event.which) ? event.which : event.keyCode
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;

    return true;
}

document.addEventListener("DOMContentLoaded", function () {
    const avatarBtn = document.getElementById("btn-login-register");
    const dropdown = document.getElementById("userDropdown");

    avatarBtn.addEventListener("click", function (e) {
        e.stopPropagation();
        dropdown.classList.toggle("show");
    });


    document.addEventListener("click", function (e) {
        if (!avatarBtn.contains(e.target) && !dropdown.contains(e.target)) {
            dropdown.classList.remove("show");
        }
    });
});

function AlerSweet(title, message, icon) {


    Swal.fire(  title, message, icon);
      
}
function AlerSweetWithTimer(title, icon, position) {


    Swal.fire({
        position,
        icon,
        title,
        showConfirmButton: false,
        timer: 1500
    });


}

