function GetBestBlogs() {
    $.ajax({
        url: "/Blog/Blog/GetBestBlogs",
        type: "GET",
        dataType: "json"
    })
        .done(function (res) {

            var parent = $("#BestBlogParentul");
            var childs = $("#BestBlogChilddiv");

            parent.empty();
            childs.empty();

            console.log(res);

            for (let i = 0; i < res.length; i++) {

                // ---------- Tabs ----------
                parent.append(`
                <li class="tabs__item ${i === 0 ? 'tabs__item--active' : ''}">
                    <a href="#parent_${res[i].categoryId}" class="tabs__trigger">
                        ${res[i].categoryTitle}
                    </a>
                </li>
            `);

                // ---------- Content Pane ----------
                childs.append(`
                <div class="tabs__content-pane ${i === 0 ? 'tabs__content-pane--active' : ''}"
                     id="parent_${res[i].categoryId}">
                    <div class="row card-row" id="childdiv_${res[i].categoryId}"></div>
                </div>
            `);

                var blogparent = $(`#childdiv_${res[i].categoryId}`);

                // ---------- Blogs ----------
                for (let j = 0; j < res[i].blogs.length; j++) {

                    blogparent.append(`
                    <div class="col-md-6">
                        <article class="entry card">
                            <div class="entry__img-holder card__img-holder">
                                <a href="/Blog/${res[i].blogs[j].blogSlug}">
                                    <div class="thumb-container thumb-70">
                                        <img src="${res[i].blogs[j].imageName}"
                                             alt="${res[i].blogs[j].imageAlt}">
                                    </div>
                                </a>
                                <a href="/Blog/${res[i].categorySlug}"
                                   class="entry__meta-category entry__meta-category--label entry__meta-category--violet">
                                   ${res[i].categoryTitle}
                                </a>
                            </div>

                            <div class="entry__body card__body">
                                <h2 class="entry__title">
                                    <a href="/Blog/${res[i].blogs[j].blogSlug}">
                                        ${res[i].blogs[j].title}
                                    </a>
                                </h2>

                                <ul class="entry__meta">
                                    <li>
                                        <span>نویسنده:</span>
                                        ${res[i].blogs[j].writer}
                                    </li>
                                    <li>${res[i].blogs[j].createDate}</li>
                                </ul>

                                <p>${res[i].blogs[j].shortDescription} ...</p>
                            </div>
                        </article>
                    </div>
                `);
                }
            }
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}
$(document).on("click", ".tabs__trigger", function (e) {
    e.preventDefault();

    // remove active from all tabs
    $(".tabs__item").removeClass("tabs__item--active");
    $(".tabs__content-pane").removeClass("tabs__content-pane--active");

    // activate clicked tab
    $(this).closest(".tabs__item").addClass("tabs__item--active");

    // activate related content
    var target = $(this).attr("href");
    $(target).addClass("tabs__content-pane--active");
});

function GetComments(pageId, OwnerId) {

    var parent = $("#CommentParentli");

    $.ajax({
        url: "/Blog/GetBlogsComments",
        type: "GET",
        data: { ownerId: OwnerId, pageId: pageId },
        dataType: "json"
    })
        .done(function (res) {

            console.log(res.comments);
            for (let i = 0; i < res.comments.length; i++) {

                // ---------- Tabs ----------
                parent.append(`
                                     <div id="commentbody" class="comment-body" >
                                                    <div class="comment-avatar">
                                                        <img alt="" src="${res.comments[i].imageName}">
                                                    </div>
                                                    <div class="comment-text" id="body_${OwnerId}>
                                                        <h6 class="comment-author">${res.comments[i].fullName} </h6>
                                                        <div class="comment-metadata">
                                                            <a href="#" class="comment-date">${res.comments[i].createDaate}</a>
                                                        </div>
                                                        <p>${res.comments[i].text}</p>
                                                        <a onclick="OpenReplyInput('${res.comments[i].fullName}',${OwnerId},${res.comments[i].id})" class="comment-reply">پاسخ</a>
                                                    </div>
                                                </div>
                                                 <ul class="children" id="Child_${res.comments[i].id}"  >
                                                  </ul>

                                         `);

                var id = `Child_${res.comments[i].id}`;
                var replyParent = $(`#${id}`);
                for (let j = 0; j < res.comments[i].replys.length; j++) {
                    replyParent.append(`

                                       <li class="comment">
                                                        <div id="CommentReplyBody" class="comment-body">
                                                            <div class="comment-avatar">
                                                                <img alt="" src="${res.comments[i].replys[j].imageName}">
                                                            </div>
                                                            <div class="comment-text">
                                                                <h6 class="comment-author">${res.comments[i].replys[j].fullName}</h6>
                                                                <div class="comment-metadata">
                                                                    <a href="#" class="comment-date">${res.comments[i].replys[j].createDaate} </a>
                                                                </div>
                                                                 <div class="comment-metadata">
                                                                    <a style="color: #6e6ef4;" href="#" class="comment-author text-primary">پاسخ به :${res.comments[i].fullName} </a>
                                                                    <span class="CommentReply">(${res.comments[i].text})</span>
                                                                </div>
                                                                <p>${res.comments[i].replys[j].text}</p>
                                                                <a onclick="OpenReplyInput(${OwnerId})" class="comment-reply">پاسخ</a>
                                                            </div>
                                                        </div>
                                                    </li>
                                                  
                                         `);


                }

                replyParent.append(`  <hr/>`);

            }
            var loadDiv = $("#loadMoreCommentDiv");
            loadDiv.empty();
            if (res.pageCount > res.pageId + 1) {
                var pageIdplus = 0;
                pageIdplus = res.pageId + 1;
                loadDiv.append(`<a  onclick="LoadMoreComments(${pageIdplus},${res.ownerId} )">بیشتر</a>`)

            }

        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });



}
function LoadMoreComments(pageId, ownerId) {


    GetComments(pageId, ownerId);

}
function AddComment(ownerId) {

    var comment = $("#commenttext").val();

    if (comment === null || comment === "") {

        $("span.errorSpan").text("این فیلد اجباری است");
        return;
    }
    $("span.errorSpan").text(" ");
    $.ajax({
        url: "/Blog/AddComment",
        type: "GET",
        data: { comment: comment, ownerId: ownerId },
        dataType: "json"
    })
        .done(function (res) {

            if (res.success) {
                AlerSweetWithTimer("کامنت شما ارسال شد و پس از باز بینی منتشر خواهد شد", "success", "center");
            }
            else {
                AlerSweetWithTimer(res.message, "error", "center");
            }

        }).fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
    $("#commenttext").val('');
}

function OpenReplyInput(fullName, ownerId, parentId) {

    AjaxSweetInputReplyComment(`پاسخ به ${fullName}`, `ارسال`, `/Blog/AddComment?ownerId=${ownerId}&parentId=${parentId}&comment=`);
}

function AjaxSweetInputReplyComment(title1, confirmButtonText1, url1) {
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
                    AlerSweetWithTimer("نظر شما دریافت شد و پس از بازبینی منتشر خواهد شد", "success", "Center");


                }
                else {
                    AlerSweetWithTimer("عملیات نا موفق", "error", "Center");

                }
                EndLoading();
            });
        }
    });
}