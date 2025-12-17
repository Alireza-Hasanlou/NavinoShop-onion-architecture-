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
            ParentId = parentId
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
                ImageName = FileDirectories.MenuImageDirectory100 + m.ImageName
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
                ImageName = FileDirectories.MenuImageDirectory100 + m.ImageName
            }).ToListAsync();

        }
        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetForBlogAsync()
    {
        List<MenuForUiQueryModel> model = new();
        var menus = _menuRepository.GetAllBy(b => b.Active &&
        (b.Status == MenuStatus.منوی_وبلاگ_لینک));
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
        var menus = await _menuRepository.GetAllBy(b => b.Active &&
        (b.Status == MenuStatus.تیتر_منوی_فوتر)).ToListAsync();
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

            menu.Childs = await _menuRepository.GetAllBy(m => m.Active && m.ParentId == item.Id).Select(m => new MenuForUiQueryModel
            {
                Number = m.Number,
                Title = m.Title,
                Url = m.Url,
                Status = m.Status
            }).OrderBy(n => n.Number).ToListAsync();




            model.Add(menu);
        }
        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetForIndexAsync()
    {
        var model = new List<MenuForUiQueryModel>();

        var menus = await _menuRepository
            .GetAllBy(m =>
                m.Active &&
                (m.Status == MenuStatus.منوی_اصلی ||
                 m.Status == MenuStatus.منوی_اصلی_با_زیر_منو))
            .OrderBy(m => m.Number)
            .ToListAsync();

        foreach (var item in menus)
        {
            var menu = new MenuForUiQueryModel
            {
                Id = item.Id,
                Number = item.Number,
                Title = item.Title,
                Url = item.Url,
                Status = item.Status,
                ImageAlt = item.ImageAlt,
                ImageName = string.IsNullOrEmpty(item.ImageName)
                    ? null
                    : FileDirectories.MenuImageDirectory + item.ImageName,
                Childs = new List<MenuForUiQueryModel>()
            };


            if (item.Status == MenuStatus.منوی_اصلی_با_زیر_منو)
            {
                menu.Childs = await _menuRepository
                    .GetAllBy(m => m.Active && m.ParentId == item.Id)
                    .OrderBy(m => m.Number)
                    .Select(m => new MenuForUiQueryModel
                    {
                        Id = m.Id,
                        Number = m.Number,
                        Title = m.Title,
                        Url = m.Url,
                        Status = m.Status,
                        Childs = new List<MenuForUiQueryModel>()
                    })
                    .ToListAsync();
            }

            model.Add(menu);
        }

        return model;
    }


    public async Task<List<MenuForUiQueryModel>> GetProductCategoriesForIndexAsync()
    {
        var model = new List<MenuForUiQueryModel>();

        var productCategories = await _menuRepository
            .GetAllBy(m => m.Status == MenuStatus.منوی_گروه_محصولات && m.Active)
            .ToListAsync();

        foreach (var item in productCategories)
        {
            var menu = new MenuForUiQueryModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title,
                Url = item.Url,
                ImageAlt = item.ImageAlt,
                ImageName = FileDirectories.MenuImageDirectory + item.ImageName,
                Number = item.Number,
                Childs = new()
            };

            var level1 = await _menuRepository
                .GetAllBy(m => m.ParentId == item.Id && m.Active).OrderBy(i => i.Number)
                .ToListAsync();

            foreach (var sub1 in level1)
            {
                var menu1 = new MenuForUiQueryModel
                {
                    Id = sub1.Id,
                    Status = sub1.Status,
                    Title = sub1.Title,
                    Url = sub1.Url,
                    ImageAlt = sub1.ImageAlt,
                    ImageName = FileDirectories.MenuImageDirectory + sub1.ImageName,
                    Number = sub1.Number,
                    Childs = new()
                };

                var level2 = await _menuRepository
                    .GetAllBy(m => m.ParentId == sub1.Id && m.Active).OrderBy(i => i.Number)
                    .ToListAsync();

                foreach (var sub2 in level2)
                {
                    var menu2 = new MenuForUiQueryModel
                    {
                        Id = sub2.Id,
                        Status = sub2.Status,
                        Title = sub2.Title,
                        Url = sub2.Url,
                        ImageAlt = sub2.ImageAlt,
                        ImageName = FileDirectories.MenuImageDirectory + sub2.ImageName,
                        Number = sub2.Number,
                        Childs = await _menuRepository
                            .GetAllBy(m => m.ParentId == sub2.Id && m.Active).OrderBy(i => i.Number)
                            .Select(sub3 => new MenuForUiQueryModel
                            {
                                Id = sub3.Id,
                                Status = sub3.Status,
                                Title = sub3.Title,
                                Url = sub3.Url,
                                ImageAlt = sub3.ImageAlt,
                                ImageName = sub3.ImageName,
                                Number = sub3.Number,
                                Childs = new()
                            }).ToListAsync()
                    };

                    menu1.Childs.Add(menu2);
                }

                menu.Childs.Add(menu1);
            }

            model.Add(menu);
        }

        return model;
    }

}
