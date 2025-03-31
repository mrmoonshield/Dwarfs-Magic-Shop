using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts
{
	public class AccountDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DATABASE));
			optionsBuilder.UseSnakeCaseNamingConvention();
			optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<User>().ToTable("users");
			builder.Entity<Role>().ToTable("roles");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
			builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
		}

		private ILoggerFactory CreateLoggerFactory()
		{
			return LoggerFactory.Create(builder => builder.AddConsole());
		}
	}
}
