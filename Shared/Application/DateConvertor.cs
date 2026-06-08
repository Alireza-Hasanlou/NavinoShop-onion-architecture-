using System.Globalization;

namespace Shared.Application
{
    public static class DateConvertor
    {
        public static string[] MonthNames =
             {"فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند"};

        public static string[] DayNames = { "شنبه", "یکشنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه" };
        public static string[] DayNamesG = { "یکشنبه", "دو شنبه", "سه شنبه", "چهار شنبه", "پنج شنبه", "جمعه", "شنبه" };


        public static string ToPersainDate(this DateTime? date)
        {
            try
            {
                if (date != null) return date.Value.ToPersainDate();
            }
            catch (Exception)
            {
                return "";
            }

            return "";
        }

        public static string ToPersainDate(this DateTime date)
        {
            if (date == new DateTime()) return "";
            var pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date):00}/{pc.GetDayOfMonth(date):00}";
        }

        public static string ToDiscountFormat(this DateTime date)
        {
            if (date == new DateTime()) return "";
            return $"{date.Year}/{date.Month}/{date.Day}";
        }

        private static readonly string[] Pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
        private static readonly string[] En = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        public static string ToEnglishNumber(this string strNum)
        {
            if (string.IsNullOrEmpty(strNum)) return strNum;
            var cash = strNum;
            for (var i = 0; i < 10; i++)
                cash = cash.Replace(Pn[i], En[i]);
            return cash;
        }

        public static string ToPersianNumber(this int intNum)
        {
            var chash = intNum.ToString();
            for (var i = 0; i < 10; i++)
                chash = chash.Replace(En[i], Pn[i]);
            return chash;
        }

        public static DateTime ToEnglishDateTime(this string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
                throw new ArgumentNullException(nameof(persianDate));

            // تبدیل اعداد فارسی به انگلیسی
            persianDate = persianDate.ToEnglishNumber();

            // حذف فاصله‌ها
            persianDate = persianDate.Trim();

            // استفاده از Split به جای Substring
            var parts = persianDate.Split('/');

            if (parts.Length != 3)
                throw new FormatException($"فرمت تاریخ نامعتبر: {persianDate}");

            var year = Convert.ToInt32(parts[0]);
            var month = Convert.ToInt32(parts[1]);
            var day = Convert.ToInt32(parts[2]);

            // اعتبارسنجی ساده
            if (year < 1300 || year > 1500)
                throw new ArgumentOutOfRangeException($"سال {year} نامعتبر است");

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException($"ماه {month} نامعتبر است");

            if (day < 1 || day > 31)
                throw new ArgumentOutOfRangeException($"روز {day} نامعتبر است");

            return new DateTime(year, month, day, new PersianCalendar());
        }
    }
}