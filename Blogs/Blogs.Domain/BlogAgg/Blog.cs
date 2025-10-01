

using Utility.Shared.Domain;

namespace Blogs.Domain.BlogAgg
{
    public class Blog : BaseEntityCreate<int>
    {

        public string Title { get; private set; }
        public long UserId { get; private set; }
        public string Writer { get; private set; }
        public string Slug { get; private set; }
        public string ShortDescription { get; private set; }
        public string Text { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public int CategoryId { get; private set; }
        public int SubCategoryId { get; private set; }
        public int VisitCount { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public bool Active { get; private set; }

        public Blog(string title, long userId, string writer, string slug,
            string shortDescription, string text, string imageName, string imageAlt,
            int categoryId, int subCategoryId)
        {
            Title = title;
            UserId = userId;
            Writer = writer;
            Slug = slug;
            ShortDescription = shortDescription;
            Text = text;
            ImageName = imageName;
            ImageAlt = imageAlt;
            VisitCount = 0;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            UpdateDate = DateTime.Now;
            Active = true;
        }
        public void Edit(string title, string writer, string slug,
           string shortDescription, string text, string imageName, string imageAlt,
           int categoryId, int subCategoryId)
        {
            Title = title;
            Writer = writer;
            Slug = slug;
            ShortDescription = shortDescription;
            Text = text;
            ImageName = imageName;
            ImageAlt = imageAlt;
            CategoryId = categoryId;
            SubCategoryId = subCategoryId;
            UpdateDate = DateTime.Now;
            Active = true;
        }
        public void ActivationChange()
        {
            if (Active==true)
            {
                Active = false;
            }
            else
            {
                Active = true;
            }

        }
        public void VisitPlus()
        {
            VisitCount++;
        }
    }
}
