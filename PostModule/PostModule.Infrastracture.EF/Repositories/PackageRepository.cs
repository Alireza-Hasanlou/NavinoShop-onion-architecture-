using PostModule.Domain.UserPostAgg;
using PostModule.Application.Contract.UserPostApplication.Command;
using PostModule.Infrastracture.Context;
using Shared.Insfrastructure;
using Microsoft.EntityFrameworkCore;

namespace PostModule.Infrastracture.Repositories;

internal class PackageRepository : GenericRepository<Package, int>, IPackageRepository
{
    private readonly PostContext _context;
    public PackageRepository(PostContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CreatePostOrder> GetCreatePostModelAsync(int userId, int packageId)
    {
        var package = await _context.Packages.FindAsync(packageId);
        return new CreatePostOrder(userId,package.Id,package.Price);
    }

    public async Task<EditPackage> GetForEdit(int id)
    {
        return await _context.Packages.Select(p => new EditPackage
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Count = p.Count,
            Price = p.Price,
            ImageAlt = p.ImageAlt,
            ImageFile = null,
            ImageName = p.ImageName
        }).SingleOrDefaultAsync(p => p.Id == id);
    }
}
