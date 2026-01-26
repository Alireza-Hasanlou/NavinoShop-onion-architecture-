using Microsoft.EntityFrameworkCore;
using Site.Application.Contract.SiteSettingService.Command;
using Site.Domain.SiteSettingAgg;
using Site.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Infrastructure.Persistence.Repository
{
    internal class SiteSettingRepository : ISiteSettingRepository
    {
        private readonly SiteContext _context;

        public SiteSettingRepository(SiteContext context)
        {
            _context = context;
        }

        public async Task<UpsertSiteSetting> GetForUpsert()
        {
            var site = await GetSingle();
            return new()
            {
                AboutDescription = site.AboutDescription,
                AboutTitle = site.AboutTitle,
                Address = site.Address,
                Android = site.Android,
                LogoAlt = site.LogoAlt,
                WhatsApp = site.WhatsApp,
                ContactDescription = site.ContactDescription,
                Email1 = site.Email1,
                Email2 = site.Email2,
                Enamad = site.Enamad,
                FavIcon = site.FavIcon,
                FavIconFile = null,
                FooterDescription = site.FooterDescription,
                FooterTitle = site.FooterTitle,
                Instagram = site.Instagram,
                IOS = site.IOS,
                LogoFile = null,
                LogoName = site.LogoName,
                Phone1 = site.Phone1,
                Phone2 = site.Phone2,
                SamanDehi = site.SamanDehi,
                SeoBox = site.SeoBox,
                Telegram = site.Telegram,
                Youtube = site.Youtube,
                AboutUsImageName=site.AboutImageName
            };
        }

        public async Task< SiteSetting> GetSingle()
        {
            SiteSetting site = await _context.SiteSettings.SingleOrDefaultAsync();
            if(site == null)
            {
                site = new();
                _context.SiteSettings.Add(site);
               await SaveAsync();
            }
            return site;    
        }
        public async Task<bool>  SaveAsync() =>
         await   _context.SaveChangesAsync() >= 0 ? true : false;
    }
}
