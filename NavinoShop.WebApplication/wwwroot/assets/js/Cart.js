function saveCartToCookie(cart) {
    try {
        const jsonData = JSON.stringify(cart);

        Cookies.set('NavinoshoppingCart', jsonData, {
            expires: 7,
            path: '/',
            sameSite: 'Lax',
            secure: window.location.protocol === 'https:'
        });

        localStorage.setItem('NavinoshoppingCart', jsonData);

        window.dispatchEvent(new StorageEvent('storage', {
            key: 'NavinoshoppingCart',
            newValue: jsonData,
            oldValue: localStorage.getItem('NavinoshoppingCart'),
            storageArea: localStorage
        }));

        $(document).trigger('cartUpdated', [cart]);
        verifyCartSaved();
    } catch (e) {
        console.error('Error saving cart:', e);
    }
}

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

function loadCartFromCookie(forceRefresh = false) {
    try {
        if (forceRefresh) {
            localStorage.removeItem('NavinoshoppingCart_cache');
        }

        const cartCookie = Cookies.get('NavinoshoppingCart');
        if (cartCookie && cartCookie !== 'undefined' && cartCookie !== 'null' && cartCookie !== '{}') {
            const parsed = JSON.parse(cartCookie);
            if (parsed && typeof parsed === 'object') {
                let validCart = {};
                for (let key in parsed) {
                    if (parsed[key] && typeof parsed[key] === 'object') {
                        validCart[key] = parsed[key];
                    }
                }
                localStorage.setItem('NavinoshoppingCart', JSON.stringify(validCart));
                localStorage.setItem('NavinoshoppingCart_cache', JSON.stringify({
                    data: validCart,
                    timestamp: Date.now()
                }));
                return validCart;
            }
        }
    } catch (e) {
        console.error('Error loading from cookie:', e);
    }

    try {
        const cacheData = localStorage.getItem('NavinoshoppingCart_cache');
        if (cacheData) {
            const cache = JSON.parse(cacheData);
            if (cache && cache.data && typeof cache.data === 'object') {
                if (Date.now() - cache.timestamp < 5000) {
                    return cache.data;
                }
            }
        }
    } catch (e) {
        console.error('Error loading from cache:', e);
    }

    try {
        const cartData = localStorage.getItem('NavinoshoppingCart');
        if (cartData && cartData !== 'undefined' && cartData !== 'null') {
            const parsed = JSON.parse(cartData);
            if (parsed && typeof parsed === 'object') {
                return parsed;
            }
        }
    } catch (e) {
        console.error('Error loading from localStorage:', e);
    }

    return {};
}

function forceRefreshCart() {
    try {
        localStorage.removeItem('NavinoshoppingCart_cache');

        const cartCookie = Cookies.get('NavinoshoppingCart');
        if (cartCookie && cartCookie !== 'undefined' && cartCookie !== 'null' && cartCookie !== '{}') {
            const parsed = JSON.parse(cartCookie);
            if (parsed && typeof parsed === 'object') {
                localStorage.setItem('NavinoshoppingCart', cartCookie);
                localStorage.setItem('NavinoshoppingCart_cache', JSON.stringify({
                    data: parsed,
                    timestamp: Date.now()
                }));
                updateCartDisplay();
                return parsed;
            }
        }

        const cartData = localStorage.getItem('NavinoshoppingCart');
        if (cartData && cartData !== 'undefined' && cartData !== 'null') {
            const parsed = JSON.parse(cartData);
            if (parsed && typeof parsed === 'object') {
                Cookies.set('NavinoshoppingCart', cartData, {
                    expires: 7,
                    path: '/',
                    sameSite: 'Lax'
                });
                localStorage.setItem('NavinoshoppingCart_cache', JSON.stringify({
                    data: parsed,
                    timestamp: Date.now()
                }));
                updateCartDisplay();
                return parsed;
            }
        }

        localStorage.removeItem('NavinoshoppingCart');
        localStorage.removeItem('NavinoshoppingCart_cache');
        Cookies.remove('NavinoshoppingCart', { path: '/' });
        updateCartDisplay();
    } catch (e) {
        console.error('Error forcing refresh:', e);
    }
    return {};
}

