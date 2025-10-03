
using Comments.Application.Contract.CommentService.Command;
using Comments.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Shared.Application;
using Shared.Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Application.Services
{
    internal class CommentService : ICommentCommandService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<OperationResult> AcceptedComment(long id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            comment.AcceptedComment();
            if (await _commentRepository.SaveAsync())
                return new(true);
            return new(false);
        }

        public async Task<OperationResult> Create(CreateCommentCommandModel command)
        {
            Comment comment = new(command.UserId, command.OwnerId, command.For,
                command.FullName, command.Email, command.Text, command.ParentId);
            var result= await _commentRepository.CreateAsync(comment);
            if (result.Success) return new(true);
            return new(false,ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> Reject(RejectCommentCommandModel command)
        {
            var comment = await _commentRepository.GetByIdAsync(command.Id);
            comment.RejectedComment(command.Why);
            if (await _commentRepository.SaveAsync())
                return new(true);
            return new(false,ValidationMessages.SystemErrorMessage);
        }
    }
}
