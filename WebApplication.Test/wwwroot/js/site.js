
$(document).ready(function () {

    /* =======================
       Elements
    ======================= */
    const $province = $("#province");
    const $city = $("#city");
    const $destProvince = $("#Destinationprovince");
    const $destCity = $("#Destinationcity");
    const $weight = $("#weight");
    const $apiCode = $("#apiCode");
    const $calculateBtn = $("#calculateBtn");
    const $error = $("#errorMessage");
    const $loading = $("#loading");
    const $results = $("#results");
    const $selectedInfo = $("#selectedInfo");
    const $pricesList = $("#pricesList");

    /* =======================
       Load Provinces
    ======================= */
    function loadProvinces() {
        $.get("/Home/states")
            .done(res => {
                fillSelect($province, res);
                fillSelect($destProvince, res);
            })
            .fail(() => showError("خطا در بارگذاری استان‌ها"));
    }

    function fillSelect($select, items) {
        $select.empty().append('<option value="">انتخاب استان</option>');
        $.each(items, (_, item) => {
            $select.append(`<option value="${item.id}">${item.title}</option>`);
        });
    }

    /* =======================
       Load Cities
    ======================= */
    function loadCities(provinceId, $targetSelect) {
        if (!provinceId) {
            $targetSelect
                .html('<option value="" disabled selected>ابتدا استان را انتخاب کنید</option>')
                .prop("disabled", true);
            return;
        }

        $.get("/Home/Cities", { stateId: provinceId })
            .done(res => {
                $targetSelect.empty();

                if (!res || res.length === 0) {
                    $targetSelect
                        .append(`<option value="" disabled>شهری یافت نشد</option>`)
                        .prop("disabled", true);
                } else {
                    $targetSelect.append('<option value="" disabled selected>انتخاب شهر</option>');

                    $.each(res, (_, city) => {
                        $targetSelect.append(
                            `<option value="${city.cityCode}">${city.title}</option>`
                        );
                    });

                    $targetSelect.prop("disabled", false);
                }
            })
            .fail(() => showError("خطا در بارگذاری شهرها"));
    }

    /* =======================
       Click Calculate
    ======================= */
    $calculateBtn.on("click", function () {

        const sourceProvince = $province.val();
        const sourceCity = $city.val();
        const destProvince = $destProvince.val();
        const destCity = $destCity.val();
        const weightVal = $weight.val().trim();
        const apiCodeVal = $apiCode.val().trim();

        // اعتبارسنجی واضح‌تر و خواناتر
        if (!sourceProvince || sourceProvince === "") {
            showError("استان مبدا را انتخاب کنید");
            $province.focus();
            return;
        }
        if (!sourceCity || sourceCity === "") {
            showError("شهر مبدا را انتخاب کنید");
            $city.focus();
            return;
        }
        if (!destProvince || destProvince === "") {
            showError("استان مقصد را انتخاب کنید");
            $destProvince.focus();
            return;
        }
        if (!destCity || destCity === "") {
            showError("شهر مقصد را انتخاب کنید");
            $destCity.focus();
            return;
        }

        const weight = Number(weightVal);
        if (!weightVal || isNaN(weight) || weight < 1) {
            showError("وزن معتبر (بزرگتر از ۰) وارد کنید");
            $weight.focus().select();
            return;
        }

        if (!apiCodeVal) {
            showError("کد API را وارد کنید");
            $apiCode.focus().select();
            return;
        }

        // همه چیز درست است → ادامه می‌دهیم
        console.log({
            sourceCityId: Number(sourceCity),
            destinationCityId: Number(destCity),
            weight,
            apiCode: apiCodeVal
        });

        calculatePrice(Number(sourceCity), Number(destCity), weight, apiCodeVal);
    });

    /* =======================
       Calculate Price
    ======================= */
    function calculatePrice(sourceCityId, destinationCityId, weight, apiCode) {

        $loading.show();
        $results.hide();

        $.post("/Home/Calculate", {
            SourceCityId: sourceCityId,
            DestinationCityId: destinationCityId,
            Weight: weight,
            ApiCode: apiCode
        })
            .done(res => {
                $loading.hide();
                renderResults(res);
            })
            .fail(() => {
                $loading.hide();
                showError("خطا در محاسبه قیمت");
            });
    }

    /* =======================
       Render Results
    ======================= */
    function renderResults(res) {
        if (!res.success) {
            $pricesList.html('<div class="error">خطا در دریافت قیمت ها از درست بودن کد api اطمینان حاصل فرمایید</div>');
            return;
        }
        $selectedInfo.html(`
   
            <div><strong>مبدا:</strong> ${$province.find(":selected").text()} - ${$city.find(":selected").text()}</div>
            <div><strong>مقصد:</strong> ${$destProvince.find(":selected").text()} - ${$destCity.find(":selected").text()}</div>
            <div><strong>وزن:</strong> ${$weight.val()} گرم</div>
        `);

        $pricesList.empty();

        if (!res.prices || res.prices.length === 0) {
            $pricesList.html('<div class="error">قیمتی یافت نشد</div>');
        } else {
            $.each(res.prices, (_, item) => {
                $pricesList.append(`
                    <div class="price-item">
                        <div class="price-header">
                            <span>${item.title}</span>
                            <span>${item.price.toLocaleString()} تومان</span>
                        </div>
                        <div>زمان تحویل: ${item.status}</div>
                    </div>
                `);
            });
        }

        $results.show();
    }

    /* =======================
       Utils
    ======================= */
    function showError(msg) {
        $error.text(msg).show();
        setTimeout(() => $error.hide(), 4000);
    }

    /* =======================
       Events
    ======================= */
    $province.on("change", function () {
        loadCities(this.value, $city);
    });

    $destProvince.on("change", function () {
        loadCities(this.value, $destCity);
    });

    /* =======================
       Init
    ======================= */
    loadProvinces();
});

