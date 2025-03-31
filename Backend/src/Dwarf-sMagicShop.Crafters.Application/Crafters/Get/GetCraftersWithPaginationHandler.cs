using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.Get;

public class GetCraftersWithPaginationHandler : IQueryHandler<PagedList<CrafterDto>, GetEntitiesWithPaginationQuery, GetFilteredMagicArtefactQuery>
{
	private readonly IReadDbContextCrafter readDbContext;

	public GetCraftersWithPaginationHandler(IReadDbContextCrafter readDbContext)
	{
		this.readDbContext = readDbContext;
	}

	public async Task<PagedList<CrafterDto>> ExecuteAsync(
		GetEntitiesWithPaginationQuery query,
		GetFilteredMagicArtefactQuery? filterQuery,
		CancellationToken cancellationToken)
	{
		var queryable = readDbContext.Crafters.Where(a => !a.IsDeleted);

		if (filterQuery != null)
		{
			var artefacts = await MagicArtefactFilter.Filter(filterQuery, readDbContext).ToListAsync(cancellationToken);
			var crafters = await queryable.ToListAsync(cancellationToken);
			var ienumerable = crafters.Where(c => artefacts.Any(a => a.CrafterId == c.Id));
			return ienumerable.ToPagedList(query.Page, query.PageSize);
		}
		else
		{
			return await queryable.ToPagedListAsync(
			query.Page, query.PageSize, cancellationToken);
		}
	}
}