using Blogs.Domain.BlogAgg;
using Comments.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.UI.Comments;
using Shared.Application;
using Shared.Domain.Enums;
using Users.Domain.User.Agg;
using Users.Domain.User.Agg.IRepository;
namespace Query.Service.Ui.Comments
{
    internal class CommentsUiQueryService : ICommentsUiQueryService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentsUiQueryService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<CommentsUiPaging> GetCommments(int ownerId, CommentFor commentFor, int pageId)
        {
            var result = new CommentsUiPaging();
            var comments = _commentRepository.GetAllBy(c => c.OwnerId == ownerId
            && c.CommentFor == CommentFor.مقاله
            && c.Status == CommentStatus.تایید_شده)
                .Select(c => new
                {
                    c.OwnerId,
                    c.FullName,
                    c.Email,
                    c.CreateDate,
                    c.Text,
                    c.Childs,
                    c.Id,
                    c.UserId,
                    c.ParentId
                    

                });


            if (comments.Count() > 0)
            {


                result.GetData(comments, pageId, 2, 2);
                result.CommentFor = CommentFor.مقاله;
                result.OwnerId = ownerId;
                result.Comments = comments.Where(p=>p.ParentId==null)
                    .Skip(result.Skip)
                    .Take(result.Take)
                    .OrderByDescending(c => c.CreateDate)
                    .Select(comment => new CommentUiQueryModel
                    {

                        Id = comment.Id,
                        FullName = comment.FullName,
                        CreateDaate = comment.CreateDate.ToPersainDate(),
                        Text = comment.Text,
                        ImageName = "",
                        UserId = comment.UserId,
                        Replys = new()
                    }).ToList();
                foreach (var item in result.Comments)
                {
                    item.Replys = _commentRepository.GetAllBy(c => c.OwnerId == ownerId
                        && c.CommentFor == CommentFor.مقاله
                        && c.ParentId == item.Id
                         && c.Status == CommentStatus.تایید_شده)
                        
                     .Select(r => new CommentUiQueryModel
                     {

                         Id = r.Id,
                         FullName = r.FullName,
                         CreateDaate = r.CreateDate.ToPersainDate(),
                         Text = r.Text,
                         UserId = r.UserId,
                         ImageName = "",
                         Replys = new()

                     }).ToList();
                }

                if (result.Comments.Count() > 0)
                {
                    foreach (var comment in result.Comments)
                    {
                        if (comment.UserId > 0)
                        {
                            var parentUser = await _userRepository.GetByIdAsync(comment.UserId);
                            comment.ImageName = FileDirectories.UserImageDirectory100 + parentUser.Avatar;
                            if (comment.Replys.Count() > 0)
                            {
                                foreach (var reply in comment.Replys)
                                {
                                    var childUser = await _userRepository.GetByIdAsync(reply.UserId);
                                    reply.ImageName = FileDirectories.UserImageDirectory100 + childUser.Avatar;
                                }

                            }
                        }
                    }
                }


            }

            return result;
        }
    }
}

