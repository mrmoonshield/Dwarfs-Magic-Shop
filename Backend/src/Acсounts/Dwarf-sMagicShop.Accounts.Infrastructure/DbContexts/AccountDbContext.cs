using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts
{
	public class AccountDbContext(IConfiguration configuration) : IdentityDbContext<User, Role, Guid>
	{
		public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
		public DbSet<Permission> Permissions => Set<Permission>();

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
			builder.Entity<Permission>().ToTable("permissions");
			builder.Entity<RolePermission>().ToTable("role_permissions");
			builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
			builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
			builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
			builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
			builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");

			//builder.Entity<User>()
			//	.Property(a => a.Socials)
			//	.HasConversion(
			//		socials => JsonSerializer.Serialize(socials, JsonSerializerOptions.Default),
			//		json => JsonSerializer.Deserialize<List<Social>>(json, JsonSerializerOptions.Default)!)
			//	.IsRequired(false);

			builder.Entity<Permission>()
				.HasIndex(a => a.Code)
				.IsUnique();

			builder.Entity<RolePermission>()
				.HasOne(a => a.Role)
				.WithMany(a => a.Permissions)
				.HasForeignKey(a => a.RoleId);

			builder.Entity<RolePermission>()
				.HasOne(a => a.Permission)
				.WithMany()
				.HasForeignKey(a => a.PermissionId);

			builder.Entity<RolePermission>()
				.HasKey(a => new { a.RoleId, a.PermissionId });

			builder.HasDefaultSchema("accounts");
		}

		private ILoggerFactory CreateLoggerFactory()
		{
			return LoggerFactory.Create(builder => builder.AddConsole());
		}
	}
}
