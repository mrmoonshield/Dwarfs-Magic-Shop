using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Configurations.Write;

public class CrafterConfiguration : IEntityTypeConfiguration<Crafter>
{
	public void Configure(EntityTypeBuilder<Crafter> builder)
	{
		builder.ToTable("crafters");
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(
				id => id.Value,
				value => CrafterID.Create(value));

		builder.Property(a => a.Nickname)
			.HasConversion(
				nickname => nickname.Value,
				value => Nickname.CreateFromDatabase(value))
			.IsRequired()
			.HasMaxLength(Core.Constants.MAX_LOW_TEXT_LENGHT);

		builder.Property(a => a.Experience);

		builder.HasMany(a => a.Artefacts)
			.WithOne()
			.HasForeignKey("crafter_id")
			.IsRequired(true);

		builder.Property(a => a.Socials)
			.HasConversion(
				socials => JsonSerializer.Serialize(socials, JsonSerializerOptions.Default),
				json => JsonSerializer.Deserialize<IReadOnlyCollection<Social>>(json, JsonSerializerOptions.Default)!)
			.IsRequired(false);

		builder.Property(a => a.IsDeleted)
			.HasColumnName("deleted");

		builder.Property(a => a.DeletionDate)
			.HasConversion<long>()
			.HasDefaultValue(DateTime.MinValue)
			.HasColumnName("deletion_date");
	}
}