
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
function AjaxSweetNotDelete(title1, text1, icon1, confirmButtonText1, cancelButtonText1, url1,id) {
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
        } else {
            location.reload();
        }

    });
}
