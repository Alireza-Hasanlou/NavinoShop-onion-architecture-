// ================== iCheck (چک‌باکس و رادیو) ==================
$('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({ checkboxClass: 'icheckbox_flat-green', radioClass: 'iradio_flat-green' })
// ================== DataTable عمومی ==================
$('.DataTable').DataTable({
    paging: true,
    autoWidth: true,
    lengthChange: false,
    searching: true,
    ordering: true,
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


// ================== نمایش کاربران یک نقش ==================
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

    // فعال کردن DataTable داخل مودال
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

// Ajax برای گرفتن لیست کاربران نقش
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

//// تست رندر مودال
//$("#testRenderBtn").click(function () {
//    const fakeData = {
//        roleTitle: "ادمین",
//        users: [
//            { userAvatar: "default.png", userFullName: "علی رضایی", mobile: "09123456789" },
//            { userAvatar: "default.png", userFullName: "مینا احمدی", mobile: "09351234567" }
//        ]
//    };
//    renderRoleUsersModal(fakeData);
//});

// ================== تولید Slug ==================
function makeSlug(source, destination) {
    var titleStr = $('#' + source).val();
    titleStr = titleStr.replace(/^\s+|\s+$/g, '');
    titleStr = titleStr.toLowerCase();
    titleStr = titleStr.replace(/[^a-z0-9_\s-ءاأإآؤئبتثجحخدذرزسشصضطظعغفقكلمنهويةى]/g, '')
        .replace(/\s+/g, '-')
        .replace(/-+/g, '-');
    $('#' + destination).val(titleStr);
}

// ================== بارگذاری زیرگروه‌ها (Ajax SubCategories) ==================
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
