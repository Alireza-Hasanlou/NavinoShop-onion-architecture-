
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
                type: "GET", // یا بهتره POST باشه
                data: { id: DeletedId }, // 👈 آیدی اینجاست
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
                    setTimeout($(`#${deletedId}`).hide('slow'), 3000);

                }
                else {
                    AlerSweetWithTimer("عملیات نا موفق", "error", "Center");
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                }
                EndLoading();
            });
        }
    });
}
function AjaxSweet(title1, text1, icon1, confirmButtonText1, cancelButtonText1, url1, deletedId) {
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
            Loding();
            console.log("Start Load");
            $.ajax({
                type: "GET",
                url: url1
            })
                .done(function (res) {
                    EndLoading();

                    if (res) {
                        AlerSweetWithTimer("عملیات موفق", "success", "Center");

                      
                        setTimeout(() => {
                            $(`#${deletedId}`).fadeOut('slow');
                        }, 1000);
                    } else {
                        AlerSweetWithTimer("عملیات ناموفق", "error", "Center");
                    }
                })
                .fail(function () {

                    AlertSweetTimer("خطا در برقراری ارتباط با سرور", "error", "Center");
                    EndLoading();
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

        Loding();

        $.ajax({
            type: "GET",
            url: url
        })
            .done(res => {
                EndLoading();

                if (res.success ) {
                    AlerSweetWithTimer("عملیات موفق", "success", "Center");

                    setTimeout(() => location.reload(), 2000);
                }
                else {
                    AlerSweetWithTimer("عملیات ناموفق", "error", "Center");
                }
            })
            .fail(() => {
                EndLoading();
                AlerSweetWithTimer("خطا در ارتباط با سرور", "error", "Center");
            });
    });
}


