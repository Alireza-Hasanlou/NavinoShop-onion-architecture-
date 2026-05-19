function isNumber(event) {
    const code = event.which ? event.which : event.keyCode;
    if (code > 31 && (code < 48 || code > 57)) {
        return false;
    }
    return true;
}

function validateEmail(email) {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return pattern.test(email);
}

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
    var isAdmin = false;
    if (data.isuseradmin) {
        isAdmin = true;
    }
    return `
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
        <div class="auth-form">
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
                </a>
            </div>
            <div class="quick-links">
                <a href="/forgot-password" class="quick-link">فراموشی رمز عبور</a>
                <a href="/help" class="quick-link">راهنمای ورود</a>
            </div>
        </div>
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
}

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

function Loding() {
    const loader = $("loader");
    if (loader) loader.style.display = "flex";
}

function EndLoading() {
    const loader = $("#loader");
    if (loader) loader.style.display = "none";
}

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
                messageBox.addClass("text-danger").text(res.message || "خطا در ثبت ایمیل");
            }
        })
        .fail(function () {
            messageBox.addClass("text-danger").text("خطا در ارتباط با سرور");
        })
        .always(function () {
            $("#InputUsersEmail").val('');
            EndLoading();
        });
}

function copyUrl(Url) {
    if (!Url) {
        alert("آدرس معتبر نیست!");
        return;
    }
    navigator.clipboard.writeText(Url)
        .then(() => {
            alert("✅ آدرس با موفقیت کپی شد!");
        })
        .catch(() => {
            alert("❌ خطا در کپی آدرس!");
        });
}

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
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

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

$("[data-format='money']").each(function () {
    var value = $(this).text();
    $(this).text(Number(value).toLocaleString('en-US'));
});

$(".money").on("input", function () {
    var value = $(this).val().replace(/,/g, '');
    $(this).val(Number(value).toLocaleString('en-US'));
});

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

$(document).ready(function () {
    const $province = $("#province");
    const $city = $("#city");
    const $error = $("#errorMessage");

    console.log($province);

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

    function loadCities(provinceId, $targetSelect) {
        if (!provinceId) {
            $targetSelect.html('<option value="" disabled selected>ابتدا استان را انتخاب کنید</option>').prop("disabled", true);
            return;
        }
        $.get("/Api/Post/Cities", { stateId: provinceId })
            .done(res => {
                $targetSelect.empty();
                if (!res || res.length === 0) {
                    $targetSelect.append(`<option value="" disabled>شهری یافت نشد</option>`).prop("disabled", true);
                } else {
                    $targetSelect.append('<option value="" disabled selected>انتخاب شهر</option>');
                    $.each(res, (_, city) => {
                        $targetSelect.append(`<option value="${city.cityCode}">${city.title}</option>`);
                    });
                    $targetSelect.prop("disabled", false);
                }
            })
            .fail(() => showError("خطا در بارگذاری شهرها"));
    }

    function showError(msg) {
        $error.text(msg).show();
        setTimeout(() => $error.hide(), 4000);
    }

    $province.on("change", function () {
        loadCities(this.value, $city);
    });

    loadProvinces();
});

$(document).ready(function () {
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

    $('#chargeBtn').on('click', function () {
        let rialAmount = $('#rialAmount').val();
        let selectedGateway = $('input[name="paymentGateway"]:checked').val();
        if (!rialAmount || rialAmount <= 0) {
            $('#WalleterrorMsg').text('لطفاً مبلغ معتبر به ریال وارد کنید.');
            return;
        }
        let toman = parseInt(rialAmount) / 10;
        if (toman < 1000) {
            $('#WalleterrorMsg').text('مبلغ شارژ نباید کمتر از ۱۰۰۰ تومان باشد.');
            return;
        }
        if (!selectedGateway) {
            $('#WalleterrorMsg').text('لطفاً درگاه پرداخت را انتخاب کنید.');
            return;
        }
        var chargeWalletDescription = $("#chargeWalletDescription").val();
        var portal = selectedGateway;
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
                EndLoading();
            });
    });

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
        const selectedCount = $('.pct-checkbox:checked').length;
        $('#selectedCount').text(selectedCount);
    });

    $('.pct-children').addClass('collapsed').hide();
    $('.pct-toggle-icon').addClass('collapsed');
}

$(document).ready(function () {
    window.getProductsByCategories = function () {
        var selectedCategories = [];
        $('.pct-checkbox:checked').each(function () {
            var catId = parseInt($(this).val());
            if (!isNaN(catId)) {
                selectedCategories.push(catId);
            }
        });

        console.log("دسته‌بندی‌های انتخاب شده:", selectedCategories);

        if (selectedCategories.length === 0) {
            var productSelect = document.getElementById('productSelect');
            if (productSelect) {
                productSelect.innerHTML = '<option value="">انتخاب محصول...</option>';
            }
            return;
        }

        $('#loadingSpinner').show();
        $('#productSelect').prop('disabled', true);

        $.ajax({
            url: '/Profile/Seller/GetProductsForAddToShop',
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

    function updateSelect(products) {
        var select = document.getElementById('productSelect');
        if (!select) return;

        select.innerHTML = '';
        var defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = 'انتخاب محصول...';
        select.appendChild(defaultOption);

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

    function resetSelect() {
        var select = document.getElementById('productSelect');
        if (select) {
            select.innerHTML = '<option value="">انتخاب محصول...</option>';
        }
    }

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

    function updateSelectedCount() {
        var count = $('.pct-checkbox:checked').length;
        var countSpan = document.getElementById('selectedCount');
        if (countSpan) {
            countSpan.textContent = count;
        }
    }

    // تغییرات چک‌باکس - فقط شمارش و بارگذاری محصولات، بدون ارتباط بین پرنت و فرزند
    $(document).off('change', '.pct-checkbox').on('change', '.pct-checkbox', function () {
        updateSelectedCount();
        getProductsByCategories();
    });

    // باز و بسته کردن شاخه‌ها
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

    updateSelectedCount();

    if ($('.pct-checkbox:checked').length > 0) {
        setTimeout(function () {
            getProductsByCategories();
        }, 100);
    }
});
function CreateProductSell() {
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
    if (!unit || unit.trim() === '') {
        $('#Unit').focus();
        $('.product-form-validation[data-for="Unit"]').show();
        $('#Unit').addClass('is-invalid');
        return;
    }
    var formData = new FormData();
    formData.append("ProductId", selectedProduct);
    formData.append("Price", price);
    formData.append("Weight", weight);
    formData.append("Unit", unit);
    formData.append("SellerId", $('#SellerId').val());
    $.ajax({
        url: '/Seller/AddProductToShop',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('.btn-primary').prop('disabled', true).text('در حال ثبت...');
            $('#loadingOverlay').fadeIn();
        },
        success: function (response) {
            if (response.success) {
                AlerSweetWithTimer("محصول با موفقیت اضافه شد", "success", "center");
                setTimeout(function () {
                    close_Modal_Ajax();
                    if (typeof refreshProductList === 'function') {
                        refreshProductList();
                    } else {
                        location.reload();
                    }
                }, 1500);
            } else {
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
    $.ajax({
        url: '/Profile/Seller/EditProduct',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('.btn-primary').prop('disabled', true).text('در حال ویرایش...');
            $('#loadingOverlay').fadeIn();
        },
        success: function (response) {
            if (response.success) {
                AlerSweetWithTimer("محصول با موفقیت ویرایش شد", "success", "center");
                setTimeout(function () {
                    close_Modal_Ajax();
                    location.reload();
                }, 1500);
            } else {
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
            $('.btn-primary').prop('disabled', false).text('ذخیره');
            $('#loadingOverlay').fadeOut();
        }
    });
}

function GetUsersShop() {
    $('#loadingSpinner').show();
    $('#ShopSelect').prop('disabled', true);
    $.ajax({
        url: '/Profile/Stores/GetUsersShop',
        type: 'GET',
        success: function (response) {
            var select = $('#ShopSelect');
            select.empty();
            select.append('<option value="0">انتخاب فروشگاه...</option>');
            if (response && response.length > 0) {
                $.each(response, function (index, item) {
                    select.append('<option value="' + item.id + '">' + item.title + '</option>');
                });
            } else {
                select.append('<option value="0">فروشگاهی یافت نشد</option>');
            }
        },
        error: function () {
            $('#ShopSelect').html('<option value="0">خطا در بارگذاری</option>');
        },
        complete: function () {
            $('#loadingSpinner').hide();
            $('#ShopSelect').prop('disabled', false);
        }
    });
}

function CreateStore() {
    var selectedShopId = $('#ShopSelect').val();
    var storeDescription = $('#StoreDescription').val();
    $('.product-form-control').removeClass('is-invalid');
    $('.product-form-validation').hide();
    if (!selectedShopId || selectedShopId == '0') {
        $('.product-form-validation[data-for="ShopSelect"]').show();
        $('#ShopSelect').addClass('is-invalid');
        return;
    }
    if (!storeDescription || storeDescription.trim() === '') {
        $('.product-form-validation[data-for="StoreDescription"]').show();
        $('#StoreDescription').addClass('is-invalid');
        return;
    }
    var formData = new FormData();
    formData.append("SellerId", selectedShopId);
    formData.append("Description", storeDescription);
    $.ajax({
        url: '/Profile/Stores/Create',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                AlerSweetWithTimer(response.message, "success", "center");
                setTimeout(function () {
                    close_Modal_Ajax();
                    location.reload();
                }, 3000);
            } else {
                AlerSweetWithTimer(response.message, "error", "center");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', status, error);
            let errorMessage = 'خطا در ارتباط با سرور';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = xhr.responseJSON.message;
            }
            AlerSweetWithTimer(errorMessage, "error", "center");
        },
    });
}



function loadProductSells() {
    debugger;
    var sellerId = $('#SellerId').val();
    $('#loadingSpinner').show();
    $('#productsell').prop('disabled', true);

    $.ajax({
        url: '/Profile/Stores/GetProductSellsForAddToStore',
        type: 'GET',
        data: { sellerId: sellerId },
        dataType: 'json',
        success: function (response) {
            var select = $('#productsell');
            select.empty();
            select.append('<option value="">انتخاب محصول...</option>');

            if (response && response.length > 0) {
                $.each(response, function (index, item) {
                    var option = `<option value="${item.id}">${item.title}-موجودی (${item.count})</option>`
                    select.append(option);
                });
            } else {
                select.append('<option value="" disabled>محصولی یافت نشد</option>');
            }

            select.trigger('change');
        },
        error: function (xhr, status, error) {
            console.error('Error loading products:', error);
            $('#productsell').html('<option value="">خطا در بارگذاری محصولات</option>');
        },
        complete: function () {
            $('#loadingSpinner').hide();
            $('#productsell').prop('disabled', false);
        }
    });
}

function CreateStoreProduct() {
    debugger;
    var productId = $('#productsell').val();
    var storeType = $('#StoreType').val();
    var count = $('#count').val();
    var storeId = $('#storeId').val();
    $('.text-danger').text('');
    $('.form-control').removeClass('is-invalid');

    var hasError = false;

    if (!productId || productId === '') {
        $('#productsell').addClass('is-invalid');
        $('#productsell').closest('.form-group').find('.text-danger').text('لطفاً محصول را انتخاب کنید');
        hasError = true;
    }

    if (!storeType) {
        $('#StoreType').addClass('is-invalid');
        $('#StoreType').closest('.form-group').find('.text-danger').text('لطفاً نوع عملیات را انتخاب کنید');
        hasError = true;
    }

    if (!count || parseInt(count) <= 0) {
        $('#count').addClass('is-invalid');
        $('#count').closest('.form-group').find('.text-danger').text('لطفاً تعداد معتبر وارد کنید');
        hasError = true;
    }

    if (hasError) return;

    var formData = new FormData();
    formData.append("ProdcutSellId", productId);
    formData.append("StoreProductType", storeType);
    formData.append("Count", count);
    formData.append("StoreId", storeId);
    $.ajax({
        url: '/Profile/Stores/AddStoreProduct',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        beforeSend: function () {
            $('.btn-submit').prop('disabled', true).text('در حال ثبت...');
            $('#loadingOverlay').fadeIn();
        },
        success: function (response) {
            if (response.success) {
                AlerSweetWithTimer(response.message || 'محصول با موفقیت ثبت شد', "success", "center");
                setTimeout(function () {
                    close_Modal_Ajax();
                    location.reload();
                }, 2000);
            } else {
                AlerSweetWithTimer(response.message || 'خطا در ثبت محصول', "error", "center");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', xhr);
            var errorMsg = 'خطا در ارتباط با سرور';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMsg = xhr.responseJSON.message;
            }
            AlerSweetWithTimer(errorMsg, "error", "center");
        },
        complete: function () {
            $('.btn-submit').prop('disabled', false).text('ذخیره');
            $('#loadingOverlay').fadeOut();
        }
    });
};

function EitStoreDescription() {
    var storeDescription = $('#StoreDescription').val();
    var storeId = $('#storeId').val()
    $('.product-form-control').removeClass('is-invalid');
    $('.product-form-validation').hide();


    if (!storeDescription || storeDescription.trim() === '') {
        $('.product-form-validation[data-for="StoreDescription"]').show();
        $('#StoreDescription').addClass('is-invalid');
        return;
    }
    var formData = new FormData();
    formData.append("Id", storeId);
    formData.append("Description", storeDescription);
    $.ajax({
        url: '/Profile/Stores/EditStoreDescription',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                AlerSweetWithTimer(response.message, "success", "center");
                setTimeout(function () {
                    close_Modal_Ajax();
                    location.reload();
                }, 3000);
            } else {
                AlerSweetWithTimer(response.message, "error", "center");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error:', status, error);
            let errorMessage = 'خطا در ارتباط با سرور';
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = xhr.responseJSON.message;
            }
            AlerSweetWithTimer(errorMessage, "error", "center");
        },
    });
}

$(document).ready(function () {

    // ===============================
    // باز و بسته کردن زیرمجموعه‌ها با کلیک روی دکمه
    // ===============================
    $(document).on('click', '.filter-cat-toggle', function (e) {
        e.preventDefault();
        e.stopPropagation();

        const $btn = $(this);
        const categoryId = $btn.data('filter-cat-id');
        const $children = $(`#FilterCatChildren_${categoryId}`);

        $btn.toggleClass('collapsed');

        if ($children.is(':visible')) {
            $children.slideUp(300);
        } else {
            $children.slideDown(300);
        }
    });


    // ===============================
    // باز نگه داشتن زیرمجموعه‌های دارای انتخاب (در زمان لود صفحه)
    // ===============================
    function expandCheckedNodes() {
        $('.filter-cat-checkbox:checked').each(function () {
            // IMPORTANT: از data-filter-cat-id استفاده کن نه value
            const $node = $(this).closest('.filter-cat-node');
            const categoryId = $node.data('filter-cat-id');

            if (!categoryId) return;

            // باز کردن زیرمجموعه‌های گره انتخاب شده
            const $childrenDiv = $(`#FilterCatChildren_${categoryId}`);
            if ($childrenDiv.length) {
                $childrenDiv.show();
                const $toggle = $childrenDiv.siblings('.filter-cat-node-header').find('.filter-cat-toggle');
                if ($toggle.length) {
                    $toggle.removeClass('collapsed');
                }
            }
        });
    }

    // ===============================
    // دریافت اسلاگ‌های انتخاب شده (برای فیلتر محصولات)
    // ===============================
    window.getSelectedCategorySlugs = function () {
        const slugs = [];
        $('.filter-cat-checkbox:checked').each(function () {
            const slug = $(this).val();
            if (slug && slug !== '') {
                slugs.push(slug);
            }
        });
        return slugs;
    };

    // ===============================
    // دریافت Idهای انتخاب شده
    // ===============================
    window.getSelectedCategoryIds = function () {
        const ids = [];
        $('.filter-cat-checkbox:checked').each(function () {
            const $node = $(this).closest('.filter-cat-node');
            const id = $node.data('filter-cat-id');
            if (id) {
                ids.push(id);
            }
        });
        return ids;
    };

    // اجرا در زمان لود
    expandCheckedNodes();
});


//********************************************* */
// Products.js
$(document).ready(function () {
    // فقط در صفحه /products اجرا شود
    if (!window.location.pathname.toLowerCase().includes('/products')) {
        return;
    }

    // ---------- تنظیمات جاری (State) ----------
    var currentSettings = {
        minPrice: 0,
        maxPrice: 0,
        sort: 0,
        categorySlug: '',
        pageId: 1,
        search: ''
    };

    // ---------- خواندن مقادیر از URL (در ابتدا) ----------
    var urlParams = new URLSearchParams(window.location.search);
    var searchQuery = urlParams.get('search');
    if (searchQuery) {
        currentSettings.search = searchQuery;
        $('#search-input').val(searchQuery);
    }

    // خواندن sort از URL
    var sortParam = urlParams.get('sort');
    if (sortParam && sortParam != 0) {
        currentSettings.sort = parseInt(sortParam);
    }

    // خواندن page از URL
    var pageParam = urlParams.get('page');
    if (pageParam && pageParam > 1) {
        currentSettings.pageId = parseInt(pageParam);
    }

    // خواندن minPrice و maxPrice از URL
    var minPriceParam = urlParams.get('minPrice');
    if (minPriceParam && minPriceParam > 0) {
        currentSettings.minPrice = parseInt(minPriceParam);
    }
    var maxPriceParam = urlParams.get('maxPrice');
    if (maxPriceParam && maxPriceParam > 0) {
        currentSettings.maxPrice = parseInt(maxPriceParam);
    }

    // خواندن categorySlug از مسیر (مثل /Products/لپ‌تاپ)
    var pathParts = window.location.pathname.split('/').filter(part => part !== '');
    if (pathParts.length >= 2 && pathParts[0].toLowerCase() === 'products') {
        var slug = pathParts[1];
        if (slug && slug !== '') {
            currentSettings.categorySlug = decodeURIComponent(slug);
        }
    }

    // ---------- تابع به‌روزرسانی URL ----------
    function updateUrl() {
        var params = new URLSearchParams();

        if (currentSettings.search && currentSettings.search !== '') {
            params.set('search', currentSettings.search);
            $("#ResultTitle").text("نتایج جستجو برای " + currentSettings.search);
        }

        if (currentSettings.sort && currentSettings.sort != 0) {
            params.set('sort', currentSettings.sort);
        }

        if (currentSettings.pageId && currentSettings.pageId > 1) {
            params.set('page', currentSettings.pageId);
        }

        if (currentSettings.minPrice && currentSettings.minPrice > 0) {
            params.set('minPrice', currentSettings.minPrice);
        }

        if (currentSettings.maxPrice && currentSettings.maxPrice > 0 && currentSettings.maxPrice < 100000000) {
            params.set('maxPrice', currentSettings.maxPrice);
        }

        var baseUrl = '/Products';
        var newUrl = '';

        if (currentSettings.categorySlug && currentSettings.categorySlug !== '') {
            newUrl = baseUrl + '/' + encodeURIComponent(currentSettings.categorySlug);
            var categoryTitle = currentSettings.categorySlug.replace(/_/g, ' ');
            $("#ResultTitle").text("محصولات در دسته بندی " + categoryTitle);
            if (currentSettings.search && currentSettings.search !== '') {
                $("#ResultTitle").text("نتایج جستجو برای " + currentSettings.search + " در دسته " + categoryTitle);
            }
            if (params.toString()) {
                newUrl += '?' + params.toString();
            }
        } else {
            newUrl = baseUrl + (params.toString() ? '?' + params.toString() : '');
            if (currentSettings.search && currentSettings.search !== '') {
                $("#ResultTitle").text("نتایج جستجو برای " + currentSettings.search);
            } else {
                $("#ResultTitle").text("همه محصولات");
            }
        }

        window.history.pushState({ ...currentSettings }, '', newUrl);
    }

    // ---------- تنظیم کلاس‌های active مرتب‌سازی ----------
    function setActiveSortClass() {
        $('.order-filter').removeClass('active');
        $(`.order-filter[data-sort="${currentSettings.sort}"]`).addClass('active');
    }

    setActiveSortClass();

    // ---------- توابع اصلی ----------
    window.changeSort = function (sortValue) {
        currentSettings.sort = sortValue;
        currentSettings.pageId = 1;
        setActiveSortClass();
        loadProducts();
    };

    window.applyPriceRange = function () {
        if (window.priceSlider) {
            var values = window.priceSlider.get();
            var minPrice = Math.round(values[0]);
            var maxPrice = Math.round(values[1]);
            currentSettings.minPrice = minPrice;
            currentSettings.maxPrice = maxPrice;
            currentSettings.pageId = 1;
            loadProducts();
        } else {
            console.error("priceSlider not defined!");
        }
    };

    // بارگذاری محصولات با AJAX
    window.loadProducts = function (options) {
        if (options) {
            $.extend(currentSettings, options);
        }

        var requestData = {
            minPrice: currentSettings.minPrice,
            maxPrice: currentSettings.maxPrice,
            sort: currentSettings.sort,
            categorySlug: currentSettings.categorySlug,
            pageId: currentSettings.pageId,
            search: currentSettings.search,
            isAjax: true
        };

        $('#Products').html('<div class="text-center p-5"><div class="spinner-border text-primary" role="status"></div></div>');

        $.ajax({
            url: '/Products',
            type: 'GET',
            data: requestData,
            dataType: 'json',
            success: function (response) {
                if (response.success && response.products && response.products.length > 0) {
                    var productsHtml = renderProducts(response.products);
                    $('#Products').html(productsHtml);
                    if (response.pagination) {
                        renderPagination(response.pagination);
                    }
                } else if (response.success && (!response.products || response.products.length === 0)) {
                    $('#Products').html('<div class="text-center p-5 alert alert-info">محصولی یافت نشد</div>');
                } else {
                    $('#Products').html('<div class="alert alert-danger">' + (response.message || 'خطا در بارگذاری') + '</div>');
                }

                // رندر بردکرامپ
                if (response.breadCrumbs && response.breadCrumbs.length > 0) {
                    var breadCrumbsHtml = RenderBreadCrumbs(response.breadCrumbs);
                    $('#BreadCrumbsOl').html(breadCrumbsHtml);
                }

                $('html, body').animate({ scrollTop: 0 }, 300);
                updateUrl();
            },
            error: function (xhr, status, error) {
                console.error('خطا در بارگذاری محصولات:', error);
                $('#Products').html('<div class="alert alert-danger">خطا در بارگذاری محصولات: ' + error + '</div>');
            }
        });
    };

    // تابع رندر بردکرامپ
    window.RenderBreadCrumbs = function (breadCrumbs) {
        if (!breadCrumbs || breadCrumbs.length === 0) return '';
        var html = ''; 
        $.each(breadCrumbs, function (index, breadcrumb) {
            html += `<li>/<a href="${breadcrumb.url}">${breadcrumb.title}</a></li>`;
        });
        return html;
    };

    // رندر محصولات
    window.renderProducts = function (products) {
        if (!products || products.length === 0) {
            return '<div class="text-center p-5 alert alert-info">محصولی یافت نشد</div>';
        }
        var html = '';
        $.each(products, function (index, product) {
            var categoryHtml = '';

            if (product.categorySlug) {
                categoryHtml = `<a href="javascript:void(0)" onclick="loadProducts({categorySlug: '${product.categorySlug || ''}', pageId: 1})">${escapeHtml(product.category)}</a>`;
            }

            html += `
                <div class="col" style="display: block;">
                    <div class="encode4326654321vfb">
                        <a href="/Product/${product.slug}">
                            <div class="image" style="background-image: url('/Images/Product/500/${product.imageName || 'default.jpg'}');"></div>
                        </a>
                        <div class="details p-3">
                            <div class="category">
                                ${categoryHtml || '<a href="#">دسته‌بندی نشده</a>'}
                            </div>
                            <a href="/Product/${product.slug}">
                                <h2>${escapeHtml(product.title || 'بدون عنوان')}</h2>
                            </a>
                            <div class="encode4365gbf265g43d">${formatPrice(product.price || 0)} تومان</div>
                                <span><a>غرفه :@product.SellerTitle</a></span>
                            <div class="rate">
                                ${generateStarRating(product.rating || 0)}
                                <span class="encode43bf265g43d">(${product.votesCount || 0} رای دهنده)</span>
                            </div>
                        </div>
                    </div>
                </div>
            `;
        });
        return html;
    };

    // رندر صفحه‌بندی
    window.renderPagination = function (pagination) {
        if (!pagination || pagination.totalPages <= 1) return '';

        var currentPage = pagination.currentPage || currentSettings.pageId || 1;
        var totalPages = pagination.totalPages;
        var startPage = Math.max(1, currentPage - 2);
        var endPage = Math.min(totalPages, currentPage + 2);

        var paginationHtml = '<div class="unique-pagination-wrapper text-center mt-4">';
        paginationHtml += '<ul class="unique-pagination-list">';

        // دکمه قبلی
        if (currentPage > 1) {
            paginationHtml += `<li class="unique-pagination-item">
            <a class="unique-pagination-link unique-pagination-prev" href="javascript:void(0)" 
               onclick="loadProducts({pageId: ${currentPage - 1}})">
               قبلی
            </a>
        </li>`;
        }

        // صفحه اول و ...
        if (startPage > 1) {
            paginationHtml += `<li class="unique-pagination-item">
            <a class="unique-pagination-link" href="javascript:void(0)" 
               onclick="loadProducts({pageId: 1})">
               1
            </a>
        </li>`;
            if (startPage > 2) {
                paginationHtml += `<li class="unique-pagination-item">
                <span class="unique-pagination-dots">...</span>
            </li>`;
            }
        }

        // حلقه صفحات اصلی
        for (var i = startPage; i <= endPage; i++) {
            if (i === currentPage) {
                paginationHtml += `<li class="unique-pagination-item">
                <span class="unique-pagination-active">${i}</span>
            </li>`;
            } else {
                paginationHtml += `<li class="unique-pagination-item">
                <a class="unique-pagination-link" href="javascript:void(0)" 
                   onclick="loadProducts({pageId: ${i}})">
                   ${i}
                </a>
            </li>`;
            }
        }

        // صفحه آخر و ...
        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                paginationHtml += `<li class="unique-pagination-item">
                <span class="unique-pagination-dots">...</span>
            </li>`;
            }
            paginationHtml += `<li class="unique-pagination-item">
            <a class="unique-pagination-link" href="javascript:void(0)" 
               onclick="loadProducts({pageId: ${totalPages}})">
               ${totalPages}
            </a>
        </li>`;
        }

        // دکمه بعدی
        if (currentPage < totalPages) {
            paginationHtml += `<li class="unique-pagination-item">
            <a class="unique-pagination-link unique-pagination-next" href="javascript:void(0)" 
               onclick="loadProducts({pageId: ${currentPage + 1}})">
               بعدی
            </a>
        </li>`;
        }

        paginationHtml += '</ul></div>';
        $('#Products').append(paginationHtml);
        return paginationHtml;
    };
    // توابع کمکی (فرمت قیمت، escape، ستاره‌ها)
    function formatPrice(price) {
        if (!price && price !== 0) return '۰';
        return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function escapeHtml(text) {
        if (!text) return '';
        var map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#39;'
        };
        return text.replace(/[&<>"']/g, function (m) { return map[m]; });
    }

    function generateStarRating(rating) {
        var fullStars = Math.floor(rating);
        var hasHalfStar = (rating - fullStars) >= 0.5;
        var starsHtml = '';
        for (var i = 1; i <= 5; i++) {
            if (i <= fullStars) {
                starsHtml += `<svg class="svg-inline--fa fa-star fa-w-18" style="color: #ffc107; width: 18px; height: 18px; display: inline-block;" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="star" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512"><path fill="currentColor" d="M259.3 17.8L194 150.2 47.9 171.5c-26.2 3.8-36.7 36.1-17.7 54.6l105.7 103-25 145.5c-4.5 26.3 23.2 46 46.4 33.7L288 439.6l130.7 68.7c23.2 12.2 50.9-7.4 46.4-33.7l-25-145.5 105.7-103c19-18.5 8.5-50.8-17.7-54.6L382 150.2 316.7 17.8c-11.7-23.6-45.6-23.9-57.4 0z"></path></svg>`;
            } else if (i === fullStars + 1 && hasHalfStar) {
                starsHtml += `<svg class="svg-inline--fa fa-star-half-alt fa-w-17" style="color: #ffc107; width: 18px; height: 18px; display: inline-block;" aria-hidden="true" focusable="false" data-prefix="fa" data-icon="star-half-alt" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 536 512"><path fill="currentColor" d="M508.55 171.51L362.18 150.2 296.77 17.81C290.89 5.98 279.42 0 267.95 0c-11.4 0-22.79 5.9-28.69 17.81l-65.43 132.38-146.38 21.29c-26.25 3.8-36.77 36.09-17.74 54.59l105.89 103-25.06 145.48C86.98 495.33 103.57 512 122.15 512c4.93 0 10-1.17 14.87-3.75l130.95-68.68 130.94 68.7c4.86 2.55 9.92 3.71 14.83 3.71 18.6 0 35.22-16.61 31.66-37.4l-25.03-145.49 105.91-102.98c19.04-18.5 8.52-50.8-17.73-54.6zm-121.74 123.2l-18.12 17.62 4.28 24.88 19.52 113.45-102.13-53.59-22.38-11.74.03-317.19 51.03 103.29 11.18 22.63 25.01 3.64 114.23 16.63-82.65 80.38z"></path></svg>`;
            } else {
                starsHtml += `<svg class="svg-inline--fa fa-star fa-w-18" style="color: #ddd; width: 18px; height: 18px; display: inline-block;" aria-hidden="true" focusable="false" data-prefix="far" data-icon="star" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512"><path fill="currentColor" d="M528.1 171.5L382 150.2 316.7 17.8c-11.7-23.6-45.6-23.9-57.4 0L194 150.2 47.9 171.5c-26.2 3.8-36.7 36.1-17.7 54.6l105.7 103-25 145.5c-4.5 26.3 23.2 46 46.4 33.7L288 439.6l130.7 68.7c23.2 12.2 50.9-7.4 46.4-33.7l-25-145.5 105.7-103c19-18.5 8.5-50.8-17.7-54.6zM388.6 312.3l23.7 138.4L288 385.4l-124.3 65.3 23.7-138.4-100.6-98 139-20.2 62.2-126 62.2 126 139 20.2-100.6 98z"></path></svg>`;
            }
        }
        return starsHtml;
    }

    // ---------- جستجو با فشردن Enter ----------
    $('#search-input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            var newQuery = $(this).val().trim();
            if (newQuery !== currentSettings.search) {
                currentSettings.search = newQuery;
                currentSettings.pageId = 1;
                loadProducts();
            }
        }
    });

    // ---------- مدیریت دکمه بازگشت مرورگر ----------
    window.addEventListener('popstate', function (event) {
        var state = event.state || {};
        currentSettings.categorySlug = state.categorySlug || '';
        currentSettings.pageId = state.pageId || 1;
        currentSettings.sort = state.sort || 0;
        currentSettings.search = state.search || '';
        currentSettings.minPrice = state.minPrice || 0;
        currentSettings.maxPrice = state.maxPrice || 0;

        $('#search-input').val(currentSettings.search);
        setActiveSortClass();
        loadProducts();
    });

    // ---------- تابع پاک کردن همه فیلترها ----------
    window.resetAllFilters = function () {
        currentSettings = {
            minPrice: 0,
            maxPrice: 0,
            sort: 0,
            categorySlug: '',
            pageId: 1,
            search: ''
        };

        $('#search-input').val('');
        $('.order-filter').removeClass('active');
        $('.order-filter[data-sort="0"]').addClass('active');
        $("#ResultTitle").text("همه محصولات");
        $('input[name="categoryRadio"]').prop('checked', false);

        if (window.priceSlider) {
            window.priceSlider.set([0, 100000000]);
            $('#encode4365gbf265g43d-range-from').text('0');
            $('#encode4365gbf265g43d-range-to').text(formatPrice(100000000) + ' تومان');
        }
        loadProducts();
    };

});

