using Dwarf_sMagicShop.Species.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Species.Infrastructure.Configurations.Write;

public class ArtefactSpeciesConfiguration : IEntityTypeConfiguration<ArtefactSpecies>
{
	public void Configure(EntityTypeBuilder<ArtefactSpecies> builder)
	{
		builder.ToTable("artefacts_species");
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(
			id => id.Value,
			value => ArtefactSpeciesID.Create(value));

		builder.Property(a => a.Name)
			.IsRequired()
			.HasMaxLength(Core.Constants.MAX_LOW_TEXT_LENGHT);
	}
}