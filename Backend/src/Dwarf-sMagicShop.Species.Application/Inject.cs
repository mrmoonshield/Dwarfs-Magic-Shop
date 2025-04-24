using Dwarf_sMagicShop.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Species.Application;

public static class Inject
{
	public static IServiceCollection AddApplicationSpecies(this IServiceCollection services)
	{
		return services
			.AddResultHandlersSpecies()
			.AddUnitResultHandlersSpecies()
			.AddQueryHandlersSpecies()
			.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Inject)));
	}

	public static IServiceCollection AddResultHandlersSpecies(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IResultHandler<>),
						typeof(IResultHandler<,>),
						typeof(IResultHandler<,,>),
						typeof(IResultHandler<,,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}

	public static IServiceCollection AddUnitResultHandlersSpecies(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IUnitResultHandler<>),
						typeof(IUnitResultHandler<,>),
						typeof(IUnitResultHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}

	public static IServiceCollection AddQueryHandlersSpecies(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IQueryHandler<,>),
						typeof(IQueryHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}
}