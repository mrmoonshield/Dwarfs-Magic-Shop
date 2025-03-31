using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Species.Application.ArtefactsSpecies.Delete;

public class DeleteArtefactSpeciesHandler : IUnitResultHandler<Guid>
{
	private readonly IReadDbContextCrafter readDbContext;
	private readonly ISpeciesRepository speciesRepository;

	public DeleteArtefactSpeciesHandler(
		IReadDbContextCrafter readDbContext,
		ISpeciesRepository speciesRepository)
	{
		this.readDbContext = readDbContext;
		this.speciesRepository = speciesRepository;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
	{
		var inUse = await readDbContext.MagicArtefacts
			.AnyAsync(a => a.SpeciesId == id, cancellationToken);

		if (inUse)
			return Errors.InvalidOperation($"Unable to delete {id}, because species is in use").ToErrorsList();

		var result = await speciesRepository.Delete(id, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		return Result.Success<ErrorsList>();
	}
}