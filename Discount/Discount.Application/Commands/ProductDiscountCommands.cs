using Discount.Application.Contract.ProductDiscount.Command;
using Discount.Domain.ProductDiscountAgg;
using Microsoft.VisualBasic;
using Shared.Application;


namespace Discount.Application.Commands
{


    internal class ProductDiscountCommands : IProductDiscountCommands
    {
        private readonly IProductDiscountRepository _productDiscountRepository;

        public ProductDiscountCommands(IProductDiscountRepository productDiscountRepository)
        {
            _productDiscountRepository = productDiscountRepository;
        }

        public async Task<OperationResult> UpSertAsync(UpsertProductDiscountCommandModel command)
        {
            try
            {

                if (command == null)
                    return new OperationResult(false, "داده‌های تخفیف معتبر نیست");

                if (command.ProductId <= 0)
                    return new OperationResult(false, "شناسه محصول معتبر نیست");

                if (command.DiscountPercent < 0 || command.DiscountPercent > 100)
                    return new OperationResult(false, "درصد تخفیف باید بین 0 تا 100 باشد");

                DateTime startDate = DateTime.Now;
                DateTime EndDate = DateTime.Now;
                if (string.IsNullOrEmpty(command.StartDate))
                    return new OperationResult(false, "تاریخ شروع نمیتواند خالی باشد");
                if (string.IsNullOrEmpty(command.EndDate))
                    return new OperationResult(false, "تاریخ پایان نمیتواند خالی باشد");

                startDate = command.StartDate.ToEnglishDateTime();
                EndDate = command.EndDate.ToEnglishDateTime();



                if (startDate >= EndDate)
                    return new OperationResult(false, "تاریخ شروع باید قبل از تاریخ پایان باشد");

                if (startDate.AddDays(1) < DateTime.Now)
                    return new OperationResult(false, "تاریخ شروع نمی‌تواند در گذشته باشد");
         
                var discont = await _productDiscountRepository.GetByProductSellIdAsync(command.ProductId, command.ProductSellId);
                bool result = false;
                if (discont == null) 
                {
                    var discount = new ProductDiscount(command.ProductId, command.ProductSellId, command.DiscountPercent, startDate, EndDate);
                   var  CreateRes = await _productDiscountRepository.CreateAsync(discount);
                    if(CreateRes.Success)
                        result = true;
                }
                else
                {
                    discont.Edit(command.DiscountPercent,startDate,EndDate);
                    result = await _productDiscountRepository.SaveAsync();
                }

                if (result)
                    return new OperationResult(true, "تخفیف با موفقیت اعمال شد");

                return new OperationResult(false, "خطا در ایجاد تخفیف");
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"خطای سیستمی: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteAsync(int Id)
        {

            if (Id <= 0)
                return new OperationResult(false, "شناسه محصول معتبر نیست");

            var discount = await _productDiscountRepository.GetByIdAsync(Id);
            var result = await _productDiscountRepository.DeleteAsync(discount);
            if (result.Success)
                return new OperationResult(true, "تخفیف محصول با موفقیت حذف شد ");
            return new OperationResult(false, "خطا در حذف تخفیف محصول");

        }

        public async Task<UpsertProductDiscountCommandModel> GetForUpsertAsync(int productId,  int ProductSellId)
        {
            try
            {

                var discount = await _productDiscountRepository.GetByProductSellIdAsync(productId, ProductSellId);

                if (discount == null)
                    return null;


                return new UpsertProductDiscountCommandModel
                {
                    ProductId = discount.ProductId,
                    ProductSellId = discount.ProductSellId,
                    DiscountPercent = discount.Percent,
                    StartDate = discount.StartDate.ToPersainDate(),
                    EndDate = discount.EndDate.ToPersainDate()
                };
            }
            catch
            {
                return null;
            }
        }

    }



}
