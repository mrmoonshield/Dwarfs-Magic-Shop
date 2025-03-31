namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record CrafterBalanceID
{
	private CrafterBalanceID(Guid guid)
	{
		Value = guid;
	}

	public Guid Value { get; }

	public static CrafterBalanceID NewCrafterBalanceID => new(Guid.NewGuid());
	public static CrafterBalanceID EmptyCrafterBalanceID => new(Guid.Empty);
	public static CrafterBalanceID Create(Guid id) => new(id);
}