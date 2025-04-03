using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

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
}
