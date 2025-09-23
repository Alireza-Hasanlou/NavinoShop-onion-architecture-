// -------------------------
// 🧮 Validation Functions
// -------------------------

// فقط اجازه ورود عدد را می‌دهد
function isNumber(event) {
    const ASCIICode = event.which ? event.which : event.keyCode;
    if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
    return true;
}


// -------------------------
// 👤 User Dropdown Menu
// -------------------------

document.addEventListener("DOMContentLoaded", function () {
    const avatarBtn = document.getElementById("btn-login-register");
    const dropdown = document.getElementById("userDropdown");

    if (avatarBtn && dropdown) {
        avatarBtn.addEventListener("click", function (e) {
            e.stopPropagation();
            dropdown.classList.toggle("show");
        });

        document.addEventListener("click", function (e) {
            if (!avatarBtn.contains(e.target) && !dropdown.contains(e.target)) {
                dropdown.classList.remove("show");
            }
        });
    }
});


// -------------------------
// 🖼 File Upload Preview
// -------------------------

document.addEventListener("DOMContentLoaded", function () {
    const file = document.getElementById("file-upload");
    const img = document.getElementById("img");
    const text = document.getElementById("text");

    if (file && img && text) {
        file.addEventListener("change", function () {
            img.src = URL.createObjectURL(this.files[0]);
            text.innerHTML = this.files[0].name;
        });
    }
});


// -------------------------
// 💬 SweetAlert Helpers
// -------------------------

// پیام ساده
function AlerSweet(title, message, icon) {
    Swal.fire(title, message, icon);
}

function AlerSweetWithTimer(title, icon, position) {
    Swal.fire({
        position,
        icon,
        title,
        showConfirmButton: false,
        timer: 2000
    })
}

function Loding() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "flex";
}

function EndLoading() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "none";
}
