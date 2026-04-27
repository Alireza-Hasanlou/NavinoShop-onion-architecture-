$('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({ checkboxClass: 'icheckbox_flat-green', radioClass: 'iradio_flat-green' })

$('.DataTable').DataTable({
    paging: true,
    autoWidth: true,
    lengthChange: false,
    searching: true,
    ordering: true,
    scroll: true,
    info: false,
    language: {
        search: "جستجو:",
        paginate: {
            next: "بعدی",
            previous: "قبلی"
        }
    }
});

$(document).ready(function () {
    $(".picture-uploader").imageUploader();
});

function renderRoleUsersModal(res) {
    if (!res || !res.users || res.users.length === 0) {
        Swal.fire({
            title: "خطا",
            text: "کاربری برای این نقش یافت نشد",
            icon: "error",
            confirmButtonText: "متوجه شدم",
        });
        return;
    }

    let tableHtml = `
        <div style="font-size:18px; text-align:right; direction:rtl">
            <h3 style="margin-bottom:20px">کاربران نقش: <b>${res.roleTitle}</b></h3>
            <table id="swalTable" class="table table-bordered table-striped" style="width:100%; font-size:16px">
                <thead>
                    <tr>
                        <th>آواتار</th>
                        <th>نام کامل</th>
                        <th>موبایل</th>
                    </tr>
                </thead>
                <tbody>
                    ${res.users.map(u => `
                        <tr>
                            <td><img src="/Images/User/100/${u.userAvatar}" style="width:40px; height:40px; border-radius:50%"></td>
                            <td>${u.userFullName}</td>
                            <td>${u.mobile}</td>
                        </tr>
                    `).join("")}
                </tbody>
            </table>
        </div>
    `;

    Swal.fire({
        title: "لیست کاربران",
        html: tableHtml,
        width: "900px",
        showCloseButton: true,
        showConfirmButton: false,
        customClass: {
            title: 'swal-title-large'
        }
    });

    $('#swalTable').DataTable({
        paging: true,
        searching: true,
        lengthChange: false,
        info: false,
        language: {
            search: "جستجو:",
            paginate: {
                next: "بعدی",
                previous: "قبلی"
            }
        }
    });
}

function ShowRoleUsers(roleId) {
    $.ajax({
        url: "/Admin/Roles/UsersInRole",
        type: "GET",
        data: { id: roleId },
        success: function (res) {
            renderRoleUsersModal(res);
        },
        error: function () {
            Swal.fire("خطا", "ارتباط با سرور برقرار نشد", "error");
        }
    });
}

function makeSlug(source, destination) {
    let text = $('#' + source).val();

    if (!text) {
        $('#' + destination).val('');
        return;
    }

    text = text
        .normalize('NFC')
        .trim()
        .replace(/\u0640/g, '')
        .replace(/[\u064B-\u065F]/g, '')
        .replace(/[\u200C\u200D\uFEFF]/g, ' ')
        .replace(/[^a-zA-Z0-9\u0600-\u06FF\s-]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-')
        .replace(/^-+|-+$/g, '');

    $('#' + destination).val(text);
}

$(function () {
    $(document).on("change", "#categoryId", function () {
        var categoryId = $(this).val();
        var $subCategory = $("#subCategoryId");

        if (categoryId) {
            $.ajax({
                url: '/Admin/Blogs/Create?handler=SubCategories',
                type: 'GET',
                data: { categoryId: categoryId },
                success: function (data) {
                    $subCategory.empty();
                    $subCategory.append('<option value="0">انتخاب کنید</option>');
                    $.each(data, function (i, sub) {
                        $subCategory.append('<option value="' + sub.id + '">' + sub.title + '</option>');
                    });
                },
                error: function () {
                    alert("خطا در بارگذاری زیرگروه‌ها");
                }
            });
        } else {
            $subCategory.empty();
            $subCategory.append('<option value="0">ابتدا سر گروه را انتخاب کنید</option>');
        }
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

function RechargeWallet() {
    var amount = $("#WalletAmount").val();
    var description = $("#description").val();
    var userId = $("#UserId").val();
    var amountvalidation = $("#amountValidation");
    amountvalidation.text("");

    $.ajax({
        url: "/Admin/Users/RechargeWallet",
        type: "POST",
        data: {
            UserId: userId,
            Description: description,
            Amount: amount,
        },
        dataType: "json"
    })
        .done(function (res) {
            if (res.success) {
                $("#modal-default").modal("hide");
                AlerSweetWithTimer(res.message, "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                amountvalidation.text(res.message);
            }
        }).fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

function CreateUser() {
    var fullName = $("#FullName").val();
    var mobile = $("#Mobile").val();
    var email = $("#Email").val();
    var gender = $("#Gender").val();
    var password = $("#Password").val();
    var createUserValidation = $("#CreateUserValidation");
    createUserValidation.text("");

    $.ajax({
        url: "/Admin/Users/Create",
        type: "POST",
        data: {
            FullName: fullName,
            Mobile: mobile,
            Email: email,
            Gender: gender,
            Password: password
        },
        dataType: "json"
    })
        .done(function (res) {
            if (res.success) {
                $("#modal-default").modal("hide");
                AlerSweetWithTimer(res.message, "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                createUserValidation.text(res.message);
            }
        }).fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

function CreateProductCategory() {
    const productcategorytitle = $("#productcategorytitle").val();
    const parentId = $("#parentId").val();
    const productcategorySlug = $("#productcategorySlug").val();
    const productcategoryvalidation = $("#productcategoryvalidation");

    productcategoryvalidation.text("");

    $.ajax({
        url: "/Admin/ProductCategories/Create",
        type: "POST",
        data: {
            ParentId: parentId,
            ProductCategoryTitle: productcategorytitle,
            Slug: productcategorySlug
        },
        dataType: "json"
    })
        .done(function (res) {
            if (res.success) {
                $("#modal-default").modal("hide");
                AlerSweetWithTimer(res.message, "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                productcategoryvalidation.text(res.message);
            }
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

function EditProductCategory() {
    const productcategorytitle = $("#productcategorytitle").val();
    const parentId = $("#parentId").val();
    const id = $("#id").val();
    const productcategorySlug = $("#productcategorySlug").val();
    const productcategoryvalidation = $("#productcategoryvalidation");

    productcategoryvalidation.text("");

    $.ajax({
        url: "/Admin/ProductCategories/Edit",
        type: "POST",
        data: {
            Title: productcategorytitle,
            Slug: productcategorySlug,
            ParentId: parentId,
            Id: id
        },
        dataType: "json"
    })
        .done(function (res) {
            if (res.success) {
                $("#modal-default").modal("hide");
                AlerSweetWithTimer(res.message, "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                productcategoryvalidation.text(res.message);
            }
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}

function CreateProduct() {
    let isValid = true;
    $('.product-form-validation').hide();
    $('.product-form-control, .product-form-textarea, .picture__input').removeClass('is-invalid');

    if (!$('#Title').val() || !$('#Title').val().trim()) {
        $('.product-form-validation[data-for="Title"]').show();
        $('#Title').addClass('is-invalid');
        isValid = false;
    }

    if (!$('#Slug').val() || !$('#Slug').val().trim()) {
        $('.product-form-validation[data-for="Slug"]').show();
        $('#Slug').addClass('is-invalid');
        isValid = false;
    }

    if (!$('#ShortDescription').val() || !$('#ShortDescription').val().trim()) {
        $('.product-form-validation[data-for="ShortDescription"]').show();
        $('#ShortDescription').addClass('is-invalid');
        isValid = false;
    }

    if (!$('#Text').val() || !$('#Text').val().trim()) {
        $('.product-form-validation[data-for="Text"]').show();
        $('#Text').addClass('is-invalid');
        isValid = false;
    }

    if (!$('#Weight').val()) {
        $('.product-form-validation[data-for="Weight"]').show();
        $('#Weight').addClass('is-invalid');
        isValid = false;
    }

    if (!$('#ImageAlt').val() || !$('#ImageAlt').val().trim()) {
        $('.product-form-validation[data-for="ImageAlt"]').show();
        $('#ImageAlt').addClass('is-invalid');
        isValid = false;
    }

    var imageFile = $('#upload-file')[0].files[0];
    if (!imageFile) {
        $('.product-form-validation[data-for="ImageFile"]').show();
        $('#ImageFile').addClass('is-invalid');
        isValid = false;
    }

    if (!isValid) {
        var firstInvalid = $('.is-invalid:first');
        if (firstInvalid.length > 0) {
            $('html, body').animate({
                scrollTop: firstInvalid.offset().top - 100
            }, 500);
        }
        return;
    }

    var formData = new FormData();
    formData.append("Title", $('#Title').val());
    formData.append("Slug", $('#Slug').val());
    formData.append("ShortDescription", $('#ShortDescription').val());
    formData.append("Text", $('#Text').val());
    formData.append("Weight", $('#Weight').val());
    formData.append("ImageAlt", $('#ImageAlt').val());
    formData.append("ImageFile", imageFile);

    var selectedCategories = [];
    $('.pct-checkbox:checked').each(function () {
        var catId = parseInt($(this).val());
        selectedCategories.push(catId);
        formData.append("CategoryIds", catId);
    });

    $.ajax({
        url: "/Admin/Products/Create",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        beforeSend: function () {
            $('.btn-primary').prop('disabled', true).text('در حال ارسال...');
        },
        success: function (res) {
            if (res.success) {
                $("#modal-default").modal("hide");
                AlerSweetWithTimer("محصول با موفقیت ایجاد شد", "success", "center");
                setTimeout(function () {
                    location.reload();
                }, 3000);
            } else {
                if (res.message !== null) {
                    AlerSweetWithTimer(res.message, "error", "center");
                } else {
                    AlerSweetWithTimer("خطای ناشناخته ای رخ داده", "error", "center");
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
            var errorMessage = "خطا در ارتباط با سرور: " + xhr.status;
            if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage += "\n" + xhr.responseJSON.message;
            }
            AlerSweetWithTimer(errorMessage, "error", "center");
        },
        complete: function () {
            $('.btn-primary').prop('disabled', false).text('ذخیره');
        }
    });
}

function EditProduct() {
    $('.product-form-validation').hide();
    $('.product-form-control, .product-form-textarea, .picture__input').removeClass('is-invalid');

    let isValid = true;
    const validationFields = [
        { id: 'Title', name: 'عنوان' },
        { id: 'Slug', name: 'Slug' },
        { id: 'ShortDescription', name: 'توضیح مختصر' },
        { id: 'Text', name: 'توضیحات کامل' },
        { id: 'Weight', name: 'وزن' },
        { id: 'ImageAlt', name: 'Alt تصویر' }
    ];

    validationFields.forEach(field => {
        const $element = $(`#${field.id}`);
        const value = $element.val();

        if (!value || !value.toString().trim()) {
            $(`.product-form-validation[data-for="${field.id}"]`).show();
            $element.addClass('is-invalid');
            isValid = false;
        }
    });

    const weightValue = $('#Weight').val();
    if (weightValue && parseFloat(weightValue) <= 0) {
        $('.product-form-validation[data-for="Weight"]').show();
        $('#Weight').addClass('is-invalid');
        isValid = false;
    }

    const imageFile = $('#upload-file')[0]?.files[0];

    if (imageFile) {
        const maxSize = 2 * 1024 * 1024;
        if (imageFile.size > maxSize) {
            AlerSweetWithTimer("حجم فایل تصویر نباید بیشتر از 2 مگابایت باشد", "error", "center");
            isValid = false;
        }

        const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
        if (!allowedTypes.includes(imageFile.type)) {
            AlerSweetWithTimer("فرمت فایل باید jpg، png یا gif باشد", "error", "center");
            isValid = false;
        }
    }

    if (!isValid) {
        const $firstInvalid = $('.is-invalid:first');
        if ($firstInvalid.length > 0) {
            $('html, body').animate({
                scrollTop: $firstInvalid.offset().top - 100
            }, 500);
        }
        return;
    }

    const formData = new FormData();
    formData.append("Id", $('#ProductId').val());
    formData.append("ImageName", $('#ImageName').val());
    formData.append("Title", $('#Title').val().trim());
    formData.append("Slug", $('#Slug').val().trim());
    formData.append("ShortDescription", $('#ShortDescription').val().trim());
    formData.append("Text", $('#Text').val().trim());
    formData.append("Weight", parseInt($('#Weight').val()));
    formData.append("ImageAlt", $('#ImageAlt').val().trim());
    if (imageFile) {
        formData.append("ImageFile", imageFile);
    }

    var selectedCategories = [];
    $('.pct-checkbox:checked').each(function () {
        var catId = parseInt($(this).val());
        selectedCategories.push(catId);
        formData.append("SelectedCategory", catId);
    });

    const $submitButton = $('.btn-primary');
    const originalButtonText = $submitButton.text();

    $.ajax({
        url: "/Admin/Products/Edit",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        timeout: 30000,
        beforeSend: function () {
            $submitButton.prop('disabled', true).text('در حال ویرایش...');
            if (typeof showLoading === 'function') showLoading();
        },
        success: function (response) {
            if (response.success) {
                $("#modal-default").modal("hide");
                const successMessage = response.message || "محصول با موفقیت ویرایش شد";
                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(successMessage, "success", "center");
                } else {
                    alert(successMessage);
                }
                setTimeout(function () {
                    location.reload();
                }, 2000);
            } else {
                let errorMessage = response.message || "خطایی در ویرایش محصول رخ داده است";
                if (response.errors) {
                    if (typeof response.errors === 'object') {
                        errorMessage = Object.values(response.errors).join('\n');
                    } else if (Array.isArray(response.errors)) {
                        errorMessage = response.errors.join('\n');
                    }
                }
                if (typeof AlerSweetWithTimer === 'function') {
                    $("#modal-default").modal("hide");
                    AlerSweetWithTimer(errorMessage, "error", "center");
                }
                if (response.invalidFields && Array.isArray(response.invalidFields)) {
                    response.invalidFields.forEach(field => {
                        $(`#${field}`).addClass('is-invalid');
                        $(`.product-form-validation[data-for="${field}"]`).show();
                    });
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error Details:", {
                status: xhr.status,
                statusText: xhr.statusText,
                responseText: xhr.responseText,
                error: error
            });
            let errorMessage = "خطا در ارتباط با سرور";
            if (xhr.status === 0) {
                errorMessage = "عدم ارتباط با سرور. لطفا اتصال اینترنت خود را بررسی کنید.";
            } else if (xhr.status === 404) {
                errorMessage = "آدرس مورد نظر یافت نشد.";
            } else if (xhr.status === 500) {
                errorMessage = "خطای داخلی سرور. لطفا با پشتیبانی تماس بگیرید.";
            } else if (xhr.status === 400) {
                errorMessage = "درخواست نامعتبر. لطفا اطلاعات را بررسی کنید.";
            } else if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = xhr.responseJSON.message;
            } else if (xhr.responseText) {
                errorMessage = xhr.responseText.substring(0, 200);
            }
            if (typeof AlerSweetWithTimer === 'function') {
                AlerSweetWithTimer(errorMessage, "error", "center");
            } else {
                alert(errorMessage);
            }
        },
        complete: function () {
            $submitButton.prop('disabled', false).text(originalButtonText);
            if (typeof hideLoading === 'function') hideLoading();
        }
    });
}

(function ($) {
    'use strict';
    window.CharCounter = function (selector, counterSelector, maxLength) {
        var $element = $(selector);
        var $counter = $(counterSelector);
        var max = maxLength || 600;
        if ($element.length && $counter.length) {
            $element.on('input', function () {
                var count = $(this).val().length;
                $counter.text(count);
                if (count > max) {
                    $(this).val($(this).val().substring(0, max));
                    $counter.text(max);
                }
            });
            $counter.text($element.val().length);
        }
    };
})(jQuery);

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