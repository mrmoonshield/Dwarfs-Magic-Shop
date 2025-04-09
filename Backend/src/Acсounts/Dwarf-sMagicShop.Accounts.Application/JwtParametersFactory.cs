using Dwarf_sMagicShop.Accounts.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dwarf_sMagicShop.Accounts.Application;

public static class JwtParametersFactory
{
	public static TokenValidationParameters GetTokenValidationParameters(JwtSettings jwtSettings, bool withLifetime)
	{
		return new TokenValidationParameters
		{
			ValidIssuer = jwtSettings.Issuer,
			ValidAudience = jwtSettings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
			ValidateLifetime = withLifetime,
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateIssuerSigningKey = true
		};
	}
}
