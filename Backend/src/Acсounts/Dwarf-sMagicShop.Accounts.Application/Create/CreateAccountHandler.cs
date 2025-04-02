using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Requests;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Application.Create;

public class CreateAccountHandler : IUnitResultHandler<AccountUserRequest>
{
	private readonly UserManager<User> userManager;

	public CreateAccountHandler(UserManager<User> userManager)
	{
		this.userManager = userManager;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(AccountUserRequest request, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByNameAsync(request.UserName);

		if (user != null)
			return Errors.ValueIsAlreadyExist(request.UserName).ToErrorsList();

		user = new User { UserName = request.UserName };
		var result = await userManager.CreateAsync(user, request.Password);

		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(a => Error.Failure(a.Code, a.Description));
			return errors.ToErrorsList();
		}

		//userManager.AddToRoleAsync(user)

		return Result.Success<ErrorsList>();
	}
}
