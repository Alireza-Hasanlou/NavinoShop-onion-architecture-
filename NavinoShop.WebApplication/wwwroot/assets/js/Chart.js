// ========================================================
// Document Ready
// ========================================================

$(document).ready(function () {

    loadMonthlySales();

    loadWeeklySales();

});


// ========================================================
// Monthly Sales
// ========================================================

function loadMonthlySales() {

    $.ajax({

        url: '/Admin/Chart/ChartData?handler=MonthlySales',

        type: 'GET',

        success: function (response) {

            console.log('Monthly Sales:', response);

            renderMonthlySalesChart(response);

        },

        error: function (xhr, status, error) {

            console.error(
                'خطا در دریافت فروش ماهانه:',
                error
            );

        }

    });

}


// ========================================================
// Monthly Sales Chart
// ========================================================

function renderMonthlySalesChart(data) {

    var monthlyColor = '#4F7CFF';


    var options = {

        series: [

            {

                name: 'فروش',

                data: data.map(function (item) {

                    return item.salesCount;

                })

            }

        ],


        chart: {

            height: 360,

            type: 'area',

            toolbar: {

                show: false

            },

            zoom: {

                enabled: false

            },

            parentHeightOffset: 0,

            animations: {

                enabled: true,

                easing: 'easeinout',

                speed: 700

            }

        },


        // =================================================
        // رنگ نمودار
        // =================================================

        colors: [

            monthlyColor

        ],


        dataLabels: {

            enabled: false

        },


        // =================================================
        // خط نمودار
        // =================================================

        stroke: {

            curve: 'smooth',

            width: 3,

            colors: [

                monthlyColor

            ]

        },


        // =================================================
        // نقاط نمودار
        // =================================================

        markers: {

            size: 0,

            colors: [

                monthlyColor

            ],

            strokeColors: '#ffffff',

            strokeWidth: 2,

            hover: {

                size: 7,

                sizeOffset: 2

            }

        },


        // =================================================
        // Gradient
        // =================================================

        fill: {

            type: 'gradient',

            colors: [

                monthlyColor

            ],

            gradient: {

                shadeIntensity: 1,

                opacityFrom: 0.32,

                opacityTo: 0.03,

                stops: [

                    0,

                    85,

                    100

                ]

            }

        },


        // =================================================
        // Grid
        // =================================================

        grid: {

            show: true,

            borderColor: '#edf0f4',

            strokeDashArray: 4,

            position: 'back',

            padding: {

                left: 10,

                right: 15,

                top: 0,

                bottom: 0

            }

        },


        // =================================================
        // محور X
        // =================================================

        xaxis: {

            categories: data.map(function (item) {

                return getMonthName(item.month);

            }),

            axisBorder: {

                show: false

            },

            axisTicks: {

                show: false

            },

            labels: {

                style: {

                    colors: '#8B929C',

                    fontSize: '12px'

                }

            }

        },


        // =================================================
        // محور Y
        // =================================================

        yaxis: {

            labels: {

                style: {

                    colors: '#8B929C',

                    fontSize: '12px'

                }

            }

        },


        // =================================================
        // Tooltip
        // =================================================

        tooltip: {

            theme: 'light',

            marker: {

                show: true

            },

            x: {

                show: true

            },

            y: {

                formatter: function (value) {

                    return value.toLocaleString('fa-IR')
                        + ' فروش';

                }

            }

        }

    };


    var chart = new ApexCharts(

        document.querySelector('#monthlySalesChart'),

        options

    );


    chart.render();

}


// ========================================================
// نام ماه‌های شمسی
// ========================================================

function getMonthName(month) {

    switch (month) {

        case 1:
            return 'فروردین';

        case 2:
            return 'اردیبهشت';

        case 3:
            return 'خرداد';

        case 4:
            return 'تیر';

        case 5:
            return 'مرداد';

        case 6:
            return 'شهریور';

        case 7:
            return 'مهر';

        case 8:
            return 'آبان';

        case 9:
            return 'آذر';

        case 10:
            return 'دی';

        case 11:
            return 'بهمن';

        case 12:
            return 'اسفند';

        default:
            return '';

    }

}


// ========================================================
// Weekly Sales
// ========================================================

function loadWeeklySales() {

    $.ajax({

        url: '/Admin/Chart/ChartData?handler=WeeklySales',

        type: 'GET',

        success: function (response) {

            console.log('Weekly Sales:', response);

            renderWeeklySalesChart(response);

        },

        error: function (xhr, status, error) {

            console.error(
                'خطا در دریافت فروش هفتگی:',
                error
            );

        }

    });

}


// ========================================================
// Weekly Sales Chart
// ========================================================

function renderWeeklySalesChart(data) {

    var weeklyColor = '#7C5CFC';


    var options = {

        series: [

            {

                name: 'تعداد فروش',

                data: data.map(function (item) {

                    return item.salesCount;

                })

            }

        ],


        chart: {

            height: 360,

            type: 'bar',

            toolbar: {

                show: false

            },

            parentHeightOffset: 0,

            animations: {

                enabled: true,

                easing: 'easeinout',

                speed: 600

            }

        },


        // =================================================
        // رنگ نمودار
        // =================================================

        colors: [

            weeklyColor

        ],


        // =================================================
        // تنظیمات ستون‌ها
        // =================================================

        plotOptions: {

            bar: {

                borderRadius: 8,

                columnWidth: '42%',

                distributed: false,

                dataLabels: {

                    position: 'top'

                }

            }

        },


        dataLabels: {

            enabled: false

        },


        // =================================================
        // Grid
        // =================================================

        grid: {

            show: true,

            borderColor: '#edf0f4',

            strokeDashArray: 4,

            position: 'back',

            padding: {

                left: 10,

                right: 15

            }

        },


        // =================================================
        // محور X
        // =================================================

        xaxis: {

            categories: data.map(function (item) {

                return getDayName(item.dayOfWeek);

            }),

            axisBorder: {

                show: false

            },

            axisTicks: {

                show: false

            },

            labels: {

                style: {

                    colors: '#8B929C',

                    fontSize: '12px'

                }

            }

        },


        // =================================================
        // محور Y
        // =================================================

        yaxis: {

            labels: {

                style: {

                    colors: '#8B929C',

                    fontSize: '12px'

                }

            }

        },


        // =================================================
        // Tooltip
        // =================================================

        tooltip: {

            theme: 'light',

            marker: {

                show: true

            },

            y: {

                formatter: function (value) {

                    return value.toLocaleString('fa-IR')
                        + ' فروش';

                }

            }

        }

    };


    var chart = new ApexCharts(

        document.querySelector('#weeklySalesChart'),

        options

    );


    chart.render();

}


// ========================================================
// نام روزهای هفته
// ========================================================

function getDayName(dayOfWeek) {

    switch (dayOfWeek) {

        case 0:
            return 'یکشنبه';

        case 1:
            return 'دوشنبه';

        case 2:
            return 'سه‌شنبه';

        case 3:
            return 'چهارشنبه';

        case 4:
            return 'پنجشنبه';

        case 5:
            return 'جمعه';

        case 6:
            return 'شنبه';

        default:
            return '';

    }

}
