namespace NavinoShop.WebApplication.Utility
{
    // در یک فایل جداگانه به نام Constraints/SellerSlugConstraint.cs
    using Microsoft.AspNetCore.Routing;
    using System.Text.RegularExpressions;

    public class SellerSlugConstraint : IRouteConstraint
    {
        private readonly HashSet<string> _invalidSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "manifest.json", "favicon.ico", "robots.txt", "sitemap.xml",
        "browserconfig.xml", "site.webmanifest", ".well-known",
        "admin", "api", "swagger", "identity", "account", "profile",
        "products", "product", "home", "about", "contact", "blog",
        "css", "js", "images", "fonts", "lib", "assets", "uploads"
    };

        private readonly string[] _invalidExtensions = { ".json", ".ico", ".xml", ".txt", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".webp" };
        private readonly string[] _invalidPrefixes = { "_", ".", "-" };

        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.TryGetValue(routeKey, out var value))
                return false;

            var slug = value?.ToString()?.Trim()?.ToLower();

            if (string.IsNullOrEmpty(slug))
                return false;

            // بررسی slugs ممنوع
            if (_invalidSlugs.Contains(slug))
                return false;

            // بررسی پسوندهای ممنوع
            if (_invalidExtensions.Any(ext => slug.EndsWith(ext)))
                return false;

            // بررسی شروع با کاراکترهای ممنوع
            if (_invalidPrefixes.Any(prefix => slug.StartsWith(prefix)))
                return false;

            // بررسی وجود نقطه (برای فایل‌ها)
            if (slug.Contains('.'))
                return false;

            // حداقل طول (اختیاری)
            if (slug.Length < 2)
                return false;

            // حداکثر طول (اختیاری - جلوگیری از حملات)
            if (slug.Length > 100)
                return false;

            // بررسی کاراکترهای مجاز (حروف، اعداد، خط تیره، زیرخط و فارسی)
            if (!Regex.IsMatch(slug, @"^[a-zA-Z0-9_\-\u0600-\u06FF]+$"))
                return false;

            return true;
        }
    }

    // کلاس برای slugs فارسی (اختیاری)
    public class PersianSlugConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value))
            {
                var slug = value?.ToString();
                return !string.IsNullOrEmpty(slug) && Regex.IsMatch(slug, @"^[\u0600-\u06FF0-9\-_]+$");
            }
            return false;
        }
    }
}
