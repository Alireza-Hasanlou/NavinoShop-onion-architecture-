using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostModule.Application.Contract.StateQuery;
using Shared.Application.Auth;
using System.Net;
using Users.Application.Contract.UserAddressService.Command;
using Users.Application.Contract.UserAddressService.Query;

namespace NavinoShop.WebApplication.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    [Route("/Profile/[action]/{id?}")]
    public class UserAddressController : Controller
    {
        private readonly IStateQueryService _stateQueryService;
        private readonly IUserAddressCommandService _userAddressCommandService;
        private readonly IAuthService _authService;

        public UserAddressController(IStateQueryService stateQueryService,
            IUserAddressCommandService userAddressCommandService, IAuthService authService)
        {
            _stateQueryService = stateQueryService;
            _userAddressCommandService = userAddressCommandService;
            _authService = authService;
        }

        public async Task<IActionResult> NewAddress(bool Status = false)
        {
            //Edit OR Create Status
            if (Status == true)
            {
                ViewData["success"] = "عملیات با موفقیت انجام شد";
            }
            CreateUserAddressCommand newAddress = new();
            ViewData["States"] = await _stateQueryService.GetStatesForChoose();
            return View(newAddress);
        }
        [HttpPost]
        public async Task<IActionResult> NewAddress(CreateUserAddressCommand command)
        {
            ViewData["States"] = await _stateQueryService.GetStatesForChoose();
            if (command.StateId < 1)
            {
                ModelState.AddModelError("StateId", "لطفا استان محل سکونت خود رو انتخاب کنید");
                return View();
            }
            if (command.CityId < 1)
            {
                ModelState.AddModelError("StateId", "لطفا شهر محل سکونت خود رو انتخاب کنید");
                return View();
            }
            if (!ModelState.IsValid)
                return View();
            var userId = _authService.GetLoginUserId();
            var res = await _userAddressCommandService.CreateAsync(command, userId);
            if (res.Success)
            {
                ViewData["success"] = "آدرس جدید با موفقیت اضافه شد";
                return RedirectToAction("NewAddress", new { Status = true });
            }
            ModelState.AddModelError(res.ModelName, res.Message);
            return View();
        }
        public async Task<JsonResult> DeleteAddress(int id)
        {
            if (id < 1)
                return new JsonResult(new { success = false, title = "خطا هنگام حذف آدرس" });
            var res = await _userAddressCommandService.DeleteAsync(id);
            if (res.Success)
                return new JsonResult(new { success = true, title = "آدرس مورد نظر با موفقیت حذف شد" });

            return new JsonResult(new { success = false, title = res.Message });

        }
        public async Task<IActionResult> EditAddress(int id)
        {


            if (id < 1)
                return RedirectToAction("NewAddress");
            var address = await _userAddressCommandService.GetAddressForEditAsync(id);
            if (address == null)
                return RedirectToAction("NewAddress");
            ViewData["States"] = await _stateQueryService.GetStatesForChoose();
            ViewData["Cities"] = await _stateQueryService.GetCitiesForChoose(address.StateId);
            return View(address);
        }
        [HttpPost]
        public async Task<IActionResult> EditAddress(UserAddressDto command)
        {
            if (command.StateId < 1)
            {
                ModelState.AddModelError("StateId", "لطفا استان محل سکونت خود رو انتخاب کنید");
                return View();
            }
            ViewData["States"] = await _stateQueryService.GetStatesForChoose();
            ViewData["Cities"] = await _stateQueryService.GetCitiesForChoose(command.StateId);
            if (!ModelState.IsValid)
                return View();
            var res = await _userAddressCommandService.EditAsync(command);
            if (res.Success)
                return RedirectToAction("NewAddress", new { Status = true });

            ModelState.AddModelError("Email", res.Message);
            return View();
        }
        public async Task<JsonResult> GetCitiesForState(int StateId)
        {
            var cities = await _stateQueryService.GetCitiesForChoose(StateId);
            return Json(cities);
        }
    }
}
