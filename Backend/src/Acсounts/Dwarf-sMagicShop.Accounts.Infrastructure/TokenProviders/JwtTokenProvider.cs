using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.TokenProviders;

public class JwtTokenProvider : ITokenProvider
{
	private readonly JwtSettings jwtOptions;
	private const string SUBJECT = "Sub";

	public JwtTokenProvider(IOptions<JwtSettings> jwtOptions)
	{
		this.jwtOptions = jwtOptions.Value;
	}

	public string GenerateAccessToken(User user, CancellationToken cancellationToken)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
		var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		Claim[] claims = [new Claim(SUBJECT, user.Id.ToString())];

		var token = new JwtSecurityToken(
			jwtOptions.Issuer,
			jwtOptions.Audience,
			claims,
			expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtOptions.ExpiredTimeMinutes)),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
