using Emails.Domailn.EmailAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Emails.Infrastructure.Persistence.EFConfig
{
	internal class EmailUserConfig : IEntityTypeConfiguration<EmailUser>
	{
		public void Configure(EntityTypeBuilder<EmailUser> builder)
		{
			builder.ToTable("EmailUsers");
			builder.HasKey(b => b.Id);
			builder.Property(b => b.Email).IsRequired(true).HasMaxLength(255);
		}
	}
}
