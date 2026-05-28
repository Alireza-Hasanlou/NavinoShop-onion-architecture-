function goToProductsPage() {
    var query = $('#search-input').val().trim();
    var url = '/Products';
    if (query !== '') {
        url += '?search=' + encodeURIComponent(query);
    }
    window.location.href = url;  // هدایت کامل صفحه
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
