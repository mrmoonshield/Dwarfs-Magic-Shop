using Dwarf_sMagicShop.Accounts.Application.Validators;
using Dwarf_sMagicShop.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Species.Application;

public static class Inject
{
	public static IServiceCollection AddApplicationAccounts(this IServiceCollection services)
	{
		return services
			.AddResultHandlersAccounts()
			.AddUnitResultHandlersAccounts()
			.AddQueryHandlersAccounts()
			.AddScoped<ExistingAccountValidator>();
	}

	public static IServiceCollection AddResultHandlersAccounts(this IServiceCollection services)
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

	public static IServiceCollection AddUnitResultHandlersAccounts(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IUnitResultHandler<>),
						typeof(IUnitResultHandler<,>),
						typeof(IUnitResultHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}

	public static IServiceCollection AddQueryHandlersAccounts(this IServiceCollection services)
	{
		return services.Scan(a => a.FromAssemblies(typeof(Inject).Assembly)
					.AddClasses(b => b.AssignableToAny(
						typeof(IQueryHandler<,>),
						typeof(IQueryHandler<,,>)))
					.AsSelfWithInterfaces()
					.WithScopedLifetime());
	}
}