using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Species.Domain.Models;

namespace Dwarf_sMagicShop.Species.Application.ArtefactsSpecies
{
	public interface ISpeciesRepository
	{
		Task<Result<ArtefactSpecies, Error>> AddAsync(ArtefactSpecies species, CancellationToken cancellationToken);

		Task<UnitResult<Error>> Delete(Guid id, CancellationToken cancellationToken = default);

		Task<Result<ArtefactSpecies, Error>> GetByIDAsync(ArtefactSpeciesID id, CancellationToken cancellationToken);

		Task<Result<ArtefactSpeciesDto, Error>> GetByIDAsync(Guid id, CancellationToken cancellationToken);

		Task<Result<ArtefactSpecies, Error>> GetByNameAsync(string name, CancellationToken cancellationToken);

		Task<Result<ArtefactSpecies, Error>> SaveAsync(ArtefactSpecies species, CancellationToken cancellationToken);
	}
}