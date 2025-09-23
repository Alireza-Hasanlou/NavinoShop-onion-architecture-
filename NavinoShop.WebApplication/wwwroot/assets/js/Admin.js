$('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
    checkboxClass: 'icheckbox_flat-green',
    radioClass: 'iradio_flat-green'
})
//Role DataTable
$('.DataTable').DataTable({
    'paging': true,
    autoWidth: true,
    'lengthChange': true,
    'searching': true,
    lengthChange: false, 
    'ordering': true,
    'info': false,
    'autoWidth': true,
    language: {
        search: "جستجو:",
        paginate: {
            next: "بعدی",
            previous: "قبلی"

        }
    }
})

//Create Role
//$("#CreateRoleBtn").click(function (e) {
//    e.preventDefault();


//    var formData = {
//        "RoleTitle.Title": $("#RoleTitle").val(),
//        "permissions": $(".flat-red:checked").map(function () {
//            return parseInt($(this).val());
//        }).get()
//    };

//    $.ajax({
//        url: "/Admin/Roles/Create",
//        type: "POST",
//        data: formData,
//        success: function (res) {
//            if (res.success) {
//                AlerSweetWithTimer("عملیات موفق بود", "success", "center", "/Admin/Roles/AllRoles");
//            } else {
//                if (res.errors) {

//                    $('#resultMessage').text(res.errors);

//                }

//            }
//        },
//        error: function () {
//            AlerSweetWithTimer("خطای سمت سرور", "error", "center");
//        }
//    });

//});
//EditRole
//$("#EditRoleBtn").click(function (e) {
//    e.preventDefault();


//    var formData = {
//        "Role.Title": $("#RoleTitle").val(),
//        "Role.Id": $("#RoleId").val(),
//        "permissions": $(".flat-red:checked").map(function () {
//            return parseInt($(this).val());
//        }).get()
//    };

//    $.ajax({
//        url: "/Admin/Roles/Edit",
//        type: "POST",
//        data: formData,
//        success: function (res) {
//            if (res.success) {
//                AlerSweetWithTimer("عملیات موفق بود", "success", "center", "/Admin/Roles/AllRoles");
//            } else {
//                if (res.errors) {

//                    $('#resultMessage').text(res.errors);

//                }

//            }
//        },
//        error: function () {
//            AlerSweetWithTimer("خطای سمت سرور", "error", "center");
//        }
//    });

//});


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

    // فعال کردن DataTable
 
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

// تابع Ajax فقط دیتا می‌گیرد و به تابع بالا می‌دهد
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


$("#testRenderBtn").click(function () {
    const fakeData = {
        roleTitle: "ادمین",
        users: [
            { userAvatar: "default.png", userFullName: "علی رضایی", mobile: "09123456789" },
            { userAvatar: "default.png", userFullName: "مینا احمدی", mobile: "09351234567" }
        ]
    };
    renderRoleUsersModal(fakeData);
});

function makeSlug(source, destination) {

    var titleStr = $('#' + source).val();
    titleStr = titleStr.replace(/^\s+|\s+$/g, '');
    titleStr = titleStr.toLowerCase();
    titleStr = titleStr.replace(/[^a-z0-9_\s-ءاأإآؤئبتثجحخدذرزسشصضطظعغفقكلمنهويةى]#u/, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
    $('#' + destination).val(titleStr);
}
