using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Configurations.Write;

public class CrafterBalanceConfiguration : IEntityTypeConfiguration<CrafterBalance>
{
	public void Configure(EntityTypeBuilder<CrafterBalance> builder)
	{
		builder.ToTable("crafter_balance");
		builder.HasKey(a => a.Id);

		builder.Property(a => a.Id)
			.HasConversion(
			id => id.Value,
			value => CrafterBalanceID.Create(value));

		builder.Property(a => a.CrafterId)
			.HasConversion(
			id => id.Value,
			value => CrafterID.Create(value));

		builder.Property(a => a.Balance);
	}
}