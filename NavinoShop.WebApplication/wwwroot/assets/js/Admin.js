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

function CreateProductFeatuer() {

    const config = {
        titleMinLength: 2,
        titleMaxLength: 100,
        valueMinLength: 1,
        valueMaxLength: 100
    };


    const $title = $("#FeatureTitle");
    const $value = $("#FeatuerValue");
    const $productId = $("#ProductId");
    const $errorMsg = $("#FeatureValidation");
    const $modal = $("#modal-default");
    const $submitBtn = $("#chargeBtn");
    $errorMsg.text("");
    $(".is-invalid").removeClass("is-invalid");


    let isValid = true;
    const title = $title.val().trim();
    const value = $value.val().trim();

    if (!title) {
        $errorMsg.text("لطفا عنوان ویژگی را وارد کنید");
        $title.addClass('is-invalid');
        isValid = false;
    } else if (title.length < config.titleMinLength) {
        $errorMsg.text(`عنوان باید حداقل ${config.titleMinLength} کاراکتر باشد`);
        $title.addClass('is-invalid');
        isValid = false;
    } else if (title.length > config.titleMaxLength) {
        $errorMsg.text(`عنوان باید حداکثر ${config.titleMaxLength} کاراکتر باشد`);
        $title.addClass('is-invalid');
        isValid = false;
    }

    if (!value && isValid) {
        $errorMsg.text("لطفا مقدار ویژگی را وارد کنید");
        $value.addClass('is-invalid');
        isValid = false;
    } else if (value && value.length > config.valueMaxLength && isValid) {
        $errorMsg.text(`مقدار باید حداکثر ${config.valueMaxLength} کاراکتر باشد`);
        $value.addClass('is-invalid');
        isValid = false;
    }

    if (!isValid) {
        const $firstInvalid = $('.is-invalid:first');
        if ($firstInvalid.length) {
            $('html, body').animate({
                scrollTop: $firstInvalid.offset().top - 100
            }, 300);
        }
        return;
    }



    const formData = new FormData();
    formData.append("ProductId", $productId.val());
    formData.append("Title", title);
    formData.append("Value", value);


    $submitBtn.prop('disabled', true).text('در حال ثبت...');

    $.ajax({
        url: "/Admin/ProductFeatures/Create",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false, 
        contentType: false,  
        cache: false
    })
        .done(function (res) {
            if (res.success) {
                $modal.modal("hide");

      
                $title.val("");
                $value.val("");

                AlerSweetWithTimer("ویژگی جدید با موفقیت اضافه شد", "success", "center");

                setTimeout(() => location.reload(), 3000);
            } else {
                $errorMsg.text(res.message).addClass("text-danger");
                $submitBtn.prop('disabled', false).text('ایجاد ویژگی');
            }
        })
        .fail(function (xhr) {
            let errorMsg = "خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.";

            if (xhr.status === 400) {
                errorMsg = "اطلاعات ارسال شده معتبر نیست.";
            } else if (xhr.status === 500) {
                errorMsg = "خطای داخلی سرور. لطفاً با پشتیبانی تماس بگیرید.";
            }

            $errorMsg.text(errorMsg).addClass("text-danger");
            console.error("Ajax Error:", xhr.status, xhr.responseText);

            $submitBtn.prop('disabled', false).text('ایجاد ویژگی');
  
        });
}
function EditProductFeatuer() {

    const config = {
        titleMinLength: 2,
        titleMaxLength: 100,
        valueMinLength: 1,
        valueMaxLength: 100
    };


    const $title = $("#FeatureTitle");
    const $value = $("#FeatuerValue");
    const $Id = $("#FeatureId");
    const $errorMsg = $("#FeatureValidation");
    const $modal = $("#modal-default");
    const $submitBtn = $("#chargeBtn");
    $errorMsg.text("");
    $(".is-invalid").removeClass("is-invalid");


    let isValid = true;
    const title = $title.val().trim();
    const value = $value.val().trim();

    if (!title) {
        $errorMsg.text("لطفا عنوان ویژگی را وارد کنید");
        $title.addClass('is-invalid');
        isValid = false;
    } else if (title.length < config.titleMinLength) {
        $errorMsg.text(`عنوان باید حداقل ${config.titleMinLength} کاراکتر باشد`);
        $title.addClass('is-invalid');
        isValid = false;
    } else if (title.length > config.titleMaxLength) {
        $errorMsg.text(`عنوان باید حداکثر ${config.titleMaxLength} کاراکتر باشد`);
        $title.addClass('is-invalid');
        isValid = false;
    }

    if (!value && isValid) {
        $errorMsg.text("لطفا مقدار ویژگی را وارد کنید");
        $value.addClass('is-invalid');
        isValid = false;
    } else if (value && value.length > config.valueMaxLength && isValid) {
        $errorMsg.text(`مقدار باید حداکثر ${config.valueMaxLength} کاراکتر باشد`);
        $value.addClass('is-invalid');
        isValid = false;
    }

    if (!isValid) {
        const $firstInvalid = $('.is-invalid:first');
        if ($firstInvalid.length) {
            $('html, body').animate({
                scrollTop: $firstInvalid.offset().top - 100
            }, 300);
        }
        return;
    }



    const formData = new FormData();
    formData.append("Id", $Id.val());
    formData.append("Title", title);
    formData.append("Value", value);


    $submitBtn.prop('disabled', true).text('در حال ثبت...');

    $.ajax({
        url: "/Admin/ProductFeatures/Edit",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,
        contentType: false,
        cache: false
    })
        .done(function (res) {
            if (res.success) {
                $modal.modal("hide");


                $title.val("");
                $value.val("");

                AlerSweetWithTimer("ویژگی جدید با موفقیت ویرایش شد", "success", "center");

                setTimeout(() => location.reload(), 3000);
            } else {
                $errorMsg.text(res.message).addClass("text-danger");
                $submitBtn.prop('disabled', false).text('ویرایش ویژگی');
                EndLoading();
            }
        })
        .fail(function (xhr) {
            let errorMsg = "خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.";

            if (xhr.status === 400) {
                errorMsg = "اطلاعات ارسال شده معتبر نیست.";
            } else if (xhr.status === 500) {
                errorMsg = "خطای داخلی سرور. لطفاً با پشتیبانی تماس بگیرید.";
            }

            $errorMsg.text(errorMsg).addClass("text-danger");
            console.error("Ajax Error:", xhr.status, xhr.responseText);

            $submitBtn.prop('disabled', false).text('ویرایش ویژگی');

        });
        
}
function CreateGallery() {
    // تنظیمات
    const config = {
        altMaxLength: 200,
        maxFileSize: 5 * 1024 * 1024, // 5 مگابایت
        allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'webp']
    };

    // دریافت عناصر
    const $imageAlt = $("#ImageAlt");
    const $imageFile = $("#upload-file");
    const $errorMsg = $("#galleryErrorMessage"); // اگر المان خاصی برای خطا دارید
    const $modal = $("#modal-default");

    // پاک کردن خطاهای قبلی
    $(".product-form-validation").hide();
    $(".is-invalid").removeClass("is-invalid");
    if ($errorMsg.length) $errorMsg.text("").hide();

    let isValid = true;

    // 1. اعتبارسنجی Alt
    const altValue = $imageAlt.val().trim();
    if (!altValue) {
        $('.product-form-validation[data-for="ImageAlt"]').show();
        $imageAlt.addClass('is-invalid');
        isValid = false;
    } else if (altValue.length > config.altMaxLength) {
        $('.product-form-validation[data-for="ImageAlt"]').text(`Alt نمی‌تواند بیشتر از ${config.altMaxLength} کاراکتر باشد`).show();
        $imageAlt.addClass('is-invalid');
        isValid = false;
    }

    // 2. اعتبارسنجی فایل تصویر
    const imageFile = $imageFile[0].files[0];
    if (!imageFile) {
        $('.product-form-validation[data-for="ImageFile"]').show();
        $imageFile.addClass('is-invalid');
        isValid = false;
    } else {
        // بررسی حجم فایل
        if (imageFile.size > config.maxFileSize) {
            $('.product-form-validation[data-for="ImageFile"]').text(`حجم فایل نباید بیشتر از ${config.maxFileSize / (1024 * 1024)} مگابایت باشد`).show();
            $imageFile.addClass('is-invalid');
            isValid = false;
        }

        // بررسی پسوند فایل
        const fileExtension = imageFile.name.split('.').pop().toLowerCase();
        if (!config.allowedExtensions.includes(fileExtension)) {
            $('.product-form-validation[data-for="ImageFile"]').text(`پسوندهای مجاز: ${config.allowedExtensions.join(', ')}`).show();
            $imageFile.addClass('is-invalid');
            isValid = false;
        }
    }

    // اگر اعتبارسنجی ناموفق بود
    if (!isValid) {
        const $firstInvalid = $('.is-invalid:first');
        if ($firstInvalid.length) {
            $('html, body').animate({
                scrollTop: $firstInvalid.offset().top - 100
            }, 300);
        }
        return false;
    }


    const formData = new FormData();
    formData.append("ImageAlt", altValue);
    formData.append("ImageFile", imageFile);
    formData.append("ProductId", $("#ProductId").val()); 


    const $submitBtn = $("#submitGalleryBtn"); 
    if ($submitBtn.length) {
        $submitBtn.prop('disabled', true).text('در حال ارسال...');
    }

    $.ajax({
        url: "/Admin/ProductGallery/Create",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  
        contentType: false,  
        cache: false,
        timeout: 30000 
    })
        .done(function (response) {
            if (response.success) {
           
                if ($modal.length) $modal.modal("hide");

           
                $imageAlt.val("");
                $imageFile.val("");

                if (typeof AlerSweetWithTimer === 'function') {
                    AlerSweetWithTimer(response.message || "تصویر با موفقیت اضافه شد", "success", "center");
                } else {
                    alert(response.message || "تصویر با موفقیت اضافه شد");
                }
                setTimeout(function () {
                    location.reload();
                }, 2000);
            } else {
   
                const errorMsg = response.message || "خطا در ثبت تصویر";
                if ($errorMsg.length) {
                    $errorMsg.text(errorMsg).addClass("text-danger").show();
                } else {
                    $('.product-form-validation[data-for="ImageFile"]').text(errorMsg).show();
                }

                // فعال کردن مجدد دکمه
                if ($submitBtn.length) {
                    $submitBtn.prop('disabled', false).text('ایجاد تصویر');
                }

            }
        })
        .fail(function (xhr) {
            let errorMsg = "خطا در ارتباط با سرور. لطفاً مجدداً تلاش کنید.";

            if (xhr.status === 400) {
                errorMsg = "اطلاعات ارسال شده معتبر نیست.";
     
                if (xhr.responseJSON && xhr.responseJSON.errors) {
                    const errors = xhr.responseJSON.errors;
                    for (let key in errors) {
                        if (errors.hasOwnProperty(key)) {
                            $(`.product-form-validation[data-for="${key}"]`).text(errors[key].join(', ')).show();
                            $(`#${key}`).addClass('is-invalid');
                        }
                    }
                }
            } else if (xhr.status === 413) {
                errorMsg = "حجم فایل ارسالی بیش از حد مجاز است.";
            } else if (xhr.status === 500) {
                errorMsg = "خطای داخلی سرور. لطفاً با پشتیبانی تماس بگیرید.";
            }

            console.error("Ajax Error:", xhr.status, xhr.responseText);

            if ($errorMsg.length) {
                $errorMsg.text(errorMsg).addClass("text-danger").show();
            } else {
                alert(errorMsg);
            }

            if ($submitBtn.length) {
                $submitBtn.prop('disabled', false).text('ایجاد تصویر');
            }
        });
}