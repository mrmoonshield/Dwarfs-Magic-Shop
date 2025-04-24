using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Messages;
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
		public DbSet<CrafterAccount> CrafterAccounts => Set<CrafterAccount>();
		public DbSet<RefreshSession> RefreshSessions => Set<RefreshSession>();
		public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

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
			builder.Entity<CrafterAccount>().ToTable("crafter_accounts");
			builder.Entity<RefreshSession>().ToTable("refresh_sessions");
			builder.Entity<OutboxMessage>().ToTable("outbox_messages");
			builder.HasDefaultSchema("accounts");

			builder.Entity<CrafterAccount>()
				.HasKey(a => a.Id)
				.HasName("crafter_account_id");

			builder.Entity<CrafterAccount>()
				.Property(a => a.Socials)
				.HasConversion(
					socials => JsonSerializer.Serialize(socials, JsonSerializerOptions.Default),
					json => JsonSerializer.Deserialize<List<Social>>(json, JsonSerializerOptions.Default)!)
				.HasColumnName("socials")
				.IsRequired(false);

			builder.Entity<CrafterAccount>()
				.HasOne(a => a.User);

			builder.Entity<CrafterAccount>()
				.Property(a => a.CrafterId)
				.HasColumnName("crafter_id");

			builder.Entity<Permission>()
				.HasIndex(a => a.Code)
				.IsUnique();

			builder.Entity<RolePermission>()
				.HasOne(a => a.Role)
				.WithMany(a => a.RolePermissions)
				.HasForeignKey(a => a.RoleId);

			builder.Entity<RolePermission>()
				.HasOne(a => a.Permission)
				.WithMany()
				.HasForeignKey(a => a.PermissionId);

			builder.Entity<RolePermission>()
				.HasKey(a => new { a.RoleId, a.PermissionId });

			builder.Entity<User>()
				.HasOne(a => a.Role)
				.WithMany()
				.HasForeignKey("role_id")
				.IsRequired(true);

			builder.Entity<RefreshSession>()
				.HasKey(a => a.Id);

			builder.Entity<RefreshSession>()
				.HasOne(a => a.User)
				.WithMany()
				.HasForeignKey(a => a.UserId);

			builder.Entity<OutboxMessage>()
				.HasKey(a => a.Id);

			builder.Entity<OutboxMessage>()
				.Property(a => a.Type)
				.IsRequired();

			builder.Entity<OutboxMessage>()
				.Property(a => a.Data)
				.IsRequired();

			builder.Entity<OutboxMessage>()
				.Property(a => a.OccuredDate)
				.HasConversion<long>()
				.IsRequired();

			builder.Entity<OutboxMessage>()
				.Property(a => a.ProcessedDate)
				.HasConversion<long>();

			builder.Entity<OutboxMessage>()
				.HasIndex(a => new { a.OccuredDate, a.ProcessedDate })
				.HasDatabaseName("idx_outbox_messages_unprocessed")
				.IncludeProperties(a => new { a.Id, a.Type, a.Data })
				.HasFilter("processed_date IS NULL");

		}

		private ILoggerFactory CreateLoggerFactory()
		{
			return LoggerFactory.Create(builder => builder.AddConsole());
		}
	}
}
