using Shared.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query.Contract.Admin.Seo
{
    public interface ISeoAdminQuery
    {
        Task<string> GetAdminSeoTitle(WhereSeo where, int ownerId);
    }
}
