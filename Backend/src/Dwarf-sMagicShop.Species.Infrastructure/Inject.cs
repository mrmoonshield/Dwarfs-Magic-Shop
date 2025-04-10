using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies;
using Dwarf_sMagicShop.Species.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Species.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Species.Infrastructure;

public static class Inject
{
	public static IServiceCollection AddInfrastructureSpecies(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddScoped(a => new WriteDbContextSpecies(configuration.GetConnectionString(Constants.DATABASE)!))
			.AddScoped<IReadDbContextSpecies>(a => new ReadDbContextSpecies(
				configuration.GetConnectionString(Constants.DATABASE)!))
			.AddScoped<ISpeciesRepository, SpeciesRepository>();

		return services;
	}
}