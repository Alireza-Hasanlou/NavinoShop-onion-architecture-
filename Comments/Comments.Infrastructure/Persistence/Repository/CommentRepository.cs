using Comments.Domain.CommentAgg;
using Comments.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Insfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Infrastructure.Persistence.Repository
{
    internal class CommentRepository : GenericRepository<Comment, long>, ICommentRepository
    {
        private readonly CommentContext _context;
        public CommentRepository(CommentContext context) : base(context)
        {
            _context = context;
        }
    }
}
