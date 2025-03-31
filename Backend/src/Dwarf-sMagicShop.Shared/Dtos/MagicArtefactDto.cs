using Dwarf_sMagicShop.Core.Enums;

namespace Dwarf_sMagicShop.Core.Dtos;

public class MagicArtefactDto
{
	public Guid Id { get; init; }
	public string Name { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public Guid SpeciesId { get; init; } = default!;
	public string Effect { get; init; } = string.Empty;
	public ArtefactRareType Rare { get; init; }
	public string Location { get; init; } = string.Empty;
	public float Weight { get; init; }
	public HandedAmountType HandedAmountType { get; init; }
	public ArtefactStatusType ArtefactStatusType { get; init; }
	public DateTime CreationDate { get; init; }
	public string ImageFile { get; init; } = string.Empty;
	public int Position { get; init; }
	public Guid CrafterId { get; init; }
	public bool IsDeleted { get; init; }
	public DateTime DeletionDate { get; set; }
}