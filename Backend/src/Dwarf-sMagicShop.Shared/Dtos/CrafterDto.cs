namespace Dwarf_sMagicShop.Core.Dtos;

public class CrafterDto
{
	public Guid Id { get; init; }
	public string Nickname { get; init; } = string.Empty;
	public int Experience { get; init; }
	public bool IsDeleted { get; init; }
	public DateTime DeletionDate { get; set; }
	public IReadOnlyCollection<SocialDto> Socials { get; init; } = [];
	public IReadOnlyCollection<MagicArtefactDto> Artefacts = [];
}