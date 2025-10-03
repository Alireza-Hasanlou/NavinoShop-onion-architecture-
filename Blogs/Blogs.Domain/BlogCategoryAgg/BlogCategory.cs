using Blogs.Domain.BlogAgg;
using Shared.Domain;

namespace Blogs.Domain.BlogCategoryAgg
{
    public class BlogCategory : BaseEntityCreateUpdateActive<int>
    {
        public string Title { get; private set; }
        public string Slug { get; private set; }
        public string ImageName { get; private set; }
        public string ImageAlt { get; private set; }
        public int Parent { get; private set; }

        public BlogCategory(string title, string slug, string imageName, string imageAlt, int parent)
        {
            Setvalues(title, slug, imageName, imageAlt);
            Parent = parent;
        }
        public void Edit(string title, string slug, string imageName, string imageAlt)
        {
            Setvalues(title, slug, imageName, imageAlt);
        }

        private void Setvalues(string title, string slug, string imageName, string imageAlt)
        {
            Title = title;
            Slug = slug;
            ImageName = imageName;
            ImageAlt = imageAlt;
        }

    }
}
