namespace Query.Contract.UI.UserPanel.Order
{
    public class OrderAddressQueryModel
    {

        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string AddressDetail { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string? NationalCode { get; set; }
    }
}