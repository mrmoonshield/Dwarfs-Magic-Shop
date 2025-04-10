using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Species.Infrastructure.Configurations.Read;

public class ArtefactSpeciesDtoConfiguration : IEntityTypeConfiguration<ArtefactSpeciesDto>
{
	public void Configure(EntityTypeBuilder<ArtefactSpeciesDto> builder)
	{
		builder.ToTable("artefacts_species");
		builder.HasKey(a => a.Id);
	}
}