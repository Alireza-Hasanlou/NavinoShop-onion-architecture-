using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Site.Page
{
    public interface ISitePageUiQueryService
    {
        Task<PageUiQueryModel> GetPageAsync(string slug);
    }
}
