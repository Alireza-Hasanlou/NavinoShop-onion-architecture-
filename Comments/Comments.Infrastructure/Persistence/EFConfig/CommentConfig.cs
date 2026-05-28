using Comments.Domain.CommentAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comments.Infrastructure.Persistence.EFConfig
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");
            builder.HasKey(x => x.Id);

            builder.Property(b => b.FullName).IsRequired(false).HasMaxLength(250);
            builder.Property(b => b.WhyRejected).IsRequired(false).HasMaxLength(350);
            builder.Property(b => b.CommentFor).IsRequired(true);
            builder.Property(b => b.Status).IsRequired(true);
            builder.Property(b => b.UserId).IsRequired(true);  

      
            builder.HasMany(b => b.Childs)
                .WithOne(x => x.Parent)
                .HasForeignKey(b => b.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);  

          
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OwnerId);
            builder.HasIndex(x => x.CommentFor);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.ParentId);
        }
    }
}