function addToCart(productSellId, title, imageName, price, priceAfterOff, seller, amount) {
    const productId = String(productSellId).trim();
    const validAmount = amount && !isNaN(parseFloat(amount)) && parseFloat(amount) > 0 ? parseFloat(amount) : 0;
    const validPrice = parseFloat(price) || 0;
    const validPriceAfterOff = parseFloat(priceAfterOff) || 0;

    if (validAmount === 0) {
        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('این محصول موجودی ندارد!', "error", "Center");
        } else {
            alert('این محصول موجودی ندارد!');
        }
        return;
    }

    let cart = loadCartFromCookie(true);

    if (cart[productId]) {
        const newQuantity = cart[productId].quantity + 1;
        if (newQuantity > validAmount) {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('موجودی محصول کافی نیست! حداکثر ' + validAmount + ' عدد موجود است.', "error", "Center");
            } else {
                alert('موجودی محصول کافی نیست! حداکثر ' + validAmount + ' عدد موجود است.');
            }
            return;
        }
        cart[productId].quantity = newQuantity;
    } else {
        cart[productId] = {
            Id: productId,
            Title: title || 'بدون عنوان',
            ImageName: imageName,
            Price: validPrice,
            quantity: 1,
            PriceAfterOff: validPriceAfterOff,
            Seller: seller || 'نام فروشنده',
            Amount: validAmount
        };
    }

    saveCartToCookie(cart);
    localStorage.removeItem('NavinoshoppingCart_cache');
    updateCartDisplay();

    if (typeof AlerSweetWithTimer === 'function') {
        AlerSweetWithTimer('محصول به سبد خرید اضافه شد!', "success", "Center");
    } else {
        alert('محصول به سبد خرید اضافه شد!');
    }

    return cart;
}

function removeFromCart(productSellId) {
    const productId = String(productSellId).trim();
    let cart = loadCartFromCookie(true);

    if (cart[productId]) {
        delete cart[productId];
        saveCartToCookie(cart);
        localStorage.removeItem('NavinoshoppingCart_cache');
        updateCartDisplay();

        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('محصول از سبد خرید حذف شد!', "success", "Center");
        } else {
            alert('محصول از سبد خرید حذف شد!');
        }
    } else {
        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('محصول در سبد خرید یافت نشد!', "error", "Center");
        }
    }
}

function updateQuantity(productSellId, change) {
    let cart = loadCartFromCookie(true);
    const productId = String(productSellId);

    if (cart[productId]) {
        const newQuantity = cart[productId].quantity + change;
        const maxAmount = cart[productId].Amount || 0;

        if (newQuantity > maxAmount) {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('موجودی محصول کافی نیست!', "error", "Center");
            }
            return;
        }

        cart[productId].quantity = newQuantity;

        if (cart[productId].quantity <= 0) {
            delete cart[productId];
        }

        saveCartToCookie(cart);
        localStorage.removeItem('NavinoshoppingCart_cache');
        updateCartDisplay();
    }
}

function clearCart() {
    if (confirm('آیا از خالی کردن سبد خرید مطمئن هستید؟')) {
        Cookies.remove('NavinoshoppingCart', { path: '/' });
        localStorage.removeItem('NavinoshoppingCart');
        localStorage.removeItem('NavinoshoppingCart_cache');

        window.dispatchEvent(new StorageEvent('storage', {
            key: 'NavinoshoppingCart',
            newValue: null,
            oldValue: localStorage.getItem('NavinoshoppingCart'),
            storageArea: localStorage
        }));
        $(document).trigger('cartUpdated', [{}]);

        updateCartDisplay();
        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('سبد خرید خالی شد!', "success", "Center");
        }
    }
}

function calculateTotal(cart) {
    let total = 0;
    for (let id in cart) {
        const item = cart[id];
        const price = (item.PriceAfterOff && item.PriceAfterOff > 0) ? item.PriceAfterOff : item.Price;
        total += price * item.quantity;
    }
    return total;
}

function updateCartDisplay() {
    const cart = loadCartFromCookie(true);
    const cartCountSpan = $('#cart-count');

    let itemCount = 0;
    for (let id in cart) {
        itemCount += cart[id].quantity;
    }

    if (cartCountSpan.length) {
        cartCountSpan.text(itemCount);
        if (itemCount > 0) {
            cartCountSpan.show();
        } else {
            cartCountSpan.hide();
        }
    }

    $('.cart-badge, .cart-count-header, .cart-item-count').each(function () {
        if (itemCount > 0) {
            $(this).text(itemCount).show();
        } else {
            $(this).text('0').hide();
        }
    });

    if ($('#cart-items-dropdown').length) {
        showCartDropdown();
    }
}

function getItemPrice(item) {
    return (item.PriceAfterOff && item.PriceAfterOff > 0) ? item.PriceAfterOff : item.Price;
}

