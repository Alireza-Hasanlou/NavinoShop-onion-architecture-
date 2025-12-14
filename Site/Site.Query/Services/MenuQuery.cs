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

        return model;
    }

    public async Task<List<MenuForUiQueryModel>> GetProductCategoriesForIndexAsync()
    {
        List<MenuForUiQueryModel> model = new();
        var productCategories = _menuRepository.GetAllBy(m => m.Status == MenuStatus.منوی_گروه_محصولات && m.Active == true);
        if (productCategories == null)
            return new();

        foreach (var item in productCategories)
        {
            var menu = new MenuForUiQueryModel
            {
                Id = item.Id,
                Status = item.Status,
                Title = item.Title,
                Url = item.Url,
                ImageAlt = item.ImageAlt,
                ImageName = item.ImageName,
                Number = item.Number,
                Childs = new()
            };

            if (await _menuRepository.ExistByAsync(p => p.ParentId == item.Id))
            {
                var sub1productCategories = _menuRepository.GetAllBy(m => m.ParentId == item.Id && m.Active == true);
                foreach (var sub1 in sub1productCategories)
                {
                    var menu1 = new MenuForUiQueryModel
                    {
                        Id = sub1.Id,
                        Status = sub1.Status,
                        Title = sub1.Title,
                        Url = sub1.Url,
                        ImageAlt = sub1.ImageAlt,
                        ImageName = sub1.ImageName,
                        Number = sub1.Number,
                        Childs = new()
                    };

                    menu.Childs.Add(menu1);
                    if (await _menuRepository.ExistByAsync(p => p.ParentId == sub1.Id && p.Active == true))
                    {

                        var sub2productCategories = _menuRepository.GetAllBy(m => m.ParentId == sub1.Id && m.Active == true);
                        foreach (var sub2 in sub1productCategories)
                        {
                            var menu2 = new MenuForUiQueryModel
                            {
                                Id = sub1.Id,
                                Status = sub1.Status,
                                Title = sub1.Title,
                                Url = sub1.Url,
                                ImageAlt = sub1.ImageAlt,
                                ImageName = sub1.ImageName,
                                Number = sub1.Number,
                                Childs = new()
                            };

                            menu1.Childs.Add(menu2);

                            if (await _menuRepository.ExistByAsync(p => p.ParentId == sub2.Id && p.Active == true))
                            {

                                menu2.Childs = await _menuRepository.GetAllBy(p => p.ParentId == sub2.Id && p.Active == true).Select(sub3 => new MenuForUiQueryModel
                                {
                                    Id = sub3.Id,
                                    Status = sub3.Status,
                                    Title = sub3.Title,
                                    Url = sub3.Url,
                                    ImageAlt = sub3.ImageAlt,
                                    ImageName = sub3.ImageName,
                                    Number = sub3.Number,
                                    Childs = new()
                                }).ToListAsync();

                                model.Add(menu);
                            }
                        }
                    }




                }
            }

        }
        return model;
    }
}
