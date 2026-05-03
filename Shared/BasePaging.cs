using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BasePaging
    {
        public int PageId { get; private set; }
        public int PageCount { get; private set; }
        public int Take { get; private set; }
        public int DataCount { get; private set; }
        public int Skip { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public void GetData(IQueryable<object> data, int pageId, int take, int showPageCount)
        {
            if (take < 1 || take > 100)
            {
                Take = 10;
            }
            else
            {
                Take = take;
            }
            PageCount = data.Count() / Take;
            if (data.Count() % Take > 0) PageCount++;
            if (PageCount == 0) PageCount = 1;
            if (pageId < 1)
            {
                PageId = 1;
            }
            else
            {
                PageId = pageId;
            }
            if (PageId > PageCount) PageId = PageCount;
            DataCount = data.Count();
            Skip = (PageId - 1) * Take;
            if (showPageCount < 1) showPageCount = 2;
            StartPage = PageId > showPageCount ? PageId - showPageCount : 1;
            EndPage = PageCount - PageId > showPageCount ? PageId + showPageCount : PageCount;
        }

    }
}
