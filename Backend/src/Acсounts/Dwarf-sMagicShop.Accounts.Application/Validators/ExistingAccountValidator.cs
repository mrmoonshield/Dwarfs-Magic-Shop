using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Validators;
using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Application.Validators;

public class ExistingAccountValidator : ICustomValidator
{
	private readonly IAccountRepository accountRepository;
	private readonly UserManager<User> userManager;

	public ExistingAccountValidator(IAccountRepository accountRepository, UserManager<User> userManager)
	{
		this.accountRepository = accountRepository;
		this.userManager = userManager;
	}

	public async Task<Result<User?, Error>> CheckAccountAsync(string userName, CancellationToken cancellationToken)
	{
		var userResult = await accountRepository.GetUserByNameAsync(userName, cancellationToken);

		if (userResult.IsFailure)
			return userResult.Error;

		return userResult.Value;
	}

	public async Task<Result<User?, Error>> CheckAccountWithPasswordAsync(
		string userName,
		string password,
		CancellationToken cancellationToken)
	{
		var userResult = await accountRepository.GetUserByNameAsync(userName, cancellationToken);

		if (userResult.IsFailure)
			return Errors.ValueIsInvalid("User name or password");

		var passwordValid = await userManager.CheckPasswordAsync(userResult.Value, password);

		if (!passwordValid)
			return Errors.ValueIsInvalid("User name or password");

		return userResult.Value;
	}

	public async Task<Result<CrafterAccount?, Error>> CheckCrafterAccountAsync(Guid id, CancellationToken cancellationToken)
	{
		var crafterAccountResult = await accountRepository.GetCrafterAccountAsync(id, cancellationToken);

		if (crafterAccountResult.IsFailure)
			return crafterAccountResult.Error;

		return crafterAccountResult.Value;
	}
}
