using Blogs.Domain.BlogAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Infrastructure.Persistence.EFConfig
{
    public class BlogConfig : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable("Blogs");
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Title).IsRequired().HasMaxLength(250);
            builder.Property(b => b.Slug).IsRequired().HasMaxLength(300);
            builder.Property(b => b.ShortDescription).IsRequired().HasMaxLength(600);
            builder.Property(b => b.Text).IsRequired();
            builder.Property(b => b.ImageName).IsRequired().HasMaxLength(150);
            builder.Property(b => b.ImageAlt).IsRequired().HasMaxLength(150);
            builder.Property(b => b.Writer).IsRequired().HasMaxLength(300);
        }
    }
}
