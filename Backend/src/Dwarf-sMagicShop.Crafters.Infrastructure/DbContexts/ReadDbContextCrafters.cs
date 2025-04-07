using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;

public class ReadDbContextCrafters(string connectionString) : DbContext, IReadDbContextCrafter
{
	public IQueryable<CrafterDto> Crafters => Set<CrafterDto>();
	public IQueryable<MagicArtefactDto> MagicArtefacts => Set<MagicArtefactDto>();
	public IQueryable<ArtefactSpeciesDto> ArtefactSpecies => Set<ArtefactSpeciesDto>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(connectionString);
		optionsBuilder.UseSnakeCaseNamingConvention();
		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
		optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("crafters");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContextCrafters).Assembly,
			type => type.FullName?.Contains("Read") ?? false);
	}

	private ILoggerFactory CreateLoggerFactory()
	{
		return LoggerFactory.Create(builder => builder.AddConsole());
	}
}