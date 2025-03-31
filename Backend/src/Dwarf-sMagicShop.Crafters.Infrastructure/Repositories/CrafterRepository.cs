using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Messages;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Crafters.Infrastructure.SettingsModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Repositories;

public class CrafterRepository : ICrafterRepository
{
	private readonly WriteDbContextCrafters dbContext;
	private readonly IReadDbContextCrafter readDbContext;
	private readonly IOptions<SoftDeleteSettings> options;
	private readonly IMessageQueue<string> messageQueue;

	public CrafterRepository(
		WriteDbContextCrafters dbContext,
		IReadDbContextCrafter readDbContext,
		IOptions<SoftDeleteSettings> options,
		IMessageQueue<string> messageQueue)
	{
		this.dbContext = dbContext;
		this.readDbContext = readDbContext;
		this.options = options;
		this.messageQueue = messageQueue;
	}

	public async Task<Result<Crafter, Error>> AddAsync(Crafter crafter, CancellationToken cancellationToken)
	{
		var result = await dbContext.Crafters.AddAsync(crafter, cancellationToken);
		await dbContext.SaveChangesAsync(cancellationToken);
		return result.Entity;
	}

	public async Task<Result<Crafter, Error>> SaveAsync(Crafter crafter, CancellationToken cancellationToken)
	{
		dbContext.Crafters.Attach(crafter);
		await dbContext.SaveChangesAsync(cancellationToken);
		return crafter;
	}

	public async Task<Result<Crafter?, Error>> GetByIDAsync(CrafterID id, CancellationToken cancellationToken)
	{
		var crafter = await dbContext.Crafters
			.Include(a => a.Artefacts)
			.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

		if (crafter == null) return Errors.NotFound(id.Value);
		return crafter;
	}

	public async Task<Result<CrafterDto?, Error>> GetByIDDtoAsync(Guid id, CancellationToken cancellationToken)
	{
		var crafter = await readDbContext.Crafters
			.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

		if (crafter == null) return Errors.NotFound(id);

		crafter.Artefacts = await readDbContext.MagicArtefacts
			.Where(a => a.CrafterId == id && !a.IsDeleted)
			.ToListAsync(cancellationToken);

		return crafter;
	}

	public async Task<Result<Crafter?, Error>> GetByNicknameAsync(Nickname nickname, CancellationToken cancellationToken)
	{
		var crafter = await dbContext.Crafters
			.Include(a => a.Artefacts)
			.FirstOrDefaultAsync(a => a.Nickname == nickname && !a.IsDeleted, cancellationToken);

		if (crafter == null) return Errors.NotFound("Nickname");
		return crafter;
	}

	private async Task DeleteCrafter(Crafter crafter, CancellationToken cancellationToken = default)
	{
		dbContext.Crafters.Remove(crafter);
		await dbContext.SaveChangesAsync(cancellationToken);
	}

	private async Task DeleteMagicArtefact(
		Crafter crafter,
		IEnumerable<MagicArtefact> artefacts,
		CancellationToken cancellationToken = default)
	{
		foreach (var artefact in artefacts)
		{
			crafter.RemoveArtefact(artefact);
			await messageQueue.WriteAsync([artefact.ImageFile.Name], cancellationToken);
		}

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteUnactiveEntitiesAsync()
	{
		var controlDeletionDate = DateTime.UtcNow.AddDays(-options.Value.DeletionPeriodDays);
		await DeleteUnactiveMagicArtefacts(controlDeletionDate);
		await DeleteUnactiveCrafters(controlDeletionDate);
	}

	private async Task DeleteUnactiveCrafters(DateTime controlDeletionDate)
	{
		var crafters = await dbContext.Crafters
			.Where(a => a.IsDeleted
			&& a.DeletionDate < controlDeletionDate)
			.ToListAsync();

		foreach (var item in crafters)
		{
			await DeleteCrafter(item);
		}
	}

	private async Task DeleteUnactiveMagicArtefacts(DateTime controlDeletionDate)
	{
		var removeDatas = await dbContext.Crafters
			.Select(a => new
			{
				Crafter = a,
				Artefacts = a.Artefacts
			.Where(b => b.IsDeleted && b.DeletionDate < controlDeletionDate)
			})
			.Where(a => a.Artefacts.Any())
			.ToListAsync();

		foreach (var data in removeDatas)
			await DeleteMagicArtefact(data.Crafter, data.Artefacts);
	}
}