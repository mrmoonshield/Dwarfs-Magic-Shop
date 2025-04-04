using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Repositories;

public class AccountRepository(AccountDbContext accountDbContext) : IAccountRepository
{
	public async Task SaveAsync(CancellationToken cancellationToken)
	{
		await accountDbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<UnitResult<Error>> AddCrafterAccountAsync(
		CrafterAccount crafterAccount,
		CancellationToken cancellationToken)
	{
		try
		{
			await accountDbContext.CrafterAccounts.AddAsync(crafterAccount, cancellationToken);
			return Result.Success<Error>();
		}
		catch (Exception ex)
		{
			return Errors.InvalidOperation(ex.Message);
		}
	}

	public async Task<Result<Role, Error>> GetRoleAsync(string roleName, CancellationToken cancellationToken)
	{
		var role = await accountDbContext.Roles.FirstOrDefaultAsync(a => a.Name == roleName, cancellationToken);

		if (role == null)
			return Errors.NotFound($"Role {roleName}");

		return role;
	}

	public async Task<IEnumerable<Permission>> GetPermissionsAsync(User user, CancellationToken cancellationToken)
	{
		var permissions = await accountDbContext.Users
			.Where(a => a == user)
			.Join(accountDbContext.RolePermissions,
			a => a.Role,
			b => b.Role,
			(a, b) => b.Permission)
			.ToListAsync(cancellationToken);

		return permissions!;
	}

	public async Task<Result<User, Error>> GetUserAsync(string userName, CancellationToken cancellationToken)
	{
		var user = await accountDbContext.Users
			.Where(a => a.UserName == userName)
			.Include(a => a.Role)
			.ThenInclude(a => a.RolePermissions)
			.ThenInclude(a => a.Permission)
			.FirstOrDefaultAsync(cancellationToken);

		if (user == null)
			return Errors.NotFound(userName);

		return user;
	}
}
