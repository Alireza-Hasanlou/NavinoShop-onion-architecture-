
using Microsoft.EntityFrameworkCore;
using PostModule.Application.Contract.StateApplication;
using PostModule.Domain.Services;
using PostModule.Domain.StateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PostModule.Infrastracture.Context;
using Shared.Insfrastructure;

namespace PostModule.Infrastracture.Repositories;

internal class StateRepository : GenericRepository<State, int>, IStateRepository
{
    private readonly PostContext _context;
    public StateRepository(PostContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<StateViewModel>> GetAllStateViewModel()
    {
        return await GetAll().Select(s => new StateViewModel { 
            CreateDate=s.CreateDate.ToString(),
            Id=s.Id,
            Title=s.Title

        }).ToListAsync();

    }

    public async Task<EditStateModel> GetStateForEdit(int id)
    {
        var state = await GetByIdAsync(id);
        return new()
        {
            Id=state.Id,
            Title=state.Title
        };
    }

    public async Task<string> GetStateTitle(int stateId)
    {
        return await _context.States.Where(i => i.Id == stateId).Select(t => t.Title).SingleAsync();
    }
}
