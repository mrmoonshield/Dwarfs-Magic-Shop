using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Validators;
using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Application.Validators;

public class ExistingAccountValidator : ICustomValidator
{
	private readonly UserManager<User> userManager;

	public ExistingAccountValidator(UserManager<User> userManager)
	{
		this.userManager = userManager;
	}

	public async Task<Result<User?, Error>> CheckAccountAsync(string userName, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByNameAsync(userName);

		if (user == null)
			return Errors.NotFound(userName);

		return user;
	}

	public async Task<Result<User?, Error>> CheckAccountWithPasswordAsync(
		string userName,
		string password,
		CancellationToken cancellationToken)
	{
		var user = await userManager.FindByNameAsync(userName);

		if (user == null)
			return Errors.NotFound(userName);

		var passwordValid = await userManager.CheckPasswordAsync(user, password);

		if (!passwordValid)
			return Errors.ValueIsInvalid("User name or password");

		return user;
	}
}
