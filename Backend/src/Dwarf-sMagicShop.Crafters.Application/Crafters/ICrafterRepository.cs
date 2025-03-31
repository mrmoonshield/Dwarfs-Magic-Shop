using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Domain.Models;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters;

public interface ICrafterRepository
{
	Task<Result<Crafter, Error>> AddAsync(Crafter crafter, CancellationToken cancellationToken);

	Task DeleteUnactiveEntitiesAsync();

	Task<Result<Crafter?, Error>> GetByIDAsync(CrafterID id, CancellationToken cancellationToken);

	Task<Result<CrafterDto?, Error>> GetByIDDtoAsync(Guid id, CancellationToken cancellationToken);

	Task<Result<Crafter?, Error>> GetByNicknameAsync(Nickname nickname, CancellationToken cancellationToken);

	Task<Result<Crafter, Error>> SaveAsync(Crafter crafter, CancellationToken cancellationToken);
}