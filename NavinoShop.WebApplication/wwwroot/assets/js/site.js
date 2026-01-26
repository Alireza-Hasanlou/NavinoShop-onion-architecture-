// ===============================
// 🧮 Validation Helpers
// ===============================

// فقط عدد
function isNumber(event) {
    const code = event.which ? event.which : event.keyCode;
    if (code > 31 && (code < 48 || code > 57)) {
        return false;
    }
    return true;
}

// اعتبارسنجی ایمیل
function validateEmail(email) {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return pattern.test(email);
}


// ===============================
// 👤 User Dropdown Menu
// ===============================
document.addEventListener("DOMContentLoaded", function () {

    const avatarBtn = document.getElementById("btn-login-register");
    const dropdown = document.getElementById("userDropdown");

    if (!avatarBtn || !dropdown) return;

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


// ===============================
// 🖼 Image Upload Preview (jQuery Plugin)
// ===============================
(function ($) {

    $.fn.imageUploader = function (options) {

        const settings = $.extend({
            text: "لطفا یک عکس انتخاب کنید"
        }, options);

        return this.each(function () {

            const $container = $(this);
            const $input = $container.find("input[type='file']");
            const $preview = $container.find(".picture__image");

            function showImage(src) {
                const img = $("<img/>", {
                    src: src,
                    class: "picture__img"
                });
                $preview.empty().append(img);
            }

            const dataUrl = $container.data("url");
            if (dataUrl) {
                showImage(dataUrl);
            } else {
                $preview.text(settings.text);
            }

            $container.on("click", function () {
                $input.trigger("click");
            });

            $input.on("change", function () {
                const file = this.files[0];
                if (!file) {
                    $preview.text(settings.text);
                    return;
                }

                const reader = new FileReader();
                reader.onload = function (e) {
                    showImage(e.target.result);
                };
                reader.readAsDataURL(file);
            });
        });
    };

})(jQuery);


// ===============================
// 💬 SweetAlert Helpers
// ===============================
function AlerSweet(title, message, icon) {
    Swal.fire(title, message, icon);
}

function AlerSweetWithTimer(title, icon, position) {
    Swal.fire({
        title: title,
        icon: icon,
        position: position,
        showConfirmButton: false,
        timer: 3000
    });
}


// ===============================
// ⏳ Loader
// ===============================
function Loding() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "flex";
}

function EndLoading() {
    const loader = document.getElementById("loader");
    if (loader) loader.style.display = "none";
}


// ===============================
// 📧 Add User Email (AJAX)
// ===============================
function AddUsersEmail() {

    const emailInput = $("#InputUsersEmail");
    const messageBox = $("#InputUsersEmailValid");

    const email = emailInput.val().trim();

    messageBox.removeClass("text-danger text-success").text("");

    if (email === "") {
        messageBox.addClass("text-danger").text("لطفا ایمیل را وارد کنید");
        $("#InputUsersEmail").val(''); 
        return;
    }

    if (!validateEmail(email)) {
        messageBox.addClass("text-danger").text("لطفا یک ایمیل معتبر وارد کنید");
        $("#InputUsersEmail").val(''); 
        return;
    }

    Loding();

    $.ajax({
        url: "/Home/AddUserEmail",
        type: "POST",
        data: { email: email },
        dataType: "json"
    })
        .done(function (res) {

            if (res && res.success === true) {
                messageBox.addClass("text-success").text("ایمیل با موفقیت ثبت شد");
                emailInput.val("");
            } else {
                messageBox
                    .addClass("text-danger")
                    .text(res.message || "خطا در ثبت ایمیل");
            }
        })
        .fail(function () {
            messageBox
                .addClass("text-danger")
                .text("خطا در ارتباط با سرور");
        })
        .always(function () {
            $("#InputUsersEmail").val(''); 
            EndLoading();
        });
}
// ===============================
// 📧 Copy  Url
// ===============================
function copyUrl(Url) {
    // اطمینان از اینکه آدرس خالی نباشد
    if (!Url) {
        alert("آدرس  معتبر نیست!");
        return;
    }

    // اگر آدرس نسبی است، آدرس کامل ساخته می‌شود
    //const fullUrl = Url.startsWith("http")
    //    ? Url
    //    : window.location.origin + Url;

    // کپی به کلیپ‌بورد
    navigator.clipboard.writeText(Url)
        .then(() => {
            alert("✅ آدرس با موفقیت کپی شد!");
        })
        .catch(() => {
            alert("❌ خطا در کپی آدرس!");
        });
}