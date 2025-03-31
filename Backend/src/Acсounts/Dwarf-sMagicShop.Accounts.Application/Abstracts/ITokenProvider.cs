using Dwarf_sMagicShop.Accounts.Domain.Models;

namespace Dwarf_sMagicShop.Accounts.Application.Abstracts;

public interface ITokenProvider
{
	string GenerateAccessToken(User user, CancellationToken cancellationToken);
}
