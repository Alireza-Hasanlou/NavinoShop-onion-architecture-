using Shared.Domain;

namespace Shop.Domain.ShopSettingAgg
{
    public class ShopSetting : BaseEntity<int>
    {
        public ShopSetting(int sellerDefaultId)
        {
            SellerDefaultId = sellerDefaultId;
        }

        public int SellerDefaultId { get; private set; }

        public void Edit(int sellerDefaultId)
        {
            SellerDefaultId = sellerDefaultId;
        }
    }
}
