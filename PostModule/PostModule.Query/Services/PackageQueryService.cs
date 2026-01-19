using Microsoft.EntityFrameworkCore;
using PostModule.Application.Contract.UserPostApplication.Query;
using PostModule.Domain.UserPostAgg;
using Shared.Application;

namespace PostModule.Query.Services
{
    public class PackageQueryService : IPostPackageQueryServicd
    {
        private readonly IPackageRepository _packageRepository;

        public PackageQueryService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<List<PackageAdminQueryModel>> GetAll()
        {
            IQueryable<Package> model = _packageRepository.GetAll();
            return await model.Select(p => new PackageAdminQueryModel(p.Id, p.Title, p.Count, p.Price,
                p.CreateDate.ToPersainDate(), p.UpdateDate.ToPersainDate(), p.Active, FileDirectories.PackageImageDirectory100 + p.ImageName))
            .ToListAsync();
        }
    }
}