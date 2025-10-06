using Shared.Domain;
using PostModule.Application.Contract.PostApplication;
using PostModule.Domain.PostEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostModule.Application.Contract.PostCalculate;

namespace PostModule.Domain.Services
{
    public interface IPostRepository : IGenericRepository<Post, int>
    {
        Task<List<PostPriceResponseModel>> CalculatePostAsync(PostPriceRequestModel command);
        Task<EditPost> GetForEditAsync(int id);
    }
}
