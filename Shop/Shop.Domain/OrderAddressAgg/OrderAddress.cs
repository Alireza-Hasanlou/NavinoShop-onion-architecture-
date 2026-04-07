using Shared.Domain;

namespace Shop.Domain.OrderAddressAgg
{
    public class OrderAddress:BaseEntity<int>
    {
        public int StateId { get; private set; }
        public int CityId { get; private set; }
        public string AddressDetail { get; private set; }
        public string PostalCode { get; private set; }
        public string Phone { get; private set; }
        public string FullName { get; private set; }
        public string NationalCode { get; private set; }
        public int OrderId { get; private set; }
        public OrderAddress()
        {
            
        }
        public OrderAddress(int stateId, int cityId, string addressDetail, string postalCode,
                           string phone, string fullName, string nationalCode, int orderId)

        {
            StateId = stateId;
            CityId = cityId;
            AddressDetail = addressDetail;
            PostalCode = postalCode;
            Phone = phone;
            FullName = fullName;
            NationalCode = nationalCode;
            OrderId = orderId;

        }

        public void Edit(int stateId, int cityId, string addressDetail, string postalCode,
                            string phone, string fullName, string nationalCode, int orderId)

        {
            StateId = stateId;
            CityId = cityId;
            AddressDetail = addressDetail;
            PostalCode = postalCode;
            Phone = phone;
            FullName = fullName;
            NationalCode = nationalCode;
            OrderId = orderId;

        }
    }
}

