
function DeleteAjax(Title, Text1, Icon, ConfirmButtonText, Url, DeletedId) {
    Swal.fire({
        title: Title,
        text: Text1,
        icon: Icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: ConfirmButtonText,
        cancelButtonText: "لغو"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: Url,
                type: "GET",
                data: { id: DeletedId },
                success: function (res) {
                    if (res.success) {
                        AlerSweetWithTimer(res.title, "success", "center");
                        setTimeout(function () {
                            $(`#${DeletedId}`).hide();
                        }, 1000);
                    } else {
                        if (res.errors) {
                            $('#resultMessage').text(res.errors);
                        }
                    }
                },
                error: function () {
                    AlerSweetWithTimer("خطای سمت سرور", "error", "center");
                }
            });
        }
    });
}
function AjaxSweetNotDelete(title1, text1, icon1, confirmButtonText1, cancelButtonText1, url1, id) {
  
    Swal.fire({
        title: title1,
        text: text1,
        icon: icon1,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonText1,
        cancelButtonText: cancelButtonText1
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: url1,
                type: "GET",
                data: { id: id },
            }).done(function (res) {
                if (res && res.success) {

                    AlerSweetWithTimer(res.message || "عملیات موفق", "success", "center");
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                } else {
                    AlerSweetWithTimer(res.message || "عملیات ناموفق", "error", "center");
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                }
            }).fail(function () {
                AlerSweetWithTimer("خطای سمت سرور", "error", "center");
            });
        }


    });
}
function AjaxSweetInput(title1, confirmButtonText1, url1, deletedId) {

    close_Modal_Ajax();
    Swal.fire({
        title: title1,
        input: "text",
        inputAttributes: {
            autocapitalize: "off"
        },
        showCancelButton: true,
        confirmButtonText: confirmButtonText1,
        cancelButtonText: 'انصراف',
        showLoaderOnConfirm: true,
       
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result.isConfirmed) {
            debugger;
/*            Loding();*/
            $.ajax({
                type: "Get",
                url: url1 + result.value
            }).done(function (res) {
                if (res) {

                    AlerSweetWithTimer("عملیات موفق", "success", "Center");
                    setTimeout($(`#${deletedId}`).hide('slow'), 3000);

                }
                else {
                    AlerSweetWithTimer("عملیات نا موفق \n " + res.message, "error", "Center");
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                }
         /*       EndLoading();*/
            });
        }
    });
}
function AjaxSweetInputWithRedirect(title1, confirmButtonText1, url1, RedirectUrl) {

    Swal.fire({
        title: title1,
        input: "text",
        inputAttributes: {
            autocapitalize: "off"
        },
        showCancelButton: true,
        confirmButtonText: confirmButtonText1,
        cancelButtonText: 'انصراف',
        showLoaderOnConfirm: true,
        allowOutsideClick: () => !Swal.isLoading()
    }).then((result) => {
        if (result.isConfirmed) {
            Loding();

            $.ajax({
                type: "Get",
                url: url1 + result.value
            }).done(function (res) {
                if (res) {
                    AlerSweetWithTimer("عملیات موفق", "success", "Center");
                    setTimeout(function () {
                        window.location.href = RedirectUrl;
                    }, 3000);
                } else {
                    AlerSweetWithTimer("عملیات نا موفق \n " + res.message, "error", "Center");
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                }
                EndLoading();
            });
        }
    });
}
function AjaxSweet(title1, text1, icon1, confirmButtonText1, cancelButtonText1, url1, deletedId, successCallback, errorCallback) {
    close_Modal_Ajax();
    Swal.fire({
        title: title1,
        text: text1,
        icon: icon1,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonText1,
        cancelButtonText: cancelButtonText1
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: url1,
                data: { productId: deletedId },
                success: function (res) {
                    if (res.success) {
                        if (typeof AlerSweetWithTimer === 'function') {
                            AlerSweetWithTimer("عملیات موفق", "success", "Center");
                        }

                        if (typeof successCallback === 'function') {
                            successCallback(res);
                        }

                        if (deletedId) {
                            setTimeout(() => {
                                $(`#${deletedId}`).fadeOut('slow');
                            }, 1000);
                        }
                    } else {
                        if (typeof AlerSweetWithTimer === 'function') {
                            AlerSweetWithTimer(res.message || "عملیات ناموفق", "error", "Center");
                        }

                        if (typeof errorCallback === 'function') {
                            errorCallback(res);
                        }
                    }
                },
                error: function (xhr, status, error) {
                    if (typeof AlerSweetWithTimer === 'function') {
                        AlerSweetWithTimer("خطا در برقراری ارتباط با سرور", "error", "Center");
                    }

                    if (typeof errorCallback === 'function') {
                        errorCallback(null);
                    }
                }
            });
        }
    });
}
function AjaxSweetWithRedirect(title1, text1, icon1, confirmButtonText1, cancelButtonText1, url1, RedirectUrl) {
   
    Swal.fire({
        title: title1,
        text: text1,
        icon: icon1,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: confirmButtonText1,
        cancelButtonText: cancelButtonText1
    }).then((result) => {

        if (result.isConfirmed) {
  /*          Loding();*/
            console.log("Start Load");
            $.ajax({
                type: "GET",
                url: url1
            })
                .done(function (res) {
                 /*   EndLoading();*/

                    if (res) {
                        AlerSweetWithTimer("عملیات موفق", "success", "Center");


                        setTimeout(function () {
                            window.location.href = RedirectUrl;
                        }, 3000);
                    } else {
                        AlerSweetWithTimer("عملیات ناموفق", "error", "Center");
                    }
                })
                .fail(function () {

                    AlertSweetTimer("خطا در برقراری ارتباط با سرور", "error", "Center");
              /*      EndLoading();*/
                });


        }
    });
}
function AjaxSweetRefresh(title, text, icon, confirmText, cancelText, url) {
  
    Swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: confirmText,
        cancelButtonText: cancelText
    }).then(result => {

        if (!result.isConfirmed) return;

    
        $.ajax({
            type: "GET",
            url: url
        })
            .done(res => {
             
                if (res.success) {
                    AlerSweetWithTimer("عملیات موفق", "success", "Center");

                    setTimeout(() => location.reload(), 2000);
                }
                else {
                    AlerSweetWithTimer("عملیات ناموفق", "error", "Center");
                }
            })
            .fail(() => {
                AlerSweetWithTimer("خطا در ارتباط با سرور", "error", "Center");
            });
    });
}

async function showReplyAlert(options) {

    const {
        title = "پاسخ",
        inputLabel = "پیام",
        placeholder = "متن پیام را وارد کنید...",

        smsText = "پاسخ با پیامک",
        emailText = "پاسخ با ایمیل",
        cancelText = "لغو",

        smsUrl,
        emailUrl,

        messageElementId,

        data = {}
    } = options;


    let message = "";


    const result = await Swal.fire({

        title: title,

        input: "textarea",

        inputLabel: inputLabel,

        inputPlaceholder: placeholder,

        inputAttributes: {
            "aria-label": placeholder
        },

        showCancelButton: true,

        cancelButtonText: cancelText,

        showDenyButton: true,

        denyButtonText: smsText,

        showConfirmButton: true,

        confirmButtonText: emailText,

        reverseButtons: true,


        // ایمیل
        preConfirm: (text) => {

            if (!text || !text.trim()) {

                Swal.showValidationMessage(
                    "لطفاً متن پیام را وارد کنید."
                );

                return false;
            }

            message = text.trim();

            return message;
        },


        // پیامک
        preDeny: () => {

            const text = Swal.getInput()?.value;

            if (!text || !text.trim()) {

                Swal.showValidationMessage(
                    "لطفاً متن پیام را وارد کنید."
                );

                return false;
            }

            message = text.trim();

            return true;
        }
    });


    // لغو
    if (result.isDismissed) {
        return;
    }


    let url;
    let type;


    // پیامک
    if (result.isDenied) {

        url = smsUrl;
        type = "sms";
    }

    // ایمیل
    else if (result.isConfirmed) {

        url = emailUrl;
        type = "email";
    }


    if (!url) {

        console.error("URL عملیات مشخص نشده است.");

        return;
    }


    // در اینجا دیگر از Swal.getInput() استفاده نمی‌کنیم
    // چون Alert بسته شده است.
    console.log("URL:", url);
    console.log("Type:", type);
    console.log("Message:", message);
    console.log("Message ID:", data.messageId);


    try {

        const response = await $.ajax({

            url: url,

            type: "POST",

            contentType: "application/json; charset=utf-8",

            dataType: "json",

            data: JSON.stringify({

                id: data.messageId,

                message: message
            })
        });


        // بررسی نتیجه اکشن
        if (!response || response.ok !== true) {

            throw new Error(
                "عملیات با موفقیت انجام نشد."
            );
        }


        // پیام موفقیت
        await Swal.fire({

            icon: "success",

            title: "موفق",

            text: type === "sms"
                ? "پاسخ با پیامک ارسال شد."
                : "پاسخ با ایمیل ارسال شد."
        });


        // مخفی کردن پیام
        if (messageElementId) {

            const element =
                document.getElementById(messageElementId);

            if (element) {

                element.style.display = "none";
            }
        }

    }
    catch (xhr) {

        console.error("Reply Error:", xhr);

        console.error("Status:", xhr.status);

        console.error("Response:", xhr.responseText);


        Swal.fire({

            icon: "error",

            title: "خطا",

            text: xhr.responseText ||
                "ارسال پاسخ با خطا مواجه شد."
        });
    }
}

