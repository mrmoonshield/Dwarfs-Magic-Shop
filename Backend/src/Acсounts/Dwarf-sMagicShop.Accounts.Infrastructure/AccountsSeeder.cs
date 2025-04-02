using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Accounts.Infrastructure;

public class AccountsSeeder(IServiceScopeFactory serviceScopeFactory, ILogger<AccountsSeeder> logger)
{
	public async Task SeedAsync(List<(string role, string[] permissions)> list)
	{
		using var scope = serviceScopeFactory.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
		await SeedPermissions(list, dbContext);
		await SeedRoles(list, roleManager);
		await dbContext.SaveChangesAsync();
		await SeedRolePermissionsAsync(list, dbContext);
		await dbContext.SaveChangesAsync();
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

				var rolePermission = new RolePermission
				{
					RoleId = existRole!.Id,
					PermissionId = existPermission!.Id
				};

				await dbContext.RolePermissions.AddAsync(rolePermission);
			}
		}

		logger.LogInformation("RolePermissions seeded successfully");
	}
}
