using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.UI.Comments
{
    public interface ICommentsUiQueryService
    {
       Task< CommentsUiPaging> GetCommments(int ownerId,CommentFor commentFor,int pageId);
    }
}
