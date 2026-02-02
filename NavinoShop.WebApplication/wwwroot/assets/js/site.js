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
document.addEventListener("DOMContentLoaded", () => {

    const authBtn = document.getElementById("authBtn");
    const authMenu = document.getElementById("authMenu");

    if (!authBtn || !authMenu) return;

    authBtn.addEventListener("click", (e) => {
        e.stopPropagation();

        if (authMenu.innerHTML.trim() === "") {
            if (authBtn.dataset.fullname) {
                authMenu.innerHTML = getLoggedInMenuHtml(authBtn.dataset);
            } else {
                authMenu.innerHTML = renderGuestMenu();
            }
        }

        authMenu.classList.toggle("show");
    });


    authMenu.addEventListener("click", (e) => {
        e.stopPropagation();
    });

    document.addEventListener("click", () => {
        authMenu.classList.remove("show");
    });
});



function getLoggedInMenuHtml(data) {
    return `
        <!-- هدر پروفایل -->
        <div class="menu-header">
            <img src="${data.avatar}" class="menu-profile-img">
            <h3 class="menu-profile-name">${data.fullname}</h3>
            <p class="menu-profile-email">${data.mobile}</p>
        </div>

        <li>
            <a href="/Profile/PersonalInfo" class="auth-menu-item">
                <i class="bx bxs-dashboard"></i>
                پروفایل
            </a>
        </li>

        <li>
            <a href="/profile/edit" class="auth-menu-item">
                <i class="bx bxs-user-circle"></i>
                ویرایش پروفایل
            </a>
        </li>

        <li>
            <a href="/messages" class="auth-menu-item">
                <i class="bx bxs-message-alt"></i>
                پیام‌ها
                <span class="badge">3</span>
            </a>
        </li>

        <li>
            <a href="/settings" class="auth-menu-item">
                <i class="bx bxs-cog"></i>
                تنظیمات
            </a>
        </li>

        <li>
            <a href="/Profile/orders" class="auth-menu-item">
                <i class="bx bxs-package"></i>
                سفارش‌های من
            </a>
        </li>

        <div class="auth-menu-footer">
        <a href="/Account/Logout">
        <button class="logout-btn" id="logoutBtn">
                خروج از حساب
            </button>
            </a>
     
        </div>
    `;
}
function renderGuestMenu() {
    return `
            
            <div class="menu-header">
                <i class='bx bx-user-circle guest-icon'></i>
                <h3 class="guest-title">خوش آمدید!</h3>
                <p class="guest-subtitle">برای استفاده از امکانات کامل وارد شوید</p>
            </div>

            <!-- فرم ورود -->

            <div  class="auth-form">
                <div class="form-group">
                    <input type="email" class="form-input" placeholder="ایمیل یا شماره موبایل" id="loginEmail">
                </div>
                <div class="form-group">
                    <input type="password" class="form-input" placeholder="رمز عبور" id="loginPassword">
                </div>
                
                <div class="form-buttons">
                    <button class="btn btn-primary" onclick="login()" id="loginBtn">
                        ورود به حساب
                    </button>
                    <a href="/Account/Register">
                    <button class="btn btn-secondary" id="registerBtn">
                        ثبت‌نام جدید
                    </button>
                    <a/>
                </div>
                
                <div class="quick-links">
                    <a href="/forgot-password" class="quick-link">فراموشی رمز عبور</a>
                    <a href="/help" class="quick-link">راهنمای ورود</a>
                </div>
            </div>

            <!-- ورود سریع با شبکه‌های اجتماعی -->
            <div style="padding: 0 20px 20px;">
                <div style="text-align: center; color: #666; font-size: 13px; margin-bottom: 10px;">یا ورود سریع با</div>
                <div style="display: flex; gap: 10px;">
                    <button style="flex:1; padding: 10px; background: #3b5998; color: white; border: none; border-radius: 8px; cursor: pointer;">
                        <i class='bx bxl-facebook'></i> فیسبوک
                    </button>
                    <button style="flex:1; padding: 10px; background: #db4437; color: white; border: none; border-radius: 8px; cursor: pointer;">
                        <i class='bx bxl-google'></i> گوگل
                    </button>
                </div>
            </div>
        `;
}

// ===============================
// 🖼 Login
// ===============================

function login() {
    
    $.ajax({
        url: "/Account/Login",
        type: "POST",
        data: {
            mobile: $("#loginEmail").val(),
            password: $("#loginPassword").val(),
        },
        success: function (res) {
            if (res.success) {
                AlerSweetWithTimer("ورود با موفقیت انجام شد", "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);

            } else {
                AlerSweetWithTimer(res.message, "error", "center");
                     $("#loginEmail").val(''),
                     $("#loginPassword").val('')
            }
        },
        error: function () {
            AlerSweetWithTimer("خطا در ارتباط با سرور", "error", "center");

        }
    });

};





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

// ===============================
// User Panel
// ===============================
//Get Cities
function GetCitiesForState(stateId) {
    var cities = $("#cities");
    cities.empty();

    $.ajax({
        url: "/profile/GetCitiesForState?StateId=" + stateId,
        type: "GET",
        dataType: "json"
    })
        .done(function (res) {
            console.log(res);
            for (var i = 0; i < res.length; i++) {
                cities.append(`
                <option value="${res[i].cityCode}">${res[i].title}</option>

                `);
            }

        }).fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });

}
//Profile Image Uploader
function readImageForWidget(input, previewId) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#' + previewId).css('background-image', 'url(' + e.target.result + ')');
            $('#' + previewId).hide();
            $('#' + previewId).fadeIn(650);
        }
        reader.readAsDataURL(input.files[0]);
    }
}


