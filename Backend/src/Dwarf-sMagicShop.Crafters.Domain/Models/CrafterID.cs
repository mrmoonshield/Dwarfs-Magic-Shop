namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record CrafterID
{
	private CrafterID(Guid guid)
	{
		Value = guid;
	}

	public Guid Value { get; }

	public static CrafterID NewCrafterID => new(Guid.NewGuid());
	public static CrafterID EmptyCrafterID => new(Guid.Empty);
	public static CrafterID Create(Guid id) => new(id);
}