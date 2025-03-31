using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Configurations.Read;

public class MagicArtefactDtoConfiguration : IEntityTypeConfiguration<MagicArtefactDto>
{
	public void Configure(EntityTypeBuilder<MagicArtefactDto> builder)
	{
		builder.ToTable("magic_artefacts");
		builder.HasKey(a => a.Id);
		builder.Property(a => a.Id);

		builder.Property(a => a.Rare)
			.HasConversion<string>();

		builder.Property(a => a.HandedAmountType)
			.HasConversion<string>();

		builder.Property(a => a.ArtefactStatusType)
			.HasConversion<string>();

		builder.Property(a => a.CreationDate)
			.HasConversion<DateTime>();

		builder.Property(a => a.CrafterId)
			.HasColumnName("crafter_id");

		builder.Property(a => a.IsDeleted)
			.HasColumnName("deleted");

		builder.Property(a => a.DeletionDate)
			.HasConversion<long>()
			.HasDefaultValue(DateTime.MinValue)
			.HasColumnName("deletion_date");
	}
}