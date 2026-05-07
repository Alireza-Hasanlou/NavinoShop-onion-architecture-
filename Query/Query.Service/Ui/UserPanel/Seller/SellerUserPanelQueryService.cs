using Microsoft.EntityFrameworkCore;
using PostModule.Domain.Services;
using Query.Contract.UI.UserPanel.Seller;
using Shared.Application;
using Shop.Domain.SellerAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.User.Agg.IRepository;

namespace Query.Service.Ui.UserPanel.Seller
{
    internal class SellerUserPanelQueryService : ISellerUserPanelQueries
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;

        public SellerUserPanelQueryService(ISellerRepository sellerRepository, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _sellerRepository = sellerRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }

     

        public async Task<List<SellerUserPanelQueryModel>> GetSellersForUserPanel(int UserId)
        {
            var Sellers = await _sellerRepository.GetAllBy(i => i.UserId == UserId)
                .OrderByDescending(c=>c.CreateDate)
                .Select(s => new SellerUserPanelQueryModel
                {
                 
                    Title = s.Title,
                    Id = s.Id,
                    CityId = s.CityId,
                    ImageName = FileDirectories.SellerImageDirectory100 + s.ImageName,
                    Phone = s.Phone1,
                    CityName = "",
                    whyRejected=s.WhyRejected,
                    StateId = s.StateId,
                    CreateDate = s.CreateDate.ToPersainDate(),
                    SellerStatus = s.Status,

                }).ToListAsync();

            foreach (var item in Sellers)
            {

                var state = await _stateRepository.GetByIdAsync(item.StateId);
                var city = await _cityRepository.GetByIdAsync(item.CityId);
                item.CityName = $"{state.Title}_{city.Title}";

            }
            return Sellers;
        }
    }
}
