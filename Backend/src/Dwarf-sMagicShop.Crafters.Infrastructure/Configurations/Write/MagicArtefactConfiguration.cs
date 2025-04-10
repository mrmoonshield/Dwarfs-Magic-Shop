using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Configurations.Write;

public class MagicArtefactConfiguration : IEntityTypeConfiguration<MagicArtefact>
{
	public void Configure(EntityTypeBuilder<MagicArtefact> builder)
	{
		builder.ToTable("magic_artefacts");
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(
			id => id.Value,
			value => MagicArtefactID.Create(value));

		builder.Property(a => a.Name)
			.IsRequired()
			.HasMaxLength(Core.Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(a => a.Description)
			.IsRequired()
			.HasMaxLength(Core.Constants.MAX_HIGH_TEXT_LENGHT);

		builder.Property(a => a.SpeciesId)
			.IsRequired(false);

		builder.Property(a => a.Effect).IsRequired(false);

		builder.Property(a => a.Rare)
			.HasConversion<string>();

		builder.Property(a => a.Location).IsRequired(false);

		builder.Property(a => a.Weight);

		builder.Property(a => a.HandedAmountType)
			.HasConversion<string>();

		builder.Property(a => a.ArtefactStatusType)
			.HasConversion<string>();

		builder.Property(a => a.CreationDate)
			.HasConversion<DateTime>();

		builder.Property(a => a.ImageFile)
			.HasConversion(
			file => file.Name,
			path => Domain.Models.File.CreateFileFromBD(path))
			.HasMaxLength(Core.Constants.MAX_LOW_TEXT_LENGHT)
			.IsRequired(false);

		builder.Property(a => a.Position)
			.HasConversion(
			num => num.Value,
			value => Position.CreateFromDatabase(value));

		builder.Property(a => a.IsDeleted)
			.HasColumnName("deleted");

		builder.Property(a => a.DeletionDate)
			.HasConversion<long>()
			.HasDefaultValue(DateTime.MinValue)
			.HasColumnName("deletion_date");
	}
}