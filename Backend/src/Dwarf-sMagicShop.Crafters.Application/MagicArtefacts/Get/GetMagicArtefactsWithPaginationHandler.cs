using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Get;

public class GetMagicArtefactsWithPaginationHandler : IResultHandler<
	PagedList<MagicArtefactDto>,
	GetEntitiesWithPaginationQuery,
	GetFilteredMagicArtefactQuery>
{
	private readonly IReadDbContextCrafter readDbContext;

	public GetMagicArtefactsWithPaginationHandler(IReadDbContextCrafter readDbContext)
	{
		this.readDbContext = readDbContext;
	}

	public async Task<Result<PagedList<MagicArtefactDto>, ErrorsList>> ExecuteAsync(
		GetEntitiesWithPaginationQuery query,
		GetFilteredMagicArtefactQuery? filterQuery,
		CancellationToken cancellationToken)
	{
		var queryable = readDbContext.MagicArtefacts.Where(a => !a.IsDeleted);

		if (filterQuery != null)
		{
			var artefacts = MagicArtefactFilter.Filter(filterQuery, readDbContext);
			return await artefacts.ToPagedListAsync(query.Page, query.PageSize, cancellationToken);
		}
		else
		{
			return await queryable.ToPagedListAsync(
			query.Page, query.PageSize, cancellationToken);
		}
	}
}