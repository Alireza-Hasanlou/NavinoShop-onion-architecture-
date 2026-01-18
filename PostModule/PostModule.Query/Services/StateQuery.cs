using Microsoft.EntityFrameworkCore;
using PostModule.Application.Contract.StateQuery;
using PostModule.Infrastracture.Context;
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Query.Services
{
    internal class StateQuery : IStateQueryService
    {
        private readonly PostContext _post_Context;
        public StateQuery(PostContext post_Context)
        {
            _post_Context = post_Context;
        }

        public async Task<List<CityForChooseQueryModel>> GetCitiesForChoose(int stateId) =>
          await _post_Context.Cities.Where(c => c.StateId == stateId)
            .Select(c => new CityForChooseQueryModel()
            {
                CityCode = c.Id,
                Title = c.Title
            }).ToListAsync();

        public async Task<StateDetailQueryModel> GetStateDetail(int id)
        {
            var state = await _post_Context.States.FindAsync(id);
            StateDetailQueryModel model = new()
            {
                Name = state.Title,
                Id = state.Id,
                CloseStates = state.CloseStates,
                States = new(),
                Cities = new(),
            };
            model.States = await _post_Context.States.Select(s => new StateForAddStateClosesQueryModel
            {
                Id = s.Id,
                title = s.Title
            }).ToListAsync();
            model.Cities = await _post_Context.Cities.Where(c => c.StateId == state.Id)
                .Select(c => new CityAdminQueryModel
                {
                    CreationDate = c.CreateDate.ToPersainDate(),
                    Id = c.Id,
                    Status = c.Status,
                    Title = c.Title
                }).ToListAsync();
            return model;
        }

        public async Task<List<StateAdminQueryModel>> GetStatesForAdmin() =>
        await _post_Context.States.Include(s => s.Cities).Select(s => new StateAdminQueryModel
        {
            Id = s.Id,
            Title = s.Title,
            CreateDate = s.CreateDate.ToPersainDate(),
            CityCount = s.Cities.Count()
        }).ToListAsync();

        public async Task<List<StateForChooseQueryModel>> GetStatesForChoose()
        {
            return await _post_Context.States.Select(s => new StateForChooseQueryModel
            {
                Id = s.Id,
                Title = s.Title
            }).ToListAsync();
        }

        public async Task<List<StateQueryModel>> GetStatesWithCity() =>
           await _post_Context.States.Include(s => s.Cities).Select(s => new StateQueryModel
           {
               Name = s.Title,
               Cities = s.Cities.Select(c => new CityQueryModel
               {
                   CityCode = c.Id,
                   Name = c.Title,
               }).ToList()
           }).ToListAsync();

        public async Task<string> GetStateTitle(int id)
        {
            var state = await _post_Context.States.FindAsync(id);
            return state.Title;
        }

        public async Task<bool> IsCityCorrect(int stateId, int cityId) =>
           await _post_Context.Cities.AnyAsync(c => c.Id == cityId && c.StateId == stateId);

        public async Task<bool> IsStateCorrect(int stateId) =>
           await _post_Context.States.AnyAsync(s => s.Id == stateId);
    }
}
