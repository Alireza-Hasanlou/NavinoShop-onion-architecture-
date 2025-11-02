
using Comments.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Comment;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Comment
{
    public class CommentQueryService : ICommentAdminQuery
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentQueryService(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<CommentForAdminPaging> GetForAdmin(int pageId, int take, string filter, int ownerId,
            CommentFor commentFor, CommentStatus commentStatus, int? parentId)
        {


            var comments = _commentRepository.GetAllBy(q => q.OwnerId == ownerId || q.CommentFor == commentFor || q.Status == commentStatus || q.ParentId == parentId);

            if (!string.IsNullOrEmpty(filter))
                comments.Where(f => f.FullName.Contains(filter) || f.Text.Contains(filter) || f.Email.Contains(filter));

            CommentForAdminPaging model = new CommentForAdminPaging();
            model.GetData(comments, pageId, take, 5);
            model.Filter = filter;
            model.OwnerId = ownerId;
            model.For = commentFor;
            model.CommentStatus = commentStatus;
            model.OwnerId = ownerId;
            model.PageTitle = $"لیست نظرات _ {commentStatus.ToString().Replace("_", "")} _ {commentFor.ToString().Replace("_", "")}";
            model.ParentId = parentId;
            model.Comments = new();
            if (comments.Any())
            {
                model.Comments = await comments.Skip(model.Skip).Take(model.Take).Select(c => new CommentAdminQueryModel
                {
                    CommentStatus = c.Status,
                    ParentId = c.ParentId,
                    Email = c.Email,
                    FullName = c.FullName,
                    For = c.CommentFor,
                    Text = c.Text,
                    OwnerId = c.OwnerId,
                    UserId = c.UserId,
                    WhyRejected = c.WhyRejected,
                    UserName = "",
                    CommentTitle = "",
                    Id = c.Id
                }).OrderByDescending(c => c.Id).ToListAsync();
            }
            model.Comments.ForEach(async x =>
            {
                x.HaveChild = await _commentRepository.ExistByAsync(c => c.ParentId == x.Id);
                if (x.UserId > 0)
                {
                    var user = await _userRepository.GetByIdAsync(x.UserId);
                    x.UserName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : user.Mobile;
                }


            });


            return model;

        }
    }
}