function showCartDropdown() {
    const cart = loadCartFromCookie(true);
    const dropdownItems = $('#cart-items-dropdown');
    const dropdownFooter = $('#cart-footer');

    if (!dropdownItems.length) return;

    if (Object.keys(cart).length === 0) {
        dropdownItems.html(`
            <div class="empty-cart-dropdown">
                <i class="fa fa-shopping-cart" style="font-size: 48px; color: #ddd;"></i>
                <p style="margin-top: 10px;">سبد خرید شما خالی است 🛒</p>
            </div>
        `);
        if (dropdownFooter.length) dropdownFooter.html('');
        return;
    }

    let itemsHtml = '';
    for (let id in cart) {
        const item = cart[id];
        const price = parseFloat(item.Price) || 0;
        const priceAfterOff = parseFloat(item.PriceAfterOff) || 0;
        const hasDiscount = priceAfterOff > 0 && priceAfterOff < price;
        const finalPrice = hasDiscount ? priceAfterOff : price;
        const itemTotal = finalPrice * item.quantity;
        const imagePath = item.ImageName ;
        const maxAmount = item.Amount || 0;
        const isOutOfStock = maxAmount === 0;

        let priceHtml = '';
        if (isOutOfStock) {
            priceHtml = `<span style="color: #dc3545; font-weight: bold;">ناموجود</span>`;
        } else if (hasDiscount) {
            priceHtml = `
                <span style="text-decoration: line-through; color: #999; font-size: 11px;">${formatPrice(price)}</span>
                <span style="color: #dc3545; font-weight: bold;">${formatPrice(priceAfterOff)}</span>
            `;
        } else {
            priceHtml = `<span style="color: #28a745; font-weight: bold;">${formatPrice(price)}</span>`;
        }

        itemsHtml += `
            <div class="dropdown-cart-item" data-id="${id}" data-max="${maxAmount}">
                <div class="dropdown-cart-item-image">
                    <img src="${imagePath}" alt="${item.Title}">
                </div>
                <div class="dropdown-cart-item-info">
                    <div class="dropdown-cart-item-title">${item.Title || 'بدون عنوان'}</div>
                    <div class="dropdown-cart-item-seller">${item.Seller || 'نام فروشنده'}</div>
                    <div class="dropdown-cart-item-price">
                        ${priceHtml}
                        ${!isOutOfStock ? `<span style="color: #28a745; margin-right: 5px;">× ${item.quantity} = ${formatPrice(itemTotal)}</span>` : ''}
                    </div>
                    ${!isOutOfStock && maxAmount <= 5 ? `<span style="color: #dc3545; font-size: 11px; margin-right: 5px; font-weight: bold;">فقط ${maxAmount} عدد باقی مانده</span>` : ''}
                </div>
                <div class="dropdown-cart-item-actions">
                    ${!isOutOfStock ? `
                        <button class="dropdown-quantity-btn" data-id="${id}" data-change="-1">-</button>
                        <span class="dropdown-item-quantity">${item.quantity}</span>
                        <button class="dropdown-quantity-btn" data-id="${id}" data-change="1">+</button>
                    ` : `
                        <span style="color: #dc3545; font-size: 12px;">ناموجود</span>
                    `}
                    <button class="dropdown-remove-btn" data-id="${id}">
                        <i class="fa fa-trash"></i>
                    </button>
                </div>
            </div>
        `;
    }

    dropdownItems.html(itemsHtml);

    const total = calculateTotal(cart);
    if (dropdownFooter.length) {
        dropdownFooter.html(`
            <div class="dropdown-total">
                <span>مجموع سبد خرید:</span>
                <span style="color: #28a745; font-weight: bold;">${formatPrice(total)}</span>
            </div>
            <div class="dropdown-actions">
                <a href="/Cart" class="btn btn-view-cart">
                    <i class="fa fa-eye"></i> مشاهده سبد خرید
                </a>
            </div>
        `);
    }

    attachDropdownEvents();
}

function attachDropdownEvents() {
    $('.dropdown-quantity-btn').off('click').on('click', function (e) {
        e.stopPropagation();
        const productId = $(this).data('id');
        const change = parseInt($(this).data('change'));
        if (productId && !isNaN(change)) {
            updateQuantityFromDropdown(productId, change);
        }
    });

    $('.dropdown-remove-btn').off('click').on('click', function (e) {
        e.stopPropagation();
        const productId = $(this).data('id');
        if (productId) {
            removeFromDropdown(productId);
        }
    });

    $('#clear-cart-btn').off('click').on('click', function (e) {
        e.stopPropagation();
        clearCartFromDropdown();
    });
}

