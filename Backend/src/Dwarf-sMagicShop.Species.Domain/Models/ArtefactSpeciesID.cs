namespace Dwarf_sMagicShop.Species.Domain.Models;

public record ArtefactSpeciesID
{
	private ArtefactSpeciesID(Guid guid)
	{
		Value = guid;
	}

	public Guid Value { get; }

	public static ArtefactSpeciesID NewArtefactSpeciesID => new(Guid.NewGuid());
	public static ArtefactSpeciesID EmptyArtefactSpeciesID => new(Guid.Empty);
	public static ArtefactSpeciesID Create(Guid id) => new(id);
}