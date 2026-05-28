namespace Query.Contract.UI.Products
{
    public class SellerInfoQueryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public int SellCount { get; set; }
        public string? CoverImageName { get; set; }
        public string AvatarImageName  { get; set; }
        public string ImageAlt { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string? Address { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
        public int InShop { get; set; }
        public int ProductCount { get; set; }

    }
}
