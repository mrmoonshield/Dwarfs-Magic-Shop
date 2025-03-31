using Dwarf_sMagicShop.Core.Dtos;

namespace Dwarf_sMagicShop.Core.Database;

public interface IReadDbContextSpecies
{
	IQueryable<ArtefactSpeciesDto> ArtefactSpecies { get; }
}