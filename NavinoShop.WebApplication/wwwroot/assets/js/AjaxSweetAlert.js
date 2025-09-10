
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
                        AlerSweetWithTimer("عملیات موفق بود", "success", "center");
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
