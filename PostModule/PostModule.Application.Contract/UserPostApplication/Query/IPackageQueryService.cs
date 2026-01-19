using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostModule.Application.Contract.UserPostApplication.Query
{
    public interface IPostPackageQueryServicd
    {
       Task< List<PackageAdminQueryModel>> GetAll();
    }
}
