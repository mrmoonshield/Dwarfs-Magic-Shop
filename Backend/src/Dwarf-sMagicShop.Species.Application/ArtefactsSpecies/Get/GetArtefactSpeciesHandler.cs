using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Species.Application.ArtefactsSpecies.Get;

public class GetArtefactSpeciesHandler : IResultHandler<IReadOnlyCollection<ArtefactSpeciesDto>>
{
	private readonly IReadDbContextSpecies readDbContext;

	public GetArtefactSpeciesHandler(IReadDbContextSpecies readDbContext)
	{
		this.readDbContext = readDbContext;
	}

	public async Task<Result<IReadOnlyCollection<ArtefactSpeciesDto>, ErrorsList>> ExecuteAsync(CancellationToken cancellationToken)
	{
		var result = await readDbContext.ArtefactSpecies.ToListAsync(cancellationToken);
		return result;
	}
}