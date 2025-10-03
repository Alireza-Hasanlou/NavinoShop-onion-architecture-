using Shared.Domain;
using Site.Application.Contract.BanerService.Command;

namespace Site.Domain.BanerAgg
{
    public interface IBanerRepository : IGenericRepository<Baner, int>
    {
       Task< EditBanerCommandModel> GetForEdit(int id);
    }
}
