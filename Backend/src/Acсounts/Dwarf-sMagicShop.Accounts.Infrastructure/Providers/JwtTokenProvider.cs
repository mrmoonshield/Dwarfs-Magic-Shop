using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Domain.Settings;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
	private readonly JwtSettings jwtSettings;
	private readonly RefreshSettings refreshSettings;
	private readonly AccountDbContext accountDbContext;

	public JwtTokenProvider(
		IOptions<JwtSettings> jwtOptions,
		IOptions<RefreshSettings> refreshOptions,
		AccountDbContext accountDbContext)
	{
		jwtSettings = jwtOptions.Value;
		refreshSettings = refreshOptions.Value;
		this.accountDbContext = accountDbContext;
	}

	public (string token, Guid jti) GenerateAccessToken(User user)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
		var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var jti = Guid.NewGuid();

		Claim[] claims =
			[
				new Claim(CustomClaims.USER_ID, user.Id.ToString()),
				new Claim(CustomClaims.ROLE, user.Role.Name!),
				new Claim(CustomClaims.JTI, jti.ToString())
			];

		var token = new JwtSecurityToken(
			jwtSettings.Issuer,
			jwtSettings.Audience,
			claims,
			expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings.ExpiredTimeMinutes)),
			signingCredentials: creds);

		return (new JwtSecurityTokenHandler().WriteToken(token), jti);
	}

	public async Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken cancellationToken)
	{
		var refreshSession = new RefreshSession
		{
			User = user,
			CreatedAt = DateTime.UtcNow,
			ExpiresIn = DateTime.UtcNow.AddDays(refreshSettings.ExpiresDays),
			Jti = jti,
			RefreshToken = Guid.NewGuid()
		};

		accountDbContext.RefreshSessions.Add(refreshSession);
		await accountDbContext.SaveChangesAsync(cancellationToken);
		return refreshSession.RefreshToken;
	}
}
