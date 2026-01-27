using Microsoft.EntityFrameworkCore;
using PostModule.Domain.UserPostAgg;
using Query.Contract.UI.UserPanel.PostOrder;
using Shared.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Service.Ui.UserPanel.PostOrder
{
    internal class PostOrderQueryService : IPostOrderQueryService
    {
        private readonly IPOstOrderRepository _postOrderRepository;

        public PostOrderQueryService(IPOstOrderRepository postOrderRepository)
        {
            _postOrderRepository = postOrderRepository;
        }

        public async Task<List<PostOrderUserPanelQueryModel>> GetPostOrderForUserPanelAsync(int userId)
        {
            return await _postOrderRepository.GetAllBy(i => i.UserId == userId)
                .Include(p => p.Package)
                .Select(x => new PostOrderUserPanelQueryModel()
                {
                    Id = x.Id,
                    PackagePrice = x.Price,
                    PackageId = x.PackageId,
                    Date = x.CreateDate.ToPersainDate(),
                    TransactionId = x.TransactionId,
                    postOrderStatus = x.Status,
                    packageTitle = x.Package.Title,
                    ImageName = FileDirectories.PackageImageDirectory + x.Package.ImageName,
                    Count = x.Package.Count,
                    TransactionRef = ""
                    //Set TransacionRef
                }).ToListAsync();
        }
    }
}
