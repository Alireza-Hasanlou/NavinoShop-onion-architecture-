//Title, Text1, Icon, ConfirmButtonText, CancelButtonText, Url,DeletedId
function AjaxSweetAlert(Title, Text1, Icon, ConfirmButtonText, CancelButtonText, Url,DeletedId) {
    Swal.fire({
        title: Title,
        text: Text1,
        icon: Icon,
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: ConfirmButtonText,
        cancelButtonText: CancelButtonText
    }).then((result) => {

        if (result.isConfirmed) {
            $ajax({
                type: "Get",
                url = Url,
            }).done(function(res) {

                if (res) {
                    AlerSweetWithTimer("?????? ?? ?????? ????? ?? ", "success", "center");
                    setTimeout($(`#${DeletedId}`).hide(), 3000);
                }
                else {
                    AlerSweetWithTimer("?????? ?? ??? ????? ?? ", "error", "center");
                }
            });




        }
    });
}
