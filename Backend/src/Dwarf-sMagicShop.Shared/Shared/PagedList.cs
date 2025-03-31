namespace Dwarf_sMagicShop.Core.Shared;

public class PagedList<T>
{
	public IReadOnlyList<T> Items { get; init; } = [];
	public int Count { get; init; }
	public int PageSize { get; init; }
	public int Page { get; init; }
	public bool HasNextPage => Page * PageSize < Count;
	public bool HasPreviousPage => Page > 1;
}