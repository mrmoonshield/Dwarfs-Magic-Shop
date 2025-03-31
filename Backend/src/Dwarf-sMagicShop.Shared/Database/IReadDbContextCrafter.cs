using Dwarf_sMagicShop.Core.Dtos;

namespace Dwarf_sMagicShop.Core.Database
{
	public interface IReadDbContextCrafter
	{
		IQueryable<CrafterDto> Crafters { get; }
		IQueryable<MagicArtefactDto> MagicArtefacts { get; }
		IQueryable<ArtefactSpeciesDto> ArtefactSpecies { get; }
	}
}