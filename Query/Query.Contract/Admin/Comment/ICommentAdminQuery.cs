using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Comment
{
    public interface ICommentAdminQuery
    {
        Task<CommentForAdminPaging> GetForAdmin(int pageId, int take, string filter, int ownerId, CommentFor commentFor, CommentStatus commentStatus, int? parentId);
        Task<List<CommentAdminQueryModel>> GetAllUnseenCommentsForAdmin();
    }

  
}
