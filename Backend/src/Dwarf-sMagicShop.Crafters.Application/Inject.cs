using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Crafters.Application;

public static class Inject
{
	public static IServiceCollection AddApplicationCrafters(this IServiceCollection services)
	{
		return services
			.AddResultHandlers()
			.AddUnitResultHandlers()
			.AddQueryHandlers()
			.AddScoped<ExistingEntitiesValidators>()
			.AddScoped<ValidatorsProvider>()
			.AddValidatorsFromAssembly(typeof(Inject).Assembly);
	}

	public static IServiceCollection AddResultHandlers(this IServiceCollection services)
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

	public static IServiceCollection AddUnitResultHandlers(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IUnitResultHandler<>),
						typeof(IUnitResultHandler<,>),
						typeof(IUnitResultHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}

	public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IQueryHandler<,>),
						typeof(IQueryHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}
}