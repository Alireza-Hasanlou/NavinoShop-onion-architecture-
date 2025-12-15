namespace Shared.Domain.Enums
{
    public enum MenuStatus
    {
        منوی_گروه_محصولات,
        دسته_های_گروه_مجصولات,
        زیردسته_های_گروه_محصولات,
        محصولات_در_زیردسته_گروه_محصولات,
        منوی_اصلی,
        منوی_اصلی_با_زیر_منو,
        زیرمنوی_سردسته,
        تیتر_منوی_فوتر,
        منوی_فوتر,
        منوی_وبلاگ_لینک,

    }
    public static class EnumExtensions
    {
        public static string ToFriendlyString(this MenuStatus status)
        {
            return status.ToString().Replace("_", " ");
        }
    }


}
