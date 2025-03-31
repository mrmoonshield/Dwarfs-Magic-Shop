using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies;
using Dwarf_sMagicShop.Species.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Species.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
	private readonly WriteDbContextCrafters dbContext;
	private readonly IReadDbContextCrafter readDbContext;

	public SpeciesRepository(WriteDbContextCrafters dbContext, IReadDbContextCrafter readDbContext)
	{
		this.dbContext = dbContext;
		this.readDbContext = readDbContext;
	}

	public async Task<Result<ArtefactSpecies, Error>> AddAsync(ArtefactSpecies species, CancellationToken cancellationToken)
	{
		var result = await dbContext.ArtefactSpecies.AddAsync(species, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		return result.Entity;
	}

	public async Task<Result<ArtefactSpecies, Error>> SaveAsync(ArtefactSpecies species, CancellationToken cancellationToken)
	{
		dbContext.ArtefactSpecies.Attach(species);
		await dbContext.SaveChangesAsync(cancellationToken);
		return species;
	}

	public async Task<Result<ArtefactSpecies, Error>> GetByIDAsync(ArtefactSpeciesID id, CancellationToken cancellationToken)
	{
		var species = await dbContext.ArtefactSpecies
			.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

		if (species == null) return Errors.NotFound(id.Value);
		return species;
	}

	public async Task<Result<ArtefactSpeciesDto, Error>> GetByIDAsync(Guid id, CancellationToken cancellationToken)
	{
		var species = await readDbContext.ArtefactSpecies
			.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

		if (species == null) return Errors.NotFound(id);
		return species;
	}

	public async Task<Result<ArtefactSpecies, Error>> GetByNameAsync(string name, CancellationToken cancellationToken)
	{
		var species = await dbContext.ArtefactSpecies
			.FirstOrDefaultAsync(a => a.Name == name, cancellationToken);

		if (species == null) return Errors.NotFound("Name");
		return species;
	}

	public async Task<UnitResult<Error>> Delete(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await dbContext.ArtefactSpecies.FirstOrDefaultAsync(a => a.Id.Value == id, cancellationToken);

		if (entity == null)
			return Errors.NotFound(id.ToString());

		dbContext.ArtefactSpecies.Remove(entity);
		await dbContext.SaveChangesAsync(cancellationToken);
		return Result.Success<Error>();
	}
}