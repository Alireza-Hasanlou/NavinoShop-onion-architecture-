
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
    debugger;
    var isAdmin = false;
    if (data.isuseradmin) {
        isAdmin = true;
    }
    return `
        <!-- هدر پروفایل -->
        <div class="menu-header">
            <img src="${data.avatar}" class="menu-profile-img">
            <h3 class="menu-profile-name">${data.fullname}</h3>
            <p class="menu-profile-email">${data.mobile}</p>
        </div>
           ${isAdmin ? `
        <li>
            <a href="/Admin" class="auth-menu-item">
                <i class="bx bxs-user-circle"></i>
              پنل ادمین
            </a>
        </li>` : ''}
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

    Loding();
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
    EndLoading();
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

})(jQuery)





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
    const loader = $("loader");
    if (loader) loader.style.display = "flex";
}

function EndLoading() {
    const loader = $("#loader");
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

//Price Sepatator

$("[data-format='money']").each(function () {
    var value = $(this).text();
    $(this).text(Number(value).toLocaleString('en-US'));
});
$(".money").on("input", function () {
    var value = $(this).val().replace(/,/g, '');
    $(this).val(Number(value).toLocaleString('en-US'));
});

//GetMoreTransations
function LoadWalletTransactions(pageId) {


    var parent = $(".transactions-list");
    var loadparent = $("#LoadMore");
    loadparent.html("");
    if (pageId == 0) {
        parent.html("");
    }

    $.ajax({
        url: "/profile/LoadTransaction",
        type: "GET",
        data: { PageId: pageId }
    })
        .done(function (res) {

            for (let i = 0; i < res.transactions.length; i++) {
                const transaction = res.transactions[i];

                let amountClass = '';
                let amountSign = '';


                if (transaction.transactionType === 'واریز' || transaction.transactionType === 2) {
                    amountClass = 'amount-positive';
                    amountSign = '+';
                } else {
                    amountClass = 'amount-negative';
                    amountSign = '-';
                }

                parent.append(`<div class="transaction-item">
                            <div class="transaction-info">
                                <div class="transaction-icon">⬇️</div>
                                <div class="transaction-details">
                                    <span class="transaction-name">${transaction.transactionSource}</span>
                                    <span class="transaction-date"><span class="icon-placeholder">📅</span> ${transaction.transactionDate}</span>
                                </div>
                            </div>
                            <div class="transaction-amount ${amountClass}">${amountSign}${transaction.price}</div>
                        </div>`);
            }

            if (res.pageId < res.pageCount) {
                loadparent.append(` <a onclick="LoadWalletTransactions('${res.pageId}')" style="cursor:pointer; margin:auto;" class="view-all"> بیشتر</a>`);
            }
            else if (res.dataCount > 3) {

                loadparent.append(` <a onclick="LoadWalletTransactions('0')" style="cursor:pointer; margin:auto;" class="view-all"> کمتر</a>`);
            }


        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

// Load State and Cities
$(document).ready(function () {

    /* =======================
       Elements
    ======================= */
    const $province = $("#province");
    const $city = $("#city");
    const $error = $("#errorMessage");

    console.log($province);
    /* =======================
       Load Provinces
    ======================= */
    function loadProvinces() {
        $.get("/Api/Post/States")
            .done(res => {
                fillSelect($province, res);
            })
            .fail(() => showError("خطا در بارگذاری استان‌ها"));
    }

    function fillSelect($select, items) {
        $select.empty().append('<option value="">انتخاب استان</option>');
        $.each(items, (_, item) => {
            $select.append(`<option value="${item.id}">${item.title}</option>`);
        });
    }

    /* =======================
       Load Cities
    ======================= */
    function loadCities(provinceId, $targetSelect) {
        if (!provinceId) {
            $targetSelect
                .html('<option value="" disabled selected>ابتدا استان را انتخاب کنید</option>')
                .prop("disabled", true);
            return;
        }

        $.get("/Api/Post/Cities", { stateId: provinceId })
            .done(res => {
                $targetSelect.empty();

                if (!res || res.length === 0) {
                    $targetSelect
                        .append(`<option value="" disabled>شهری یافت نشد</option>`)
                        .prop("disabled", true);
                } else {
                    $targetSelect.append('<option value="" disabled selected>انتخاب شهر</option>');

                    $.each(res, (_, city) => {
                        $targetSelect.append(
                            `<option value="${city.cityCode}">${city.title}</option>`
                        );
                    });

                    $targetSelect.prop("disabled", false);
                }
            })
            .fail(() => showError("خطا در بارگذاری شهرها"));
    }



    /* =======================
       Utils
    ======================= */
    function showError(msg) {
        $error.text(msg).show();
        setTimeout(() => $error.hide(), 4000);
    }

    /* =======================
       Events
    ======================= */
    $province.on("change", function () {

        loadCities(this.value, $city);
    });


    loadProvinces();
});

//ChargeWallet
$(document).ready(function () {
    // تبدیل ریال به تومان و نمایش زیر اینپات
    $('#rialAmount').on('input', function () {
        let rialValue = $(this).val();

        if (rialValue !== '' && rialValue > 0) {
            let tomanValue = (parseInt(rialValue) / 10).toFixed(0);
            $('#tomanHint').text('معادل تومان: ' + tomanValue + ' تومان');
        } else {
            $('#tomanHint').text('');
        }

        $('#WalleterrorMsg').text('');
    });

    // تابع شارژ
    $('#chargeBtn').on('click', function () {
        let rialAmount = $('#rialAmount').val();
        let selectedGateway = $('input[name="paymentGateway"]:checked').val();

        // بررسی خالی بودن یا صفر یا منفی
        if (!rialAmount || rialAmount <= 0) {
            $('#WalleterrorMsg').text('لطفاً مبلغ معتبر به ریال وارد کنید.');
            return;
        }

        // محاسبه تومان
        let toman = parseInt(rialAmount) / 10;

        // بررسی کمتر از 1000 تومان
        if (toman < 1000) {
            $('#WalleterrorMsg').text('مبلغ شارژ نباید کمتر از ۱۰۰۰ تومان باشد.');
            return;
        }

        // بررسی انتخاب درگاه پرداخت
        if (!selectedGateway) {
            $('#WalleterrorMsg').text('لطفاً درگاه پرداخت را انتخاب کنید.');
            return;
        }

        var chargeWalletDescription = $("#chargeWalletDescription").val();
        var portal = selectedGateway;

        // نمایش لودینگ
        Loding();

        $.ajax({
            url: "/Profile/ChargeWallet",
            type: "POST",
            data: {
                Amount: rialAmount,
                Description: chargeWalletDescription,
                Portal: portal,
            },
            dataType: "json"
        })
            .done(function (res) {
                if (res.success) {
                    closeModal();
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(res.message, "success", "center");
                    } else {
                        alert(res.message);
                    }
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                } else {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(res.message, "error", "center");
                    } else {
                        alert(res.message);
                    }
                }
            })
            .fail(function (xhr) {
                console.error("Ajax Error:", xhr.status, xhr.responseText);
                $('#WalleterrorMsg').text('خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.');
            })
            .always(function () {
                // این قسمت همیشه اجرا می‌شود (چه موفق چه خطا)
                EndLoading();
            });
    });

    // بستن مدال
    function closeModal() {
        $('#chargeModal').fadeOut();
        $('#rialAmount').val('');
        $('#tomanHint').text('');
        $('#WalleterrorMsg').text('');
        $('#chargeWalletDescription').val('');
        $('input[name="paymentGateway"]').first().prop('checked', true);
    }

    $('.WalletModal-closeBtn').on('click', closeModal);
    $(window).on('click', function (e) {
        if ($(e.target).is('#chargeModal')) closeModal();
    });
});

function open_Modal_Ajax(url, modalclass) {
  
    $('#modal-default').removeClass("product-modal");
    if (modalclass !== undefined && modalclass !== null && modalclass.trim() !== '') {
        $('#modal-default').addClass(modalclass);
    }
    Get_ajax(url);
    $('#modal-default').modal('show');
}

function Get_ajax(url) {
    var modalContent = $("#modal-content");
    modalContent.html("");
    $.get(url, function (res) {
        modalContent.html(res);
    });
}

function close_Modal_Ajax() {
    $('#modal-default').modal('hide');
}
function initProductCategoryTree() {
    $('.pct-toggle-icon').off('click').on('click', function (e) {
        e.stopPropagation();
        const $icon = $(this);
        const $children = $icon.closest('.pct-node').find('.pct-children');
        $icon.toggleClass('collapsed');
        if ($children.hasClass('collapsed')) {
            $children.removeClass('collapsed');
            $children.slideDown(300, function () {
                $(this).css('display', '');
            });
        } else {
            $children.addClass('collapsed');
            $children.slideUp(300);
        }
    });
    $('.pct-node-header').off('click').on('click', function (e) {
        if (!$(e.target).is('input') && !$(e.target).is('label') && !$(e.target).closest('label').length) {
            const $toggleIcon = $(this).find('.pct-toggle-icon');
            if ($toggleIcon.length) {
                $toggleIcon.trigger('click');
            }
        }
    });
    $('.pct-checkbox').off('change').on('change', function () {
        const $checkbox = $(this);
        const isChecked = $checkbox.is(':checked');
        const parentId = $checkbox.data('parent');
        const currentId = $checkbox.val();
        if (isChecked) {
            checkParentsInTree(parentId);
            checkChildrenInTree(currentId, true);
        } else {
            checkChildrenInTree(currentId, false);
            updateParentStateInTree(parentId);
        }
        const selectedCount = $('.pct-checkbox:checked').length;
        $('#selectedCount').text(selectedCount);
    });
    $('.pct-children').addClass('collapsed').hide();
    $('.pct-toggle-icon').addClass('collapsed');
}

function checkParentsInTree(parentId) {
    if (parentId && parentId !== 0) {
        const $parentCheckbox = $(`#pct-cat-${parentId}`);
        if ($parentCheckbox.length && !$parentCheckbox.is(':checked')) {
            $parentCheckbox.prop('checked', true);
            const grandParentId = $parentCheckbox.data('parent');
            if (grandParentId && grandParentId !== 0) {
                checkParentsInTree(grandParentId);
            }
        }
    }
}

function checkChildrenInTree(categoryId, isChecked) {
    const $parentNode = $(`#pct-cat-${categoryId}`).closest('.pct-node');
    const $childCheckboxes = $parentNode.find('.pct-children .pct-checkbox');
    $childCheckboxes.each(function () {
        const $child = $(this);
        if ($child.is(':checked') !== isChecked) {
            $child.prop('checked', isChecked);
            const childId = $child.val();
            const $grandChildren = $child.closest('.pct-node').find('.pct-children .pct-checkbox');
            if ($grandChildren.length > 0) {
                checkChildrenInTree(childId, isChecked);
            }
        }
    });
}

function updateParentStateInTree(parentId) {
    if (parentId && parentId !== 0) {
        const $parentCheckbox = $(`#pct-cat-${parentId}`);
        const $parentNode = $parentCheckbox.closest('.pct-node');
        const $siblingCheckboxes = $parentNode.find('.pct-children .pct-checkbox');
        const totalSiblings = $siblingCheckboxes.length;
        const checkedSiblings = $siblingCheckboxes.filter(':checked').length;
        const allUnchecked = checkedSiblings === 0;
        const allChecked = checkedSiblings === totalSiblings && totalSiblings > 0;
        if (allUnchecked && $parentCheckbox.is(':checked')) {
            $parentCheckbox.prop('checked', false);
            const grandParentId = $parentCheckbox.data('parent');
            if (grandParentId && grandParentId !== 0) {
                updateParentStateInTree(grandParentId);
            }
        } else if (allChecked && !$parentCheckbox.is(':checked')) {
            $parentCheckbox.prop('checked', true);
            const grandParentId = $parentCheckbox.data('parent');
            if (grandParentId && grandParentId !== 0) {
                checkParentsInTree(grandParentId);
            }
        }
    }
}
$(document).ready(function () {
    // تابع دریافت محصولات
    window.getProductsByCategories = function () {

        var selectedCategories = [];
        $('.pct-checkbox:checked').each(function () {
            var catId = parseInt($(this).val());
            if (!isNaN(catId)) {
                selectedCategories.push(catId);
            }
        });

        console.log("دسته‌بندی‌های انتخاب شده:", selectedCategories);

        // اگر چیزی انتخاب نشده
        if (selectedCategories.length === 0) {
            var productSelect = document.getElementById('productSelect');
            if (productSelect) {
                productSelect.innerHTML = '<option value="">انتخاب محصول...</option>';
            }
            return;
        }

        // نمایش لودینگ
        $('#loadingSpinner').show();
        $('#productSelect').prop('disabled', true);

        // ارسال درخواست Ajax
        $.ajax({
            url: '/Seller/GetProductsForAddToShop',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(selectedCategories),
            success: function (response) {
                console.log("پاسخ سرور:", response);
                if (response.success) {
                    updateSelect(response.data);
                } else {
                    showError(response.message || 'خطا در دریافت محصولات');
                    resetSelect();
                }
            },
            error: function (xhr, status, error) {
                console.error("Ajax Error:", status, error);
                showError('خطا در ارتباط با سرور: ' + error);
                resetSelect();
            },
            complete: function () {
                $('#loadingSpinner').hide();
                $('#productSelect').prop('disabled', false);
            }
        });
    };

    // به‌روزرسانی select معمولی
    function updateSelect(products) {
        var select = document.getElementById('productSelect');
        if (!select) return;

        // خالی کردن select
        select.innerHTML = '';

        // اضافه کردن گزینه پیش‌فرض
        var defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = 'انتخاب محصول...';
        select.appendChild(defaultOption);

        // اضافه کردن محصولات
        if (products && products.length > 0) {
            for (var i = 0; i < products.length; i++) {
                var option = document.createElement('option');
                option.value = products[i].id;
                option.textContent = products[i].title;
                select.appendChild(option);
            }
        } else {
            var emptyOption = document.createElement('option');
            emptyOption.value = '';
            emptyOption.textContent = 'محصولی یافت نشد';
            emptyOption.disabled = true;
            select.appendChild(emptyOption);
        }
    }

    // ریست کردن select
    function resetSelect() {
        var select = document.getElementById('productSelect');
        if (select) {
            select.innerHTML = '<option value="">انتخاب محصول...</option>';
        }
    }

    // نمایش خطا
    function showError(message) {
        var errorDiv = document.getElementById('errorMessage');
        if (!errorDiv) {
            errorDiv = document.createElement('div');
            errorDiv.id = 'errorMessage';
            errorDiv.style.cssText = 'display: none; color: red; margin-top: 10px; padding: 10px; background: #fee; border-radius: 5px;';
            var container = document.querySelector('.product-form-col');
            if (container) {
                container.appendChild(errorDiv);
            }
        }
        errorDiv.textContent = message;
        errorDiv.style.display = 'block';
        setTimeout(function () {
            errorDiv.style.display = 'none';
        }, 3000);
    }

    // به‌روزرسانی تعداد انتخاب شده‌ها
    function updateSelectedCount() {
        var count = $('.pct-checkbox:checked').length;
        var countSpan = document.getElementById('selectedCount');
        if (countSpan) {
            countSpan.textContent = count;
        }
    }


    $(document).off('change', '.pct-checkbox').on('change', '.pct-checkbox', function () {

        updateSelectedCount();

        var $checkbox = $(this);
        var isChecked = $checkbox.is(':checked');
        var $node = $checkbox.closest('.pct-node');

        $node.find('.pct-checkbox').prop('checked', isChecked);
        updateParentCheckbox($node);
        getProductsByCategories();
    });
    function updateParentCheckbox($node) {
        var $parentNode = $node.closest('.pct-children').closest('.pct-node');
        if ($parentNode.length) {
            var $parentCheckbox = $parentNode.find('.pct-checkbox:first');
            var $childCheckboxes = $parentNode.find('.pct-children .pct-checkbox');
            var checkedCount = $childCheckboxes.filter(':checked').length;

            if (checkedCount === 0) {
                $parentCheckbox.prop('checked', false).prop('indeterminate', false);
            } else if (checkedCount === $childCheckboxes.length) {
                $parentCheckbox.prop('checked', true).prop('indeterminate', false);
            } else {
                $parentCheckbox.prop('checked', false).prop('indeterminate', true);
            }
        }
    }

    // باز کردن/بستن زیرمجموعه‌ها
    $(document).off('click', '.pct-toggle-icon').on('click', '.pct-toggle-icon', function (e) {
        e.stopPropagation();
        var $children = $(this).closest('.pct-node').find('.pct-children');
        if ($children.is(':visible')) {
            $children.slideUp(200);
            $(this).text('▼');
        } else {
            $children.slideDown(200);
            $(this).text('▲');
        }
    });

    // مقداردهی اولیه
    updateSelectedCount();

    // اگر از قبل دسته‌بندی انتخاب شده بود
    if ($('.pct-checkbox:checked').length > 0) {
        setTimeout(function () {
            getProductsByCategories();
        }, 100);
    }
});
function CreateProductSell() {
    // 1. اعتبارسنجی اولیه
    var selectedProduct = $('#productSelect').val();
    var price = $('#Price').val();
    var weight = $('#Weight').val();
    var unit = $('#Unit').val();
    $('.product-form-control').removeClass('is-invalid');
    $('.product-form-validation').hide();


    if (!price || parseFloat(price) <= 0) {

        $('#Price').focus();
        $('.product-form-validation[data-for="Price"]').show();
        $('#Price').addClass('is-invalid');
        return;
    }


    if (!weight || parseFloat(weight) <= 0) {
     
        $('#Weight').focus();
        $('.product-form-validation[data-for="Weight"]').show();
        $('#Weight').addClass('is-invalid');
        return;
    }

    // 5. بررسی واحد فروش
    if (!unit || unit.trim() === '') {

        $('#Unit').focus();
        $('.product-form-validation[data-for="Unit"]').show();
        $('#Unit').addClass('is-invalid');
        return;
    }

    // 6. جمع‌آوری اطلاعات فرم
    var formData = new FormData();
    formData.append("ProductId", selectedProduct);
    formData.append("Price", price);
    formData.append("Weight", weight);
    formData.append("Unit", unit);
    formData.append("SellerId", $('#SellerId').val());

    // 7. ارسال به سرور
    $.ajax({
        url: '/Seller/AddProductToShop',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            // غیرفعال کردن دکمه و نمایش لودینگ
            $('.btn-primary').prop('disabled', true).text('در حال ثبت...');
            $('#loadingOverlay').fadeIn();
        },
        success: function (response) {
            if (response.success) {
                // موفقیت
                AlerSweetWithTimer("محصول با موفقیت اضافه شد", "success", "center");

                // بستن مدال
                setTimeout(function () {
                    close_Modal_Ajax();
                    // رفرش صفحه یا به‌روزرسانی لیست
                    if (typeof refreshProductList === 'function') {
                        refreshProductList();
                    } else {
                        location.reload();
                    }
                }, 1500);
            } else {
                // خطا از سمت سرور
                AlerSweetWithTimer(response.message, "error", "center");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', xhr);
            var errorMsg = 'خطا در ارتباط با سرور';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMsg = xhr.responseJSON.message;
            } else if (xhr.status === 400) {
                errorMsg = 'داده‌های ارسالی نامعتبر است';
            } else if (xhr.status === 500) {
                errorMsg = 'خطای داخلی سرور';
            }
            AlerSweetWithTimer(errorMsg, "error", "center");
        },
        complete: function () {
            // فعال کردن مجدد دکمه
            $('.btn-primary').prop('disabled', false).text('ذخیره');
            $('#loadingOverlay').fadeOut();
        }
    });
}

function EditProductSell() {
    var prodcutSellId = $('#ProductSellId').val();
    var sellerId = $('#SellerId').val();
    var price = $('#ProductSellPrice').val();
    var weight = $('#ProcutSellWight').val();
    var unit = $('#ProcutSellUnit').val();

    $('.product-form-control').removeClass('is-invalid');
    $('.product-form-validation').hide();


    if (!price || parseFloat(price) <= 0) {

        $('#ProductSellPrice').focus();
        $('.product-form-validation[data-for="Price"]').show();
        $('#ProductSellPrice').addClass('is-invalid');
        return;
    }


    if (!weight || parseFloat(weight) <= 0) {

        $('#ProcutSellWight').focus();
        $('.product-form-validation[data-for="Weight"]').show();
        $('#ProcutSellWight').addClass('is-invalid');
        return;
    }

    // 5. بررسی واحد فروش
    if (!unit || unit.trim() === '') {

        $('#ProcutSellUnit').focus();
        $('.product-form-validation[data-for="Unit"]').show();
        $('#ProcutSellUnit').addClass('is-invalid');
        return;
    }
    var formData = new FormData();
    formData.append("Id", prodcutSellId);
    formData.append("Price", price);
    formData.append("Weight", weight);
    formData.append("Unit", unit);
    formData.append("SellerId", sellerId);

    // 7. ارسال به سرور
    $.ajax({
        url: '/Seller/EditProduct',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            // غیرفعال کردن دکمه و نمایش لودینگ
            $('.btn-primary').prop('disabled', true).text('در حال ویرایش...');
            $('#loadingOverlay').fadeIn();
        },
        success: function (response) {
            if (response.success) {
                // موفقیت
                AlerSweetWithTimer("محصول با  ویرایش شد", "success", "center");

                // بستن مدال
                setTimeout(function () {
                    close_Modal_Ajax();
                    location.reload();
                }, 1500);
            } else {
                // خطا از سمت سرور
                AlerSweetWithTimer(response.message, "error", "center");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', xhr);
            var errorMsg = 'خطا در ارتباط با سرور';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMsg = xhr.responseJSON.message;
            } else if (xhr.status === 400) {
                errorMsg = 'داده‌های ارسالی نامعتبر است';
            } else if (xhr.status === 500) {
                errorMsg = 'خطای داخلی سرور';
            }
            AlerSweetWithTimer(errorMsg, "error", "center");
        },
        complete: function () {
            // فعال کردن مجدد دکمه
            $('.btn-primary').prop('disabled', false).text('ذخیره');
            $('#loadingOverlay').fadeOut();
        }
    });
}