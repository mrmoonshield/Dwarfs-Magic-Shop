using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Species.Infrastructure.DbContexts;

public class ReadDbContextSpecies(string connectionString) : DbContext, IReadDbContextSpecies
{
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
		modelBuilder.HasDefaultSchema("species");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContextSpecies).Assembly,
			type => type.FullName?.Contains("Read") ?? false);
	}

	private ILoggerFactory CreateLoggerFactory()
	{
		return LoggerFactory.Create(builder => builder.AddConsole());
	}
}