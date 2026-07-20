/*const { debug } = require("util");*/

function verifyCartSaved() {
    try {
        const cookieData = Cookies.get('NavinoshoppingCart');
        const localStorageData = localStorage.getItem('NavinoshoppingCart');
        if (!cookieData && localStorageData) {
            Cookies.set('NavinoshoppingCart', localStorageData, {
                expires: 7,
                path: '/',
                sameSite: 'Lax'
            });
        }
    } catch (e) {
        console.error('Error verifying cart:', e);
    }
}

function getCartCount() {
    $.ajax({
        url: '/Cart/GetCartCount',
        type: 'GET',
        cache: false,
        success: function (response) {
            if (response.success) {
                updateCartBadge(response.cartCount);
            }
        },
        error: function (xhr) {
            console.error('Error getting cart count:', xhr);
        }
    });
}
function updateCartBadge(count) {
    $('#cart-count').text(count);
    if (count > 0) {
        $('#cart-count').show();
    } else {
        $('#cart-count').hide();
    }
    $('.cart-badge, .cart-count-header, .cart-item-count').each(function () {
        if (count > 0) {
            $(this).text(count).show();
        } else {
            $(this).text('0').hide();
        }
    });
}

function addToCart(productSellId, amount) {
    const productId = String(productSellId).trim();

    var quantity = $("#ProductCount").val();
    const validAmount = amount && !isNaN(parseFloat(amount)) && parseFloat(amount) > 0 ? parseFloat(amount) : 0;

    if (validAmount === 0) {
        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('این محصول موجودی ندارد!', "error", "Center");
        } else {
            alert('این محصول موجودی ندارد!');
        }
        return;
    }

    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: {
            productSellId: productId,
            quantity: quantity
        },
        success: function (response) {
            if (response.success) {
                loadCartFromServer();
                getCartCount();
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'محصول به سبد خرید اضافه شد!', "success", "Center");
                }
            } else {
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'خطا در افزودن به سبد خرید', "error", "Center");
                }
            }
        },
        error: function () {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('خطا در ارتباط با سرور', "error", "Center");
            }
        }
    });
}

function removeFromCart(productSellId) {
    const productId = String(productSellId).trim();

    if (!confirm('آیا از حذف این محصول از سبد خرید مطمئن هستید؟')) {
        return;
    }

    $.ajax({
        url: '/Cart/RemoveFromCart',
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                loadCartFromServer();
                getCartCount();
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'محصول از سبد خرید حذف شد!', "success", "Center");
                }
            } else {
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'خطا در حذف محصول', "error", "Center");
                }
            }
        },
        error: function () {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('خطا در ارتباط با سرور', "error", "Center");
            }
        }
    });
}

function updateQuantity(productSellId, change) {
    const productId = String(productSellId).trim();
    debugger;
    $.ajax({
        url: '/Cart/UpdateQuantity',
        type: 'POST',
        data: { productId: productId, change: change },
        success: function (response) {
            if (response.success) {
                loadCartFromServer();
                getCartCount();
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'تعداد محصول به‌روزرسانی شد!', "success", "Center");
                }
            } else {
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'خطا در به‌روزرسانی تعداد', "error", "Center");
                }
            }
        },
        error: function () {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('خطا در ارتباط با سرور', "error", "Center");
            }
        }
    });
}
function clearCart() {
    if (!confirm('آیا از خالی کردن سبد خرید مطمئن هستید؟')) return;

    $.ajax({
        url: '/Cart/ClearCart',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                loadCartFromServer();
                getCartCount();
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'سبد خرید خالی شد!', "success", "Center");
                    setTimeout(function () {
                        window.location.reload();
                    }, 3000);

                }
            } else {
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || 'خطا در خالی کردن سبد خرید', "error", "Center");
                }
            }
        },
        error: function () {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('خطا در ارتباط با سرور', "error", "Center");
            }
        }
    });
}

function loadCartFromServer() {
    $.ajax({
        url: '/Cart/GetCartData',
        type: 'GET',
        cache: false,
        success: function (response) {
            if (response.success) {
                updateCartDisplay(response);
                updateCartBadge(response.totalItems);

                if (response.items && response.items.length > 0) {
                    $('#clear-cart-btn').show();
                    $('.checkout-btn').removeClass('disabled');
                } else {
                    $('#clear-cart-btn').hide();
                    $('.checkout-btn').addClass('disabled');
                }
            }
        },
        error: function () {
            console.error('Error loading cart from server');
        }
    });
}

function updateCartDisplay(response) {
    const cartContainer = $('#cart-items-dropdown');
    const footerContainer = $('#cart-footer');

    if (!cartContainer.length) return;

    if (!response.success) {
        showAlert(response.message || 'خطا در دریافت اطلاعات سبد خرید', 'error');
        return;
    }

    const hasItems = response.items && response.items.length > 0;

    if (hasItems) {
        let itemsHtml = '';

        $.each(response.items, function (index, item) {
            const hasDiscount = item.hasDiscount;
            const finalPrice = hasDiscount ? item.priceAfterOff : item.price;
            const itemTotal = item.totalPrice || (finalPrice * item.quantity);
            const isOutOfStock = !item.hasStock || item.amount <= 0;
            const maxAmount = item.amount || 0;
            const imagePath = item.imageName;
            const productUrl = '/product/' + item.id;

            let priceHtml = '';
            if (isOutOfStock) {
                priceHtml = `<span style="color: #dc3545; font-weight: bold;">ناموجود</span>`;
            } else if (hasDiscount) {
                priceHtml = `
                    <span style="text-decoration: line-through; color: #999; font-size: 11px;">${formatPrice(item.price)}</span>
                    <span style="color: #dc3545; font-weight: bold;">${formatPrice(item.priceAfterOff)}</span>
                `;
            } else {
                priceHtml = `<span style="color: #28a745; font-weight: bold;">${formatPrice(item.price)}</span>`;
            }

            itemsHtml += `
                <div class="dropdown-cart-item" data-id="${item.id}" data-max="${maxAmount}">
                    <div class="dropdown-cart-item-image">
                        <a href="${productUrl}" target="_blank">
                            <img src="${imagePath}" alt="${escapeHtml(item.title)}">
                        </a>
                    </div>
                    <div class="dropdown-cart-item-info">
                        <div class="dropdown-cart-item-title">
                            <a href="${productUrl}" target="_blank" class="text-dark text-decoration-none">
                                ${escapeHtml(item.title)}
                            </a>
                        </div>
                        <div class="dropdown-cart-item-seller">${escapeHtml(item.seller || '')}</div>
                        <div class="dropdown-cart-item-price">
                            ${priceHtml}
                            ${!isOutOfStock ? `<span style="color: #28a745; margin-right: 5px;">× ${item.quantity} = ${formatPrice(itemTotal)}</span>` : ''}
                        </div>
                        ${!isOutOfStock && maxAmount <= 5 ? `<span style="color: #dc3545; font-size: 11px; margin-right: 5px; font-weight: bold;">فقط ${maxAmount} عدد باقی مانده</span>` : ''}
                    </div>
                    <div class="dropdown-cart-item-actions">
                        ${!isOutOfStock ? `
                            <button class="dropdown-quantity-btn update-quantity" data-id="${item.id}" data-change="-1" ${item.quantity <= 1 ? 'disabled' : ''}>-</button>
                            <span class="dropdown-item-quantity">${item.quantity}</span>
                            <button class="dropdown-quantity-btn update-quantity" data-id="${item.id}" data-change="1" ${item.quantity >= maxAmount ? 'disabled' : ''}>+</button>
                        ` : `
                            <span style="color: #dc3545; font-size: 12px;">ناموجود</span>
                        `}
                        <button class="dropdown-remove-btn remove-item" data-id="${item.id}">
                            <i class="fa fa-trash"></i>
                        </button>
                    </div>
                </div>
            `;
        });

        const totalAmount = response.totalAmount || 0;
        const totalDiscount = response.totalDiscount || 0;
        const finalTotal = totalAmount - totalDiscount;

        const footerHtml = `
            <div class="dropdown-total">
                <span>مجموع سبد خرید:</span>
                <span style="color: #28a745; font-weight: bold;">${formatPrice(finalTotal)}</span>
                ${totalDiscount > 0 ? `<span style="color: #dc3545; font-size: 11px; margin-right: 10px;">(تخفیف: ${formatPrice(totalDiscount)})</span>` : ''}
            </div>
            <div class="dropdown-actions" style="display: flex; gap: 5px; padding: 8px 0;">
                <button class="btn btn-outline-danger btn-sm" id="clear-cart-btn" style="flex: 1;">
                    <i class="fa fa-trash"></i> خالی کردن
                </button>
                <a href="/Cart" class="btn btn-view-cart" style="flex: 1;">
                    <i class="fa fa-eye"></i> مشاهده سبد خرید
                </a>
            </div>
        `;

        cartContainer.html(itemsHtml);
        footerContainer.html(footerHtml).show();
    } else {
        cartContainer.html(`
            <div class="empty-cart-dropdown">
                <i class="fa fa-shopping-cart" style="font-size: 48px; color: #ddd;"></i>
                <p style="margin-top: 10px;">سبد خرید شما خالی است 🛒</p>
            </div>
        `);
        footerContainer.hide();
    }

    // اتصال رویدادها
    attachEvents();
    attachDropdownEvents();
}

// ======================================================
// توابع کمکی
// ======================================================

function formatPrice(price) {
    if (!price) return '۰ تومان';
    return new Intl.NumberFormat('fa-IR').format(price) + ' تومان';
}

function formatDiscount(discount) {
    if (!discount) return '';
    return new Intl.NumberFormat('fa-IR').format(discount) + ' تومان';
}

function escapeHtml(text) {
    if (!text) return '';
    var div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function showAlert(message, type) {
    if (typeof AlerSweetWithTimer === 'function') {
        AlerSweetWithTimer(message, type, 'Center');
    } else {
        alert(message);
    }
}

function showError(message) {
    console.error('Cart Error:', message);
}

// ======================================================
// اتصال رویدادها
// ======================================================

function attachEvents() {
    // رویداد تغییر تعداد
    $('.update-quantity').off('click').on('click', function () {
        var id = $(this).data('id');
        var change = $(this).data('change');
        updateQuantity(id, change);
    });

    // رویداد حذف آیتم
    $('.remove-item').off('click').on('click', function () {
        var id = $(this).data('id');
        removeFromCart(id);
    });

    // رویداد خالی کردن سبد
    $('#clear-cart-btn').off('click').on('click', function () {
        clearCart();

    });
}

function attachDropdownEvents() {
    // مدیریت باز و بسته شدن منوی سبد خرید
}

// ======================================================
// مقداردهی اولیه
// ======================================================

$(document).ready(function () {
    verifyCartSaved();

    getCartCount();


    // مدیریت منوی کشویی سبد خرید - یک کلیک
    if ($('.cart-dropdown-menu').length && $('.cart-toggle-btn').length) {
        $('.cart-toggle-btn').off('click').on('click', function (e) {
            e.stopPropagation();
            e.preventDefault();
            const $dropdown = $('.cart-dropdown-menu');
            $dropdown.toggleClass('show');
            if ($dropdown.hasClass('show')) {
                loadCartFromServer();
            }
        });
    }

    // بستن منو با کلیک خارج
    $(document).off('click').on('click', function (e) {
        if (!$(e.target).closest('.cart-dropdown-container').length) {
            $('.cart-dropdown-menu').removeClass('show');
        }
    });

    // جلوگیری از بسته شدن منو با کلیک داخل
    $('.cart-dropdown-menu').off('click').on('click', function (e) {
        e.stopPropagation();
    });

    // بستن منو با دکمه بستن
    $('#close-dropdown').off('click').on('click', function () {
        $('.cart-dropdown-menu').removeClass('show');
    });

    // بارگذاری مجدد هنگام بازگشت به صفحه
    window.addEventListener('pageshow', function (event) {
        if (event.persisted) {
            getCartCount();
            loadCartFromServer();
        }
    });

    // بارگذاری مجدد هنگام تغییر visibility
    document.addEventListener('visibilitychange', function () {
        if (!document.hidden) {
            getCartCount();
        }
    });
});

function syncCartFromCookie() {
    $.ajax({
        url: '/Cart/SyncCartFromCookie',
        type: 'POST',
        success: function (response) {
            if (response.success) {
                if (response.cartCount !== undefined) {
                    updateCartBadge(response.cartCount);
                }

                loadCartFromServer();
            }
        },
        error: function (xhr) {
            console.error('خطا در ارتباط با سرور:', xhr);

        }
    });
}
// ============================================================
//OrderDiscounts
// ============================================================

(function () {
    'use strict';

    function invFormatNumber(num) {
        if (!num && num !== 0) return '۰';
        return num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    window.invApplySellerDiscount = function (sellerId) {
        var discountCode = $(`#invDiscountInput_${sellerId}`).val();

        if (!discountCode || discountCode.trim() === '') {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('لطفاً کد تخفیف را وارد کنید!', "error", "Center");
            } else {
                alert('لطفاً کد تخفیف را وارد کنید!');
            }
            return;
        }

        var btn = $(`#invDiscountBtn_${sellerId}`);
        btn.prop('disabled', true);
        btn.html('<i class="fas fa-spinner fa-spin"></i> در حال اعمال...');

        $.ajax({
            url: '/order/ApplySellerDiscount',
            type: 'POST',
            data: {
                sellerId: sellerId,
                Code: discountCode.trim()
            },
            success: function (response) {
                if (response.success) {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "success", "Center");
                    }
                    if (response.data) {
                        invUpdateSellerDiscountUI(sellerId, response.data);
                    } else {
                        invUpdateSellerDiscountUI(sellerId, response);
                    }
                } else {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "error", "Center");
                    } else {
                        alert(response.message);
                    }
                    btn.prop('disabled', false);
                    btn.html('<i class="fas fa-check"></i> اعمال');
                }
            },
            error: function (xhr) {
                var errorMessage = 'خطا در ارتباط با سرور.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(errorMessage, "error", "Center");
                } else {
                    alert(errorMessage);
                }
                btn.prop('disabled', false);
                btn.html('<i class="fas fa-check"></i> اعمال');
            }
        });
    };

    window.invUpdateSellerDiscountUI = function (sellerId, data) {
        var isSuccess = data.success === true;
        var discountPercent = data.discountPercent || 0;

        var wrapper = $(`#invDiscountWrap_${sellerId}`);
        var display = $(`#invDiscountDisplay_${sellerId}`);

        if (isSuccess && discountPercent > 0) {
            if (wrapper.length) {
                wrapper.html('');
                wrapper.hide();
            }

            if (display.length) {
                display.show();
                var discountTitle = data.discountTitle || '';
                var discountText = `<i class="fas fa-percent"></i> تخفیف: ${discountPercent}%`;

                if (discountTitle) {
                    discountText += ` (کد: ${discountTitle})`;
                }

                display.html(`
                    <div style="display:flex;align-items:center;gap:0.5rem;flex-wrap:wrap;">
                        <span class="inv-discount-badge">
                            ${discountText}
                        </span>
                        <button id="invRemoveDiscountBtn_${sellerId}" 
                                class="inv-remove-discount-btn"
                                onclick="invRemoveSellerDiscount('${sellerId}')">
                            <i class="fas fa-times"></i> حذف تخفیف
                        </button>
                    </div>
                `);
            }
        } else if (isSuccess && discountPercent < 1) {
            if (wrapper.length) {
                wrapper.html(`
                    <input type="text" placeholder="کد تخفیف فروشنده"
                           id="invDiscountInput_${sellerId}"
                           class="inv-discount-input" />
                    <button id="invDiscountBtn_${sellerId}"
                            class="inv-discount-btn"
                            onclick="invApplySellerDiscount('${sellerId}')">
                        <i class="fas fa-check"></i> اعمال
                    </button>
                `);
                wrapper.show();
                wrapper.find('input').val('');
                var btn = wrapper.find('button');
                if (btn.length) {
                    btn.prop('disabled', false);
                    btn.html('<i class="fas fa-check"></i> اعمال');
                }
            }

            if (display.length) {
                display.hide();
                display.html('');
            }
        }

        var sellerTotal = $(`#invSellerTotal_${sellerId}`);
        if (sellerTotal.length) {
            var originalPrice = data.sellerPrice || 0;
            var discountedPrice = data.sellerPriceAfterOff || 0;

            if (discountedPrice > 0 && discountedPrice < originalPrice) {
                sellerTotal.html(`
                    <span style="text-decoration:line-through;color:#8a9bb0;font-size:0.85rem;margin-left:0.3rem;">
                        ${invFormatNumber(originalPrice)}
                    </span>
                    <span style="font-weight:700;color:#0b6e41;">
                        ${invFormatNumber(discountedPrice)}
                    </span>
                    <span class="inv-currency">تومان</span>
                `);
            } else {
                sellerTotal.html(`
                    ${invFormatNumber(originalPrice)} 
                    <span class="inv-currency">تومان</span>
                `);
            }
        }

        var paymentPrice = $(`#invPaymentPrice_${sellerId}`);
        if (paymentPrice.length && data.sellerPaymentPrice !== undefined) {
            paymentPrice.html(invFormatNumber(data.sellerPaymentPrice));
        }

        var totalOriginal = data.tolalPrice || 0;
        var discountAmount = data.discountPrice || 0;

        var totalPriceEl = $('#invOrderTotal_Unique_001');
        if (totalPriceEl.length && totalOriginal > 0) {
            totalPriceEl.html(`${invFormatNumber(totalOriginal)} تومان`);
        }

        var totalDiscountEl = $('#invOrderDiscount_Unique_001');
        if (totalDiscountEl.length) {
            if (discountAmount > 0) {
                totalDiscountEl.html(`${invFormatNumber(discountAmount)} تومان`);
            } else {
                totalDiscountEl.html('۰ تومان');
            }
        }

        var finalPayment = data.paymentPrice || 0;

        var finalSummary = $('#invFinalSummary_Unique_001');
        if (finalSummary.length && finalPayment > 0) {
            finalSummary.html(`${invFormatNumber(finalPayment)} تومان`);
        }

        var finalAmount = $('#invFinalAmount_Unique_001');
        if (finalAmount.length && finalPayment > 0) {
            finalAmount.html(invFormatNumber(finalPayment));
        }

        var discountInfo = $('#invDiscountInfo_Unique_001');
        if (discountInfo.length) {
            var discountPercentDisplay = data.discountPercent || 0;
            discountInfo.html(`
                <span><i class="fas fa-ticket-alt"></i> تخفیف: ${discountPercentDisplay}% ${data.discountTitle ? `(کد: ${data.discountTitle})` : ''}</span>
                <span><i class="fas fa-wallet"></i> قابل‌پرداخت: <span id="invFinalAmount_Unique_001">${invFormatNumber(finalPayment)}</span> تومان</span>
            `);
        }

        if (data.items && data.items.length > 0) {
            data.items.forEach(function (item) {
                var discountTag = $(`#invDiscountTag_${sellerId}${item.productId}`);
                if (discountTag.length && item.discountPercent > 0) {
                    discountTag.html(`${item.discountPercent}%`);
                    discountTag.removeClass('inv-no-discount');
                    discountTag.css('background', '#eaf6ef');
                    discountTag.css('color', '#0c6b3e');
                } else if (discountTag.length) {
                    discountTag.html('--');
                    discountTag.addClass('inv-no-discount');
                    discountTag.css('background', '#f5f5f5');
                    discountTag.css('color', '#999');
                }
            });
        } else if (data.discountPercent > 0) {
            $(`#invSellerBlock_${sellerId} .inv-discount-tag`).each(function () {
                var $tag = $(this);
                var currentText = $tag.text().trim();
                if (currentText !== '--' && currentText !== '' && !$tag.hasClass('inv-no-discount')) {
                    $tag.html(`${data.discountPercent}%`);
                }
            });
        }

        if (data.items && data.items.length > 0) {
            data.items.forEach(function (item) {
                var priceAfter = $(`#invPriceAfter_${sellerId}${item.productId}`);
                if (priceAfter.length && item.discountedPrice !== undefined) {
                    priceAfter.html(invFormatNumber(item.discountedPrice));
                }
            });
        }

        var btn = $(`#invDiscountBtn_${sellerId}`);
        if (btn.length) {
            btn.prop('disabled', false);
            btn.html('<i class="fas fa-check"></i> اعمال');
        }
    };

    window.invRemoveSellerDiscount = function (sellerId) {
        var btn = $(`#invRemoveDiscountBtn_${sellerId}`);
        btn.prop('disabled', true);
        btn.html('<i class="fas fa-spinner fa-spin"></i> در حال حذف...');

        $.ajax({
            url: '/order/RemoveSellerDiscount',
            type: 'POST',
            data: {
                sellerId: sellerId
            },
            success: function (response) {
                if (response.success) {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "success", "Center");
                    }
                    invUpdateSellerDiscountUI(sellerId, response);
                } else {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "error", "Center");
                    } else {
                        alert(response.message);
                    }
                    btn.prop('disabled', false);
                    btn.html('<i class="fas fa-times"></i> حذف تخفیف');
                }
            },
            error: function (xhr) {
                var errorMessage = 'خطا در ارتباط با سرور.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(errorMessage, "error", "Center");
                } else {
                    alert(errorMessage);
                }
                btn.prop('disabled', false);
                btn.html('<i class="fas fa-times"></i> حذف تخفیف');
            }
        });
    };

    window.invApplyOrderDiscount = function () {
        var code = $('#invDiscountInputGlobal_Unique_001').val().trim();

        if (code === '') {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('لطفاً کد تخفیف را وارد کنید.', "error", "Center");
            } else {
                alert('لطفاً کد تخفیف را وارد کنید.');
            }
            return;
        }

        var btn = $('#invDiscountBtnGlobal_Unique_001');
        btn.prop('disabled', true);
        btn.html('<i class="fas fa-spinner fa-spin"></i> در حال اعمال...');

        $.ajax({
            url: '/order/ApplyOrderDiscount',
            type: 'POST',
            data: { Code: code },
            success: function (response) {
                if (response.success) {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "success", "Center");
                    }
                    invUpdateOrderDiscountUI(response);
                } else {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer(response.message, "error", "Center");
                    } else {
                        alert(response.message);
                    }
                }
            },
            error: function (xhr) {
                var msg = 'خطا در ارتباط با سرور.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = xhr.responseJSON.message;
                }
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(msg, "error", "Center");
                } else {
                    alert(msg);
                }
            },
            complete: function () {
                btn.prop('disabled', false);
                btn.html('<i class="fas fa-check-circle"></i> اعمال');
            }
        });
    };
    window.invUpdateOrderDiscountUI = function (data) {
        var totalPriceEl = $('#invOrderTotal_Unique_001');
        if (totalPriceEl.length) {
            var totalPrice = data.tolalPrice || 0;
            totalPriceEl.html(`${invFormatNumber(totalPrice)} تومان`);
        }
        debugger;
        var totalDiscountEl = $('#invOrderDiscount_Unique_001');
        if (totalDiscountEl.length) {
            var discountPrice = data.discountPrice || 0;
            if (discountPrice > 0) {
                totalDiscountEl.html(`${invFormatNumber(discountPrice)} تومان`);
            } else {
                totalDiscountEl.html('۰ تومان');
            }
        }

        var finalSummary = $('#invFinalSummary_Unique_001');
        if (finalSummary.length) {
            var finalPayment = data.paymentPrice || 0;
            finalSummary.html(`${invFormatNumber(finalPayment)} تومان`);
        }

        var finalAmount = $('#invFinalAmount_Unique_001');
        if (finalAmount.length) {
            var finalPayment = data.paymentPrice || 0;
            finalAmount.html(invFormatNumber(finalPayment));
        }

 

        var discountSection = $('#invDiscountSection_Unique_001');
        var discountGroup = discountSection.find('#invDiscountGroup_Unique_001');
        var discountApplied = discountSection.find('#invDiscountApplied_Unique_001');
        var discountLabel = discountSection.find('.inv-discount-label');

        if (data.discountPercent > 0) {
            if (discountGroup.length) {
                discountGroup.remove();
            }

            if (discountApplied.length === 0) {
                var appliedHtml = `
                <div class="inv-discount-applied" id="invDiscountApplied_Unique_001">
                    <span class="inv-discount-applied-icon">
                        <i class="fas fa-check-circle"></i>
                    </span>
                    <span class="inv-discount-applied-text">
                        تخفیف ${data.discountPercent}% اعمال شد
                    </span>
                    ${data.discountTitle ? `<span class="inv-discount-applied-code">(کد: ${data.discountTitle})</span>` : ''}
                    <button id="invRemoveOrderDiscountBtn" class="inv-remove-discount-btn" onclick="invRemoveOrderDiscount()">
                        <i class="fas fa-times"></i> حذف تخفیف
                    </button>
                </div>
            `;

                if (discountLabel.length) {
                    discountLabel.after(appliedHtml);
                } else {
                    discountSection.prepend(appliedHtml);
                }
            } else {
                discountApplied.html(`
                <span class="inv-discount-applied-icon">
                    <i class="fas fa-check-circle"></i>
                </span>
                <span class="inv-discount-applied-text">
                    تخفیف ${data.discountPercent}% اعمال شد
                </span>
                ${data.discountTitle ? `<span class="inv-discount-applied-code">(کد: ${data.discountTitle})</span>` : ''}
                <button id="invRemoveOrderDiscountBtn" class="inv-remove-discount-btn" onclick="invRemoveOrderDiscount()">
                    <i class="fas fa-times"></i> حذف تخفیف
                </button>
            `);
            }
        } else {
            if (discountApplied.length) {
                discountApplied.remove();
            }

            if (discountGroup.length === 0) {
                var groupHtml = `
                <div class="inv-discount-group" id="invDiscountGroup_Unique_001">
                    <input type="text" placeholder="مثلاً SUMMER1405" id="invDiscountInputGlobal_Unique_001" class="inv-discount-input" />
                    <button id="invDiscountBtnGlobal_Unique_001" class="inv-discount-btn">
                        <i class="fas fa-check-circle"></i> اعمال
                    </button>
                </div>
            `;

                if (discountLabel.length) {
                    discountLabel.after(groupHtml);
                } else {
                    discountSection.prepend(groupHtml);
                }

                $('#invDiscountBtnGlobal_Unique_001').on('click', function (e) {
                    e.preventDefault();
                    invApplyOrderDiscount();
                });

                $('#invDiscountInputGlobal_Unique_001').on('keypress', function (e) {
                    if (e.which === 13) {
                        e.preventDefault();
                        invApplyOrderDiscount();
                    }
                });
            }
        }
    };

    window.invRemoveOrderDiscount = function () {
        var btn = $('#invRemoveOrderDiscountBtn');
        btn.prop('disabled', true);
        btn.html('<i class="fas fa-spinner fa-spin"></i> در حال حذف...');

        $.ajax({
            url: '/order/RemoveOrderDiscount',
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    if (typeof AlertSweetWithTimer === 'function') {
                        AlertSweetWithTimer(response.message, "success", "Center");
                    }
                    response.discountPercent = 0
                    invUpdateOrderDiscountUI(response);
                } else {
                    if (typeof AlertSweetWithTimer === 'function') {
                        AlertSweetWithTimer(response.message, "error", "Center");
                    } else {
                        alert(response.message);
                    }
                    btn.prop('disabled', false);
                    btn.html('<i class="fas fa-times"></i> حذف تخفیف');
                }
            },
            error: function (xhr) {
                var msg = 'خطا در ارتباط با سرور.';
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    msg = xhr.responseJSON.message;
                }
                if (typeof AlertSweetWithTimer === 'function') {
                    AlertSweetWithTimer(msg, "error", "Center");
                } else {
                    alert(msg);
                }
                btn.prop('disabled', false);
                btn.html('<i class="fas fa-times"></i> حذف تخفیف');
            }
        });
    };
    $(document).ready(function () {
        $('input[id^="invDiscountInput_"]').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                var inputId = $(this).attr('id');
                var sellerId = inputId.replace('invDiscountInput_', '');
                invApplySellerDiscount(sellerId);
            }
        });

        $('.inv-seller-block').each(function () {
            var $block = $(this);
            var display = $block.find('.inv-discount-display');
            var wrapper = $block.find('.inv-discount-wrapper');

            var hasDiscount = false;
            var discountBadge = display.find('.inv-discount-badge');
            if (discountBadge.length && discountBadge.text().trim() !== '') {
                var discountText = discountBadge.text().trim();
                var discountMatch = discountText.match(/(\d+)%/);
                if (discountMatch && parseInt(discountMatch[1]) > 0) {
                    hasDiscount = true;
                }
            }

            if (hasDiscount) {
                if (wrapper.length) {
                    wrapper.html('');
                }
                if (display.length) {
                    display.show();
                }
            } else {
                if (wrapper.length) {
                    wrapper.show();
                }
                if (display.length) {
                    display.hide();
                }
            }
        });

        $('#invDiscountBtnGlobal_Unique_001').on('click', function (e) {
            e.preventDefault();
            invApplyOrderDiscount();
        });

        $('#invDiscountInputGlobal_Unique_001').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                invApplyOrderDiscount();
            }
        });

        $('#invPayBtn_Unique_001').on('click', function (e) {
            e.preventDefault();
            if (typeof AlertSweetWithTimer === 'function') {
                AlertSweetWithTimer('شما به درگاه پرداخت هدایت می‌شوید.', "info", "Center");
            } else {
                alert('شما به درگاه پرداخت هدایت می‌شوید.');
            }
        });
    });
})();