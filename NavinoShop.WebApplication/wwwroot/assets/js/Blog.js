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

