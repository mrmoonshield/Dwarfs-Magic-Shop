using Dwarf_sMagicShop.Accounts.Domain.Models;

namespace Dwarf_sMagicShop.Accounts.Application.Abstracts;

public interface ITokenProvider
{
	(string token, Guid jti) GenerateAccessToken(User user);
	Task<Guid> GenerateRefreshTokenAsync(User user, Guid jti, CancellationToken cancellationToken);
}
