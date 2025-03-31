using Dwarf_sMagicShop.Core.Shared;
using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.Core.Extensions;

public static class QueriesExtensions
{
	public static async Task<PagedList<T>> ToPagedListAsync<T>(
		this IQueryable<T> queryable,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		var count = await queryable.CountAsync(cancellationToken);

		var items = await queryable
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync(cancellationToken);

		var pagedList = new PagedList<T>
		{
			Items = items,
			Page = page,
			PageSize = pageSize,
			Count = (int)MathF.Ceiling(count / (float)items.Count)
		};

		return pagedList;
	}

	public static PagedList<T> ToPagedList<T>(
		this IEnumerable<T> ienumerable,
		int page,
		int pageSize)
	{
		var count = ienumerable.Count();

		var items = ienumerable
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.ToList();

		var pagedList = new PagedList<T>
		{
			Items = items,
			Page = page,
			PageSize = pageSize,
			Count = (int)MathF.Ceiling(count / (float)items.Count)
		};

		return pagedList;
	}
}