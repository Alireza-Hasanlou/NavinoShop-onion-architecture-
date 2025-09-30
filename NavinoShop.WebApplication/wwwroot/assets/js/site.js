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
(function ($) {
    $.fn.imageUploader = function (options) {
        const defaults = {
            text: "لطفا یک عکس انتخاب کنید"
        };

        const settings = $.extend({}, defaults, options);

        return this.each(function () {
            const $container = $(this);
            const $inputFile = $container.find("input[type='file']");
            const $pictureImage = $container.find(".picture__image");

            // تابع نمایش عکس
            function showImage(src) {
                const $img = $("<img>", {
                    src: src,
                    class: "picture__img"
                });
                $pictureImage.empty().append($img);
            }

            // اگر data-url وجود داشت → عکس رو نمایش بده
            const dataUrl = $container.data("url");
            if (dataUrl && dataUrl.trim() !== "") {
                showImage(dataUrl);
            } else {
                $pictureImage.text(settings.text);
            }

            // وقتی روی کل container کلیک شد → input باز شه
            $container.on("click", function () {
                $inputFile.trigger("click");
            });

            // وقتی فایل جدید انتخاب شد
            $inputFile.on("change", function () {
                const file = this.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        showImage(e.target.result);
                    };
                    reader.readAsDataURL(file);
                } else {
                    $pictureImage.text(settings.text);
                }
            });
        });
    };
})(jQuery);


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
