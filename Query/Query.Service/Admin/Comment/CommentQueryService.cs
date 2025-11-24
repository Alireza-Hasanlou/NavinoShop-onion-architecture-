
using Azure;
using Comments.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using Query.Contract.Admin.Comment;
using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Users.Application.Contract.UserService.Query;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Admin.Comment
{
    public class CommentQueryService : ICommentAdminQuery
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserQueryService _userQueryService;

        public CommentQueryService(ICommentRepository commentRepository, IUserRepository userRepository, IUserQueryService userQueryService)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userQueryService = userQueryService;
        }

        public async Task<List<CommentAdminQueryModel>> GetAllUnseenCommentsForAdmin()
        {
            var unseenComments = await _commentRepository.GetAllBy(u => u.Status == CommentStatus.خوانده_نشده).Select(c => new CommentAdminQueryModel
            {
                CommentStatus = c.Status,
                ParentId = c.ParentId,
                Email = c.Email,
                FullName = c.FullName,
                For = c.CommentFor,
                Text = c.Text,
                OwnerId = c.OwnerId,
                UserId = c.UserId,
                UserName = "",
                CommentTitle = "",
                Id = c.Id
            }).OrderByDescending(c => c.Id).ToListAsync(); ;
            
            if (!unseenComments.Any())
            {
                return unseenComments;
            }


            var commentIds = unseenComments.Select(c => c.Id).ToList();
            var childCounts = await _commentRepository
                .GetAllBy(c => c.ParentId != null && commentIds.Contains(c.ParentId.Value))
                .Select(c => c.ParentId.Value)
                .Distinct()
                .ToListAsync();


            var userIds = unseenComments
                .Where(c => c.UserId > 0)
                .Select(c => c.UserId)
                .Distinct()
                .ToList();

            var users = await _userQueryService.GetUsersByIds(userIds);


            foreach (var comment in unseenComments)
            {
                comment.HaveChild = childCounts.Contains(comment.Id);

                if (comment.UserId > 0)
                {
                    var user = users.SingleOrDefault(u => u.Id == comment.UserId);
                    if (user != null)
                        comment.UserName = !string.IsNullOrEmpty(user.FullName)
                            ? user.FullName
                            : user.Mobile;
                }
            }
            return unseenComments;
        }

        public async Task<CommentForAdminPaging> GetForAdmin(int pageId, int take, string filter, int ownerId,
            CommentFor commentFor, CommentStatus commentStatus, int? parentId)
        {


            var commentQuery = _commentRepository.GetAllBy(q => q.OwnerId == ownerId || q.CommentFor == commentFor || q.Status == commentStatus || q.ParentId == parentId);

            if (!string.IsNullOrEmpty(filter))
                commentQuery.Where(f => f.FullName.Contains(filter) || f.Text.Contains(filter) || f.Email.Contains(filter));

            CommentForAdminPaging model = new CommentForAdminPaging();
            model.GetData(commentQuery, pageId, take, 5);
            model.Filter = filter;
            model.OwnerId = ownerId;
            model.For = commentFor;
            model.CommentStatus = commentStatus;
            model.OwnerId = ownerId;
            model.PageTitle = $"لیست نظرات _ {commentStatus.ToString().Replace("_", "")} _ {commentFor.ToString().Replace("_", "")}";
            model.ParentId = parentId;
            model.Comments = new();
            if (commentQuery.Any())
            {
                model.Comments = await commentQuery.Skip(model.Skip).Take(model.Take).Select(c => new CommentAdminQueryModel
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


            var commentIds = model.Comments.Select(c => c.Id).ToList();
            var childCounts = await _commentRepository
                .GetAllBy(c => c.ParentId != null && commentIds.Contains(c.ParentId.Value))
                .Select(c => c.ParentId.Value)
                .Distinct()
                .ToListAsync();


            var userIds = model.Comments
                .Where(c => c.UserId > 0)
                .Select(c => c.UserId)
                .Distinct()
                .ToList();

            var users = await _userQueryService.GetUsersByIds(userIds);


            foreach (var comment in model.Comments)
            {
                comment.HaveChild = childCounts.Contains(comment.Id);

                if (comment.UserId > 0)
                {
                    var user = users.SingleOrDefault(u => u.Id == comment.UserId);
                    if (user != null)
                        comment.UserName = !string.IsNullOrEmpty(user.FullName)
                            ? user.FullName
                            : user.Mobile;
                }
            }
            return model;

        }
    }
}
