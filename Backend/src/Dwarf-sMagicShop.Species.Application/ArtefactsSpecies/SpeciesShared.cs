using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Species.Domain.Models;

namespace Dwarf_sMagicShop.Species.Application.ArtefactsSpecies;

public static class SpeciesShared
{
	public static async Task<Result<ArtefactSpecies, Error>> CheckSpecies(
		string name,
		ISpeciesRepository speciesRepository,
		CancellationToken cancellationToken)
	{
		var existResult = await speciesRepository.GetByNameAsync(
			name!,
			cancellationToken);

		if (existResult.IsSuccess) return existResult;
		var speciesResult = ArtefactSpecies.Create(
			ArtefactSpeciesID.NewArtefactSpeciesID,
			name!);

		var saveResult = await speciesRepository.AddAsync(speciesResult.Value, cancellationToken);
		return saveResult;
	}
}