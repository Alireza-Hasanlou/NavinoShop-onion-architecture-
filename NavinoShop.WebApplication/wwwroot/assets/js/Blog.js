function GetBestBlogs() {
    $.ajax({
        url: "/Blog/Blog/GetBestBlogs",
        type: "GET",
        dataType: "json"
    })
        .done(function (res) {
            console.log(res);

            var parent = $("#BestBlogParentul");
            parent.empty();

            for (var i = 0; i < res.length; i++) {

                var item = `
                <li class="tabs__item">
                    <a href="#parent_${res[i].CategoryId}" class="tabs__trigger">
                        ${res[i].CategoryTitle}
                    </a>
                </li>
            `;
                parent.append(item);
            }
        })
        .fail(function (xhr) {
            console.error("Ajax Error:", xhr.status, xhr.responseText);
        });
}
