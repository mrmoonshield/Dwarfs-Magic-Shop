using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Messages;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Dwarf_sMagicShop.Crafters.Infrastructure;
using Dwarf_sMagicShop.Crafters.Infrastructure.Backgrounds;
using Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Crafters.Infrastructure.MessageQueues;
using Dwarf_sMagicShop.Crafters.Infrastructure.Providers;
using Dwarf_sMagicShop.Crafters.Infrastructure.Repositories;
using Dwarf_sMagicShop.Crafters.Infrastructure.SettingsModels;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Dwarf_sMagicShop.Crafters.Infrastructure;

public static class Inject
{
	public static IServiceCollection AddInfrastructureCrafters(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddScoped(a => new WriteDbContextCrafters(configuration.GetConnectionString(Constants.DATABASE)!))
			.AddScoped<IReadDbContextCrafter, ReadDbContextCrafters>(a => new ReadDbContextCrafters(
				configuration.GetConnectionString(Constants.DATABASE)!))
			.AddScoped<ICrafterRepository, CrafterRepository>()
			.AddHostedService<EntityDeleteService>()
			.AddHostedService<FileCleanerService>()
			.AddScoped<SoftDeleteSettings>()
			.AddScoped<IFileProvider, MinioProvider>()
			.AddScoped<IDatabaseTransactionProvider, DatabaseTransactionProvider>()
			.AddSingleton<IMessageQueue<string>, InMemoryMessageQueue<string>>()
			.AddMassTransitConfiguration(configuration);

		return services;
	}

	private static IServiceCollection AddMinio(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddMinio(options =>
		{
			var minioSettings = configuration.GetSection(nameof(MinioSettings)).Get<MinioSettings>()
			?? throw MinioSettings.GetException();

			options.WithEndpoint(minioSettings!.Endpoint);
			options.WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey);
			options.WithSSL(minioSettings.WithSsl);
		});

		return services;
	}

	public static WebApplicationBuilder AddInfrastructureCraftersBuilder(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<SoftDeleteSettings>(
			builder.Configuration.GetSection(nameof(SoftDeleteSettings)));

		builder.Services.Configure<MinioSettings>(
			builder.Configuration.GetSection(nameof(MinioSettings)));

		builder.Services.AddMinio(builder.Configuration);
		return builder;
	}

	private static IServiceCollection AddMassTransitConfiguration(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		return services.AddMassTransit(a =>
		{
			a.SetKebabCaseEndpointNameFormatter();

			a.UsingRabbitMq((context, cfg) =>
			{
				cfg.Host(new Uri(configuration["RabbitMQ:Host"]!), hconfig =>
				{
					hconfig.Username(configuration["RabbitMQ:Username"]!);
					hconfig.Password(configuration["RabbitMQ:Password"]!);
				});

				cfg.Durable = true;
				cfg.ConfigureEndpoints(context);
			});
		});
	}
}