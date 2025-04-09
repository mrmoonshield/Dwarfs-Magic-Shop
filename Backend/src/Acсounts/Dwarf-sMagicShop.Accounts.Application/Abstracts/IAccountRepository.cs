using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Accounts.Application.Abstracts;

public interface IAccountRepository
{
	Task<UnitResult<Error>> AddCrafterAccountAsync(CrafterAccount crafterAccount, CancellationToken cancellationToken);
	Task<Result<Role, Error>> GetRoleAsync(string roleName, CancellationToken cancellationToken);
	Task<Result<User, Error>> GetUserAsync(string userName, CancellationToken cancellationToken);
	Task SaveAsync(CancellationToken cancellationToken);
	Task<IEnumerable<Permission>> GetPermissionsAsync(User user, CancellationToken cancellationToken);
	Task<Result<CrafterAccount, Error>> GetCrafterAccountAsync(Guid id, CancellationToken cancellationToken);
	Task<Result<RefreshSession, Error>> GetRefreshSessionAsync(Guid refreshToken, CancellationToken cancellationToken);
	Task DeleteRefreshSessionAsync(RefreshSession refreshSession);
}
