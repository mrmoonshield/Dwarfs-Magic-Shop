using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace FileService.Endpoints;

public static class EndpointsExtensions
{
	public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
	{
		var serviceDescriptors = assembly.DefinedTypes
			.Where(a => !a.IsAbstract && !a.IsInterface && a.IsAssignableTo(typeof(IEndpoint)))
			.Select(a => ServiceDescriptor.Transient(typeof(IEndpoint), a))
			.ToArray();

		services.TryAddEnumerable(serviceDescriptors);
		return services;
	}

	public static IApplicationBuilder MapEndpoints(this WebApplication app)
	{
		var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

		foreach (var item in endpoints)
		{
			item.MapEndpoint(app);
		}

		return app;
	}
}
