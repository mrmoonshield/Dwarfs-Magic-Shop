using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.Requests;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Application.Create;

public class CreateAccountHandler : IUnitResultHandler<AccountUserRequest>
{
	private readonly UserManager<User> userManager;
	private readonly IAccountRepository accountRepository;

	public CreateAccountHandler(UserManager<User> userManager, IAccountRepository accountRepository)
	{
		this.userManager = userManager;
		this.accountRepository = accountRepository;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(AccountUserRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByNameAsync(request.UserName);

		if (user != null)
			return Errors.ValueIsAlreadyExist(request.UserName).ToErrorsList();

		var roleResult = await accountRepository.GetRoleAsync(Roles.USER, cancellationToken);

		if (roleResult.IsFailure)
			return roleResult.ToErrorsList();

		user = new User { UserName = request.UserName, Role = roleResult.Value };
		var result = await userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(a => Error.Failure(a.Code, a.Description));
			return errors.ToErrorsList();
		}

		await userManager.AddToRoleAsync(user, Roles.USER);
		return Result.Success<ErrorsList>();
	}
}
