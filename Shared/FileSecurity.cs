using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Shared
{
    public static class FileSecurity
    {
        public static bool IsImage(IFormFile file)
        {
            return true;    
        }
    }
}
