using Dwarf_sMagicShop.Crafters.Domain.Models;
using Dwarf_sMagicShop.Species.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;

public class WriteDbContextCrafters(string connectionString) : DbContext
{
	public DbSet<Crafter> Crafters => Set<Crafter>();
	public DbSet<ArtefactSpecies> ArtefactSpecies => Set<ArtefactSpecies>();

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseNpgsql(connectionString);
		optionsBuilder.UseSnakeCaseNamingConvention();
		optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContextCrafters).Assembly,
			type => type.FullName?.Contains("Write") ?? false);
	}

	private ILoggerFactory CreateLoggerFactory()
	{
		return LoggerFactory.Create(builder => builder.AddConsole());
	}
}