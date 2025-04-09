using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.Requests;
using Dwarf_sMagicShop.Accounts.Application.Validators;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Accounts.Application.Login;

public class LoginUserHandler : IResultHandler<LoginResponse, AccountUserRequest>
{
	private readonly ExistingAccountValidator existingAccountValidator;
	private readonly ITokenProvider tokenProvider;

	public LoginUserHandler(
		ExistingAccountValidator existingAccountValidator,
		ITokenProvider tokenProvider)
	{
		this.existingAccountValidator = existingAccountValidator;
		this.tokenProvider = tokenProvider;
	}

	public async Task<Result<LoginResponse, ErrorsList>> ExecuteAsync(AccountUserRequest request, CancellationToken cancellationToken)
	{
		var userResult = await existingAccountValidator
			.CheckAccountWithPasswordAsync(request.UserName, request.Password, cancellationToken);

		if (userResult.IsFailure)
			return userResult.Error.ToErrorsList();

		var accessToken = tokenProvider.GenerateAccessToken(userResult.Value!);

		var refreshToken = await tokenProvider.GenerateRefreshTokenAsync(
			userResult.Value!, 
			accessToken.jti, 
			cancellationToken);

		return new LoginResponse(accessToken.token, refreshToken);
	}
}
