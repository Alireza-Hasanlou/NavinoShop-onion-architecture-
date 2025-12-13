using PostModule.Application.Contract.PostPriceApplication;
using PostModule.Domain.PostEntity;
using PostModule.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostModule.Infrastracture.Context;
using Shared.Insfrastructure;
using Microsoft.EntityFrameworkCore;

namespace PostModule.Infrastracture.Repositories
{
    internal class PostPriceRepository : GenericRepository<PostPrice, int>, IPostPriceRepository
    {
        private readonly PostContext _context;
        public PostPriceRepository(PostContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EditPostPrice> GetForEdit(int id)
        {
            return await _context.PostPrices.Select(p => new EditPostPrice
            {
                PostId = p.PostId,
                CityPrice = p.CityPrice,
                End = p.End,
                Id = p.Id,
                InsideStatePrice = p.InsideStatePrice,
                Start = p.Start,
                StateCenterPrice = p.StateCenterPrice,
                StateClosePrice = p.StateClosePrice,
                StateNonClosePrice = p.StateNonClosePrice,
                TehranPrice = p.TehranPrice
            }).SingleOrDefaultAsync(p => p.Id == id);
        }
    }
}
