using Emails.Domailn.SendEmailAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Emails.Infrastructure.Persistence.EFConfig
{
	internal class SendEmailConfig : IEntityTypeConfiguration<SendEmail>
	{
		public void Configure(EntityTypeBuilder<SendEmail> builder)
		{
			builder.ToTable("SendEmails");
			builder.HasKey(b => b.Id);
			builder.Property(b=>b.Title).IsRequired(true).HasMaxLength(255);
			builder.Property(b=>b.Text).IsRequired(true);
		}
	}
}