function updateQuantityFromDropdown(productId, change) {
    let cart = loadCartFromCookie(true);
    const productIdStr = String(productId);

    if (cart[productIdStr]) {
        const newQuantity = cart[productIdStr].quantity + change;
        const maxAmount = cart[productIdStr].Amount || 0;

        if (newQuantity > maxAmount) {
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer('موجودی محصول کافی نیست!', "error", "Center");
            }
            return;
        }

        cart[productIdStr].quantity = newQuantity;

        if (cart[productIdStr].quantity <= 0) {
            delete cart[productIdStr];
        }

        saveCartToCookie(cart);
        localStorage.removeItem('NavinoshoppingCart_cache');
        updateCartDisplay();
        showCartDropdown();

        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('تعداد محصول به‌روزرسانی شد!', "success", "Center");
        }
    }
}

function removeFromDropdown(productId) {
    let cart = loadCartFromCookie(true);
    const productIdStr = String(productId);
    delete cart[productIdStr];
    saveCartToCookie(cart);
    localStorage.removeItem('NavinoshoppingCart_cache');
    updateCartDisplay();
    showCartDropdown();

    if (typeof AlerSweetWithTimer === 'function') {
        AlerSweetWithTimer('محصول از سبد خرید حذف شد!', "success", "Center");
    }
}

function clearCartFromDropdown() {
    if (confirm('آیا از خالی کردن سبد خرید مطمئن هستید؟')) {
        Cookies.remove('NavinoshoppingCart', { path: '/' });
        localStorage.removeItem('NavinoshoppingCart');
        localStorage.removeItem('NavinoshoppingCart_cache');

        window.dispatchEvent(new StorageEvent('storage', {
            key: 'NavinoshoppingCart',
            newValue: null,
            oldValue: localStorage.getItem('NavinoshoppingCart'),
            storageArea: localStorage
        }));
        $(document).trigger('cartUpdated', [{}]);

        updateCartDisplay();
        showCartDropdown();

        if (typeof AlerSweetWithTimer === 'function') {
            AlerSweetWithTimer('سبد خرید خالی شد!', "success", "Center");
        }
    }
}

function formatPrice(price) {
    if (price === undefined || price === null || isNaN(price)) return '0 تومان';
    return new Intl.NumberFormat('fa-IR').format(Math.round(price)) + ' تومان';
}

function syncCartAcrossTabs() {
    window.addEventListener('storage', function (e) {
        if (e.key === 'NavinoshoppingCart') {
            forceRefreshCart();
            updateCartDisplay();
            if ($('.cart-dropdown-menu.show').length) {
                showCartDropdown();
            }
        }
    });

    $(document).on('cartUpdated', function (event, cart) {
        updateCartDisplay();
        if ($('.cart-dropdown-menu.show').length) {
            showCartDropdown();
        }
    });
}

function debugCart() {
    console.log('=== CART DEBUG ===');
    console.log('Cookie:', Cookies.get('NavinoshoppingCart'));
    console.log('LocalStorage:', localStorage.getItem('NavinoshoppingCart'));
    console.log('Cache:', localStorage.getItem('NavinoshoppingCart_cache'));
    console.log('Loaded cart:', loadCartFromCookie(true));
    console.log('================');
}

window.debugCart = debugCart;

$(document).ready(function () {
    forceRefreshCart();
    syncCartAcrossTabs();
    updateCartDisplay();

    if ($('.cart-dropdown-menu').length && $('.cart-toggle-btn').length) {
        $('.cart-toggle-btn').off('click').on('click', function (e) {
            e.stopPropagation();
            e.preventDefault();
            const $dropdown = $('.cart-dropdown-menu');
            $dropdown.toggleClass('show');
            if ($dropdown.hasClass('show')) {
                forceRefreshCart();
                showCartDropdown();
            }
        });
    }

    if ($('#close-dropdown').length) {
        $('#close-dropdown').off('click').on('click', function () {
            $('.cart-dropdown-menu').removeClass('show');
        });
    }

    $(document).off('click').on('click', function (e) {
        if (!$(e.target).closest('.cart-dropdown-container').length) {
            $('.cart-dropdown-menu').removeClass('show');
        }
    });

    if ($('.cart-dropdown-menu').length) {
        $('.cart-dropdown-menu').off('click').on('click', function (e) {
            e.stopPropagation();
        });
    }

    document.addEventListener('visibilitychange', function () {
        if (!document.hidden) {
            forceRefreshCart();
            updateCartDisplay();
        }
    });

    window.addEventListener('pageshow', function (event) {
        if (event.persisted) {
            forceRefreshCart();
            updateCartDisplay();
        }
    });
});

(function () {
    setTimeout(function () {
        forceRefreshCart();
        updateCartDisplay();
    }, 500);
})();