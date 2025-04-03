using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Accounts.Application.Abstracts;

public interface IAccountRepository
{
	Task<UnitResult<Error>> AddCrafterAccountAsync(CrafterAccount crafterAccount, CancellationToken cancellationToken);
	Task SaveAsync(CancellationToken cancellationToken);
}
