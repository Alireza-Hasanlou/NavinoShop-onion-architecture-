using Emails.Domailn.EmailAgg;
using Emails.Domailn.MessageUserAgg;
using Emails.Domailn.SendEmailAgg;
using Emails.Infrastructure.Persistence.EFConfig;
using Microsoft.EntityFrameworkCore;


namespace Emails.Infrastructure.Persistence.Context
{
	public class EmailContext : DbContext
	{
        public EmailContext(DbContextOptions<EmailContext> options): base(options)
        {
            
        }
        public DbSet<EmailUser> EmailUsers { get; set; }
        public DbSet<SendEmail> SendEmails { get; set; }
        public DbSet<MessageUser> MessageUsers { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(SendEmailConfig).Assembly);
			base.OnModelCreating(modelBuilder);
		}
	}
}
