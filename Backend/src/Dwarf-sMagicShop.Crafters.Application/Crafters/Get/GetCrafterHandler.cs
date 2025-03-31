using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.Get;

public class GetCrafterHandler : IQueryHandler<Result<CrafterDto, ErrorsList>, Guid>
{
	private readonly IReadDbContextCrafter readDbContext;

	public GetCrafterHandler(IReadDbContextCrafter readDbContext)
	{
		this.readDbContext = readDbContext;
	}

	public async Task<Result<CrafterDto, ErrorsList>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
	{
		var crafter = await readDbContext.Crafters
			.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

		if (crafter == null)
			return Errors.NotFound(id).ToErrorsList();

		var artefacts = await readDbContext.MagicArtefacts
			.Where(a => a.CrafterId == id)
			.ToListAsync(cancellationToken);

		crafter.Artefacts = artefacts;

		return crafter;
	}
}