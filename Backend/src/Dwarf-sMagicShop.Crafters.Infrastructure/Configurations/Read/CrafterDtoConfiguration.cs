using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Configurations.Read
{
	public class CrafterDtoConfiguration : IEntityTypeConfiguration<CrafterDto>
	{
		public void Configure(EntityTypeBuilder<CrafterDto> builder)
		{
			builder.ToTable("crafters");
			builder.HasKey(a => a.Id);

			builder.Property(a => a.IsDeleted)
			.HasColumnName("deleted");

			builder.Property(a => a.DeletionDate)
				.HasConversion<long>()
				.HasDefaultValue(DateTime.MinValue)
				.HasColumnName("deletion_date");
		}
	}
}