namespace Blogs.Application.Contract.BlogService.Query
{
    public class MostViewedPosts
    {
       

        public int Id { get; set; }
        public string Title { get; set; }
        public string Writer { get; set; }
        public string CreateDate { get; set; }
        public string ImageAlt { get; set; }
        public string ImageName { get; set; }
        public string BlogSlug { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public int Seen { get; set; }


    }
}
