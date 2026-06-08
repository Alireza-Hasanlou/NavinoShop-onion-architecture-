using Discount.Application.Contract.OrderDiscounts.Command;
using Discount.Domain.OrderDiscountAgg;
using Shared.Application;
using Shared.Application.Validations;
using Shared.Domain.Enums;


namespace Discount.Application.Commands
{


    internal class OrderDiscountsCommands : IOrderDiscountsCommands
    {
        private readonly IOrderDiscountRepository _orderDiscountRepository;

        public OrderDiscountsCommands(IOrderDiscountRepository orderDiscountRepository)
        {
            _orderDiscountRepository = orderDiscountRepository;
        }

        public async Task<OperationResult> CreateOrderDiscountAsync(UpsertOrderDiscountCommandModel commandModel)
        {
            try
            {
                if (commandModel == null)
                    return new OperationResult(false, "داده‌های تخفیف معتبر نیست");

                if (string.IsNullOrWhiteSpace(commandModel.Title))
                    return new OperationResult(false, "عنوان تخفیف الزامی است");

                if (commandModel.Percent < 0 || commandModel.Percent > 100)
                    return new OperationResult(false, "درصد تخفیف باید بین 0 تا 100 باشد");

                if (commandModel.Count <= 0)
                    return new OperationResult(false, "تعداد تخفیف باید بیشتر از صفر باشد");

                if (string.IsNullOrWhiteSpace(commandModel.Code))
                    return new OperationResult(false, "کد تخفیف الزامی است");

                if (commandModel.type == OrderDiscountType.Order && commandModel.ShopId != 0)
                {
                    commandModel.ShopId = 0;
                }
                 

                bool existingByCode =
                    await _orderDiscountRepository.IsExistByCodeAsync(
                        commandModel.Code, commandModel.ShopId); 


                if (existingByCode)
                    return new OperationResult(false, "کد تخفیف تکراری است");

                var discount = new OrderDiscount(
                    commandModel.Percent,
                    commandModel.Title,
                    commandModel.Code,
                    commandModel.Count,
                    commandModel.ShopId, // ShopId
                    commandModel.StartDate.ToEnglishDateTime(),
                    commandModel.EndDate.ToEnglishDateTime(),
                    commandModel.type
                );

                var result = await _orderDiscountRepository.CreateAsync(discount);

                return result.Success
                    ? new OperationResult(true, "تخفیف با موفقیت ایجاد شد")
                    : new OperationResult(false, "خطا در ایجاد تخفیف");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"خطای سیستمی: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteAsync(int Id)
        {
            if (Id < 0)
                return new OperationResult(false, "شناسه نامعتبر");
            var discount = await _orderDiscountRepository.GetByIdAsync(Id);
            if (discount == null) return new OperationResult(false, "تخفیفی با شناسه ارسالی یافت نشد ");

            var res = await _orderDiscountRepository.DeleteAsync(discount);
            if (res.Success)
                return new OperationResult(true, "تخفیف با موفقیت حذف شد ");
            return new OperationResult(false, ValidationMessages.SystemErrorMessage);
        }

        public async Task<OperationResult> EditOrderDiscountAsync(UpsertOrderDiscountCommandModel commandModel)
        {
            try
            {
                if (commandModel == null)
                    return new OperationResult(false, "داده‌های تخفیف معتبر نیست");

                if (commandModel.Id <= 0)
                    return new OperationResult(false, "شناسه تخفیف معتبر نیست");

                if (string.IsNullOrWhiteSpace(commandModel.Title))
                    return new OperationResult(false, "عنوان تخفیف الزامی است");

                if (commandModel.Percent < 0 || commandModel.Percent > 100)
                    return new OperationResult(false, "درصد تخفیف باید بین 0 تا 100 باشد");

                if (commandModel.Count <= 0)
                    return new OperationResult(false, "تعداد تخفیف باید بیشتر از صفر باشد");

                if (string.IsNullOrWhiteSpace(commandModel.Code))
                    return new OperationResult(false, "کد تخفیف الزامی است");

                var discount =
                    await _orderDiscountRepository.GetByIdAsync(commandModel.Id);

                if (discount == null)
                    return new OperationResult(false, "تخفیف مورد نظر یافت نشد");

                discount.Edit(
                    commandModel.Percent,
                    commandModel.Title,
                    commandModel.Code,
                    commandModel.Count,
                    commandModel.StartDate.ToEnglishDateTime(),
                    commandModel.EndDate.ToEnglishDateTime()
                );

                var result = await _orderDiscountRepository.SaveAsync();

                return result
                    ? new OperationResult(true, "تخفیف با موفقیت ویرایش شد")
                    : new OperationResult(false, "خطا در ویرایش تخفیف");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"خطای سیستمی: {ex.Message}");
            }
        }
        public async Task<UpsertOrderDiscountCommandModel> GetForEditAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return null;

                var discount = await _orderDiscountRepository.GetByIdAsync(id);

                if (discount == null)
                    return null;


                return new UpsertOrderDiscountCommandModel
                {
                    Id = discount.Id,
                    Percent = discount.Percent,
                    Title = discount.Title,
                    Count = discount.Count,
                    Code = discount.Code,
                    StartDate = discount.StartDate.ToPersainDate(),
                    EndDate = discount.EndDate.ToPersainDate(),
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
