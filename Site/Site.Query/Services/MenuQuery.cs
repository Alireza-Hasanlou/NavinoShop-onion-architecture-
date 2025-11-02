using Microsoft.EntityFrameworkCore;
using Shared.Application;
using Shared.Domain.Enums;
using Site.Application.Contract.MenuService.Query;
using Site.Domain.MenuAgg;
using System;
using System.Threading.Tasks;

namespace Site.Query.Services;

internal class MenuQuery : IMenuQueryService
{
    private readonly IMenuRepository _menuRepository;

    public MenuQuery(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<MenuPageAdminQueryModel> GetForAdminAsync(int parentId)
    {
        MenuPageAdminQueryModel model = new()
        {
            Id = parentId
        };
        if (parentId == 0)
        {
            model.PageTitle = "لیست منو های سردسته";

            model.Menus = await _menuRepository.GetAllBy(m => m.ParentId == null).Select(m => new MenuForAdminQueryModel
            {
                Active = m.Active,
                CreationDate = m.CreateDate.ToPersainDate(),
                Id = m.Id,
                Number = m.Number,
                Status = m.Status,
                Title = m.Title,
                Url = m.Url,
                ImageName = m.ImageName
            }).ToListAsync();

        }
        else
        {
            var menuParent = await _menuRepository.GetByIdAsync(parentId);
            model.PageTitle = $"لیست زیر منو های {menuParent.Title} - وضعیت {menuParent.Status.ToString().Replace("_", " ")}";
            model.Status = menuParent.Status;

            model.Menus = await _menuRepository.GetAllBy(m => m.ParentId == parentId).Select(m => new MenuForAdminQueryModel
            {
                Active = m.Active,
                CreationDate = m.CreateDate.ToPersainDate(),
                Id = m.Id,
                Number = m.Number,
                Status = m.Status,
                Title = m.Title,
                Url = m.Url,
                ImageName = m.ImageName
            }).ToListAsync();

        }
        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetForBlogAsync()
    {
        List<MenuForUiQueryModel> model = new();
        var menus = _menuRepository.GetAllBy(b => b.Active &&
        (b.Status == MenuStatus.منوی_وبلاگ_لینک
        || b.Status == MenuStatus.منوی_وبلاگ_با_زیرمنوی_بدون_عکس
        || b.Status == MenuStatus.منوی_وبلاگ_با_زیر_منوی_عکس_دار));
        foreach (var item in menus)
        {
            MenuForUiQueryModel menu = new()
            {
                Number = item.Number,
                Title = item.Title,
                Url = item.Url,
                Status = item.Status,
                Childs = new()
            };
            if (await _menuRepository.ExistByAsync(m => m.ParentId == item.Id && m.Active))
            {

                menu.Childs = await _menuRepository.GetAllBy(m => m.Active && m.ParentId == item.Id).Select(m => new MenuForUiQueryModel
                {
                    ImageAlt = m.ImageAlt,
                    Childs = new(),
                    ImageName = FileDirectories.MenuImageDirectory + m.ImageName,
                    Number = m.Number,
                    Title = m.Title,
                    Url = m.Url,
                    Status = m.Status
                }).ToListAsync();

            }


            model.Add(menu);
        }
        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetForFooterAsync()
    {
        List<MenuForUiQueryModel> model = new();
        var menus = _menuRepository.GetAllBy(b => b.Active &&
        (b.Status == MenuStatus.تیتر_منوی_فوتر));
        foreach (var item in menus)
        {
            MenuForUiQueryModel menu = new()
            {
                Number = item.Number,
                Title = item.Title,
                Url = item.Url,
                Status = item.Status,
                Childs = new()
            };
            if (await _menuRepository.ExistByAsync(m => m.ParentId == item.Id && m.Active))
            {

                menu.Childs = await _menuRepository.GetAllBy(m => m.Active && m.ParentId == item.Id).Select(m => new MenuForUiQueryModel
                {
                    Number = m.Number,
                    Title = m.Title,
                    Url = m.Url,
                    Status = m.Status
                }).ToListAsync();


            }


            model.Add(menu);
        }
        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetForIndexAsync()
    {
        List<MenuForUiQueryModel> model = new();
        var menus = _menuRepository.GetAllBy(b => b.Active &&
        (b.Status == MenuStatus.منوی_اصلی
        || b.Status == MenuStatus.منوی_اصلی_با_زیر_منو
        ));
        foreach (var item in menus)
        {
            MenuForUiQueryModel menu = new()
            {
                Number = item.Number,
                Title = item.Title,
                Url = item.Url,
                ImageAlt = item.ImageAlt,
                ImageName = string.IsNullOrEmpty(item.ImageName) ? "" : FileDirectories.MenuImageDirectory + item.ImageName,
                Childs = new(),
                Status = item.Status
            };
            if (await _menuRepository.ExistByAsync(m => m.ParentId == item.Id && m.Active))
            {

                menu.Childs = await _menuRepository.GetAllBy(m => m.Active && m.ParentId == item.Id).Select(m => new MenuForUiQueryModel
                {
                    Id = m.Id,
                    Childs = new(),
                    Number = m.Number,
                    Title = m.Title,
                    Url = m.Url,
                    Status = m.Status
                }).ToListAsync();

            }


            model.Add(menu);
        }
        foreach (var item in model.Where(w => w.Status == MenuStatus.منوی_اصلی_با_زیر_منو && w.Childs.Count() > 0))
        {
            foreach (var sub in item.Childs)
            {

                sub.Childs = await _menuRepository.GetAllBy(m => m.Active && m.ParentId == sub.Id).Select(m => new MenuForUiQueryModel
                {
                    Childs = new(),
                    Number = m.Number,
                    Title = m.Title,
                    Url = m.Url,
                    Status = m.Status
                }).ToListAsync();

            }
        }
        return model;
    }
}
