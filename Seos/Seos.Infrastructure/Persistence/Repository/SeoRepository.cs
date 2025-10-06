using Seos.Application.Contract;
using Seos.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Seos.Infrastructure.Context;
using Seos.Domain.SeoAgg;
using Shared.Insfrastructure;
using Seos.Application.Contract.SeoService.Command;
using System.Threading.Tasks;
using Shared.Domain.Enums;

namespace Seos.Infrastructure.Persistence.Repository
{
    internal class SeoRepository : GenericRepository<Seo,int> , ISeoRepository
    {
        private readonly Seo_Context _context;
        public SeoRepository(Seo_Context context): base(context)
        {
            _context = context;
        }
       
        public Seo GetSeo(int ownerId, WhereSeo where)
        {
            return _context.Seos.SingleOrDefault(s => s.OwnerId == ownerId && s.Where == where);
        }

        public CreateSeoCommandModel GetSeoForUbsert(int ownerId, WhereSeo where)
        {
            var seo = _context.Seos.SingleOrDefault(s => s.OwnerId == ownerId && s.Where == where);
            if (seo == null)
                return new()
                {
                    OwnerId = ownerId,
                    Where = where
                };

            return new()
            {
                OwnerId = seo.OwnerId,
                Canonical = seo.Canonical,
                IndexPage = seo.IndexPage,
                MetaDescription = seo.MetaDescription,
                MetaKeyWords = seo.MetaKeyWords,
                MetaTitle = seo.MetaTitle,
                Schema = seo.Schema,
                Where = seo.Where
            };
        }

        public async Task<Seo> GetSeoForUi(int ownerId, WhereSeo where, string title)
        {
            var seo = GetSeo(ownerId, where);
            if(seo == null)
            {
                seo = new Seo(title, "", "", true, "","", where, ownerId);
               await CreateAsync(seo);
            }
            return seo;
        }
    }
}
