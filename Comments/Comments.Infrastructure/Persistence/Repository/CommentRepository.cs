using Comments.Domain.CommentAgg;
using Comments.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Shared.Insfrastructure;

namespace Comments.Infrastructure.Persistence.Repository
{
    internal class CommentRepository : GenericRepository<Comment, long>, ICommentRepository
    {
        public CommentRepository(CommentContext context) : base(context)
        {
        }
    }
}
