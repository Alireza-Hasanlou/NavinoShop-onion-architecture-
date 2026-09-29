function goToProductsPage() {

    var query = $('#search-input').val().trim();

    // نمایش لودینگ
    $('#page-loader').css('display', 'flex');

    var url = '/Products';

    if (query !== '') {
        url += '?search=' + encodeURIComponent(query);
    }

    window.location.href = url;
}

$('#search-input').on('keypress', function (e) {

    if (window.location.pathname.toLowerCase().includes('/products')) {
        return;
    }

    if (e.which === 13) {
        e.preventDefault();
        goToProductsPage();
    }
});


$(window).on('pageshow', function () {
    $('#page-loader').hide();
});