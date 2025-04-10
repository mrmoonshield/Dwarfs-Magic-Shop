using Dwarf_sMagicShop.Species.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Species.Infrastructure.DbContexts;

public class WriteDbContextSpecies(string connectionString) : DbContext
{
	public DbSet<ArtefactSpecies> ArtefactSpecies => Set<ArtefactSpecies>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(connectionString);
		optionsBuilder.UseSnakeCaseNamingConvention();
		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("species");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContextSpecies).Assembly,
			type => type.FullName?.Contains("Write") ?? false);
	}

	private ILoggerFactory CreateLoggerFactory()
	{
		return LoggerFactory.Create(builder => builder.AddConsole());
	}
}