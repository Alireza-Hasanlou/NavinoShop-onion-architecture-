
using Seos.Application.Contract;
using Seos.Application.Contract.SeoService.Command;
using Seos.Domain;
using Seos.Domain.SeoAgg;
using Shared.Application;
using Shared.Domain.Enums;
using System.Threading.Tasks;

namespace Seos.Application.Services
{
    internal class SeoService : ISeoCommandService
    {
        private readonly ISeoRepository _seoRepository;
        public SeoService(ISeoRepository seoRepository)
        {
            _seoRepository = seoRepository;
        }

        public CreateSeoCommandModel GetSeoForEdit(int ownerId, WhereSeo where)
        {
            return _seoRepository.GetSeoForUbsert(ownerId, where);
        }

        public async Task<OperationResult> UpsertSeo(CreateSeoCommandModel command)
        {
            var seo = _seoRepository.GetSeo(command.OwnerId, command.Where);
            if (seo == null)
            {
                Seo seo1 = new(command.MetaTitle, command.MetaDescription, command.MetaKeyWords, command.IndexPage,
                    command.Canonical, command.Schema, command.Where, command.OwnerId);
                var result = await _seoRepository.CreateAsync(seo1);
                if (result.Success)
                    return new(true);
                return new(false);
            }
            else
            {
                seo.Edit(command.MetaTitle, command.MetaDescription, command.MetaKeyWords, command.IndexPage,
                    command.Canonical, command.Schema);
               if( await _seoRepository.SaveAsync())
                    return new(true);
               return new(false);
            }
        }


    }
}
