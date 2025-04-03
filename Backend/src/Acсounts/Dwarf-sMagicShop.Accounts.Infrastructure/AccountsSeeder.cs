using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Accounts.Infrastructure.Settings;
using Dwarf_sMagicShop.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dwarf_sMagicShop.Accounts.Infrastructure;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
{
	public async Task SeedAsync(List<(string role, string[] permissions)> list)
	{
		using var scope = serviceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
		await SeedPermissions(list, dbContext);
		await SeedRoles(list, roleManager);
		await dbContext.SaveChangesAsync();
		await SeedRolePermissionsAsync(list, dbContext);
		await CheckAndDeleteUnusingRolePermissionsAsync(list, dbContext);
		await SeedAdminAsync(scope, userManager, dbContext);
	}

	private async Task SeedPermissions(List<(string role, string[] permissions)> list, AccountDbContext dbContext)
	{
		var allPermissionCodes = list.SelectMany(a => a.permissions).Distinct();

		foreach (var code in allPermissionCodes)
		{
			var exist = await dbContext.Permissions.AnyAsync(a => a.Code == code);
			if (exist) continue;
			await dbContext.Permissions.AddAsync(new Permission { Code = code });
		}

		logger.LogInformation("Permissions seeded successfully");
	}

	private async Task SeedRoles(List<(string role, string[] permissions)> list, RoleManager<Role> roleManager)
	{
		foreach (var data in list)
		{
			var exist = await roleManager.RoleExistsAsync(data.role);
			if (exist) continue;
			await roleManager.CreateAsync(new Role { Name = data.role });
		}

		logger.LogInformation("Roles seeded successfully");
	}

	private async Task SeedRolePermissionsAsync(List<(string role, string[] permissions)> list, AccountDbContext dbContext)
	{
		foreach (var data in list)
		{
			foreach (var permissionCode in data.permissions)
			{
				var exist = await dbContext.RolePermissions
					.AnyAsync(a => a.Role.Name == data.role && a.Permission.Code == permissionCode);

				if (exist) continue;
				var existRole = await dbContext.Roles.FirstOrDefaultAsync(a => a.Name == data.role);
				var existPermission = await dbContext.Permissions.FirstOrDefaultAsync(a => a.Code == permissionCode);

				if (existRole == null)
					throw new ArgumentNullException($"Role {data.role} is not found");

				if (existPermission == null)
					throw new ArgumentNullException($"Permission {permissionCode} is not found");

				var rolePermission = new RolePermission
				{
					RoleId = existRole!.Id,
					PermissionId = existPermission!.Id
				};

				await dbContext.RolePermissions.AddAsync(rolePermission);
			}
		}

		await dbContext.SaveChangesAsync();
		logger.LogInformation("RolePermissions seeded successfully");
	}

	private async Task CheckAndDeleteUnusingRolePermissionsAsync(
		List<(string role, string[] permissions)> list,
		AccountDbContext dbContext)
	{
		foreach (var data in list)
		{
			var rolePermissions = await dbContext.RolePermissions
				.Where(a => a.Role.Name == data.role)
				.Include(a => a.Permission)
				.ToListAsync();

			foreach (var rolePermission in rolePermissions)
			{
				if (!data.permissions.Any(a => a == rolePermission.Permission.Code))
					dbContext.RolePermissions.Remove(rolePermission);
			}
		}

		await dbContext.SaveChangesAsync();
		logger.LogInformation("RolePermissions checked successfully");
	}

	private async Task SeedAdminAsync(IServiceScope scope, UserManager<User> userManager, AccountDbContext dbContext)
	{
		var adminSettings = scope.ServiceProvider.GetRequiredService<IOptions<AdminSettings>>().Value;
		var role = await dbContext.Roles.FirstOrDefaultAsync(a => a.Name == Roles.ADMIN);
		var admin = new User { UserName = adminSettings.UserName, Role = role! };
		var result = await userManager.CreateAsync(admin, adminSettings.Password);

		if (!result.Succeeded)
		{
			var log = result.Errors.Select(a => a.Code).Aggregate((a, b) => a + "; " + b);
			logger.LogInformation($"Admin seeding failed: {log}");
			return;
		}

		await userManager.AddToRoleAsync(admin!, Roles.ADMIN);
		logger.LogInformation("Admin seeded successfully");
	}
}
