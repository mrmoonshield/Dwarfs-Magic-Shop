using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.Login;
using Dwarf_sMagicShop.Accounts.Domain.Settings;
using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Dwarf_sMagicShop.Accounts.Application.Refresh;

public class RefreshTokensHandler(
	IAccountRepository accountRepository,
	IOptions<JwtSettings> jwtOptions,
	ITokenProvider tokenProvider) : IResultHandler<LoginResponse, RefreshTokensRequest>
{
	public async Task<Result<LoginResponse, ErrorsList>> ExecuteAsync(
		RefreshTokensRequest request,
		CancellationToken cancellationToken)
	{
		var refreshSessionResult = await accountRepository
			.GetRefreshSessionAsync(request.RefreshToken, cancellationToken);

		if (refreshSessionResult.IsFailure)
			return refreshSessionResult.ToErrorsList();

		await accountRepository.DeleteRefreshSessionAsync(refreshSessionResult.Value);
		var isValid = refreshSessionResult.Value.ExpiresIn > DateTime.UtcNow;

		if (!isValid)
			return Errors.ValueIsInvalid("Token").ToErrorsList();

		var validationResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(
			request.AccessToken,
			JwtParametersFactory.GetTokenValidationParameters(jwtOptions.Value, false));

		if (!validationResult.IsValid)
			return Errors.ValueIsInvalid("Token").ToErrorsList();

		var jtiClaim = validationResult.Claims[CustomClaims.JTI];

		if (refreshSessionResult.Value.Jti.ToString() != jtiClaim.ToString())
			return Errors.ValueIsInvalid("Token").ToErrorsList();

		var user = refreshSessionResult.Value.User;
		var accessToken = tokenProvider.GenerateAccessToken(user);

		var refreshToken = await tokenProvider.GenerateRefreshTokenAsync(
			user,
			accessToken.jti,
			cancellationToken);

		return new LoginResponse(accessToken.token, refreshToken);
	}
}