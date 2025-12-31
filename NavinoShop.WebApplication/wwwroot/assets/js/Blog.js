function GetBestBlogs() {
    $.ajax({
        url: "/Blog/Blog/GetBestBlogs",
        type: "GET",
        dataType: "json"
    })
        .done(function (res) {
            

            var parent = $("#BestBlogParentul");
            console.log(res);
            parent.empty();

            for (var i = 0; i < res.length; i++) {
                if (i == 0) {
                    var item = `
                <li class="tabs__item tabs__item--active">
                    <a href="#parent_${res[i].categoryId}" class="tabs__trigger">
                        ${res[i].categoryTitle}
                    </a>
                </li>
            `;
                    parent.append(item);
                } else {
                    var item1 = `
                <li class="tabs__item">
                    <a href="#parent_${res[i].categoryId}" class="tabs__trigger">
                        ${res[i].categoryTitle}
                    </a>
                </li>
            `;
                    parent.append(item1);
                }


            }
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}
