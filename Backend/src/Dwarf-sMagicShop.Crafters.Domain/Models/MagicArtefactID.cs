namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record MagicArtefactID
{
	private MagicArtefactID(Guid guid)
	{
		Value = guid;
	}

	public Guid Value { get; }

	public static MagicArtefactID NewMagicArtefactID => new(Guid.NewGuid());
	public static MagicArtefactID EmptyMagicArtefactID => new(Guid.Empty);
	public static MagicArtefactID Create(Guid id) => new(id);
}