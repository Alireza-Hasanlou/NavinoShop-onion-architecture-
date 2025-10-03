using Microsoft.AspNetCore.Http;


namespace Shared
{
    public static class FileSecurity
    {
        public static bool IsImage(this IFormFile file)
        {
            return true;    
        }
    }
}
