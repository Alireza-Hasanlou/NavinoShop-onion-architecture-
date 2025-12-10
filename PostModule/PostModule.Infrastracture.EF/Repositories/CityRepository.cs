
using Microsoft.EntityFrameworkCore;
using PostModule.Application.Contract.CityService;
using PostModule.Domain.CityEntity;
using PostModule.Domain.Services;
using PostModule.Infrastracture.Context;
using Shared.Domain.Enums;
using Shared.Insfrastructure;
using System.Threading.Tasks;

namespace PostModule.Infrastracture.Repositories
{
    internal class CityRepository : GenericRepository<City, int>, ICityRepository
    {
        private readonly PostContext _context;
        public CityRepository(PostContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ChangeStatus(int id, CityStatus status)
        {
            var city = await GetByIdAsync(id);
            if (city == null)
                return false;

            List<City> citiesToReset = new();

            switch (status)
            {
                case CityStatus.تهران:
                    citiesToReset = await _context.Cities
                        .Where(c => c.Status == CityStatus.تهران)
                        .ToListAsync();
                    break;

                case CityStatus.مرکز_استان:
                    citiesToReset = await _context.Cities
                        .Where(c => c.Status == CityStatus.مرکز_استان && c.StateId == city.StateId)
                        .ToListAsync();
                    break;
            }

           
            city.ChangeStatus(status);

          
            foreach (var c in citiesToReset)
                c.ChangeStatus(CityStatus.شهرستان_معمولی);

            return await SaveAsync();
        }


        public async Task<List<CityViewModel>> GetAllForState(int stateId)
        {
            return await GetAllBy(c => c.StateId == stateId).Select(c => new CityViewModel
            {
                CreateDate = c.CreateDate.ToString(),
                Id = c.Id,
                Status = c.Status,
                Title = c.Title
            }).ToListAsync();
        }

        public async Task<EditCityModel> GetCityForEdit(int id)
        {
            var city = await GetByIdAsync(id);
            return new()
            {
                Id = city.Id,
                Title = city.Title,
                StateId = city.StateId,
            };
        }
    }
}
