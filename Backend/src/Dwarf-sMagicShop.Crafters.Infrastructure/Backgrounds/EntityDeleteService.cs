using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Infrastructure.SettingsModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Backgrounds;

public class EntityDeleteService : BackgroundService
{
	private readonly IServiceScopeFactory serviceScopeFactory;
	private readonly IOptions<SoftDeleteSettings> options;
	private readonly ILogger<EntityDeleteService> logger;

	public EntityDeleteService(IServiceScopeFactory serviceScopeFactory,
		IOptions<SoftDeleteSettings> options,
		ILogger<EntityDeleteService> logger)
	{
		this.serviceScopeFactory = serviceScopeFactory;
		this.options = options;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			await CheckEntity();
			await Task.Delay(TimeSpan.FromHours(options.Value.CheckInactiveEntitiesPeriodHours));
		}
	}

	private async Task CheckEntity()
	{
		using (var scope = serviceScopeFactory.CreateScope())
		{
			try
			{
				var crafterRepository = scope.ServiceProvider.GetRequiredService<ICrafterRepository>();
				await crafterRepository.DeleteUnactiveEntitiesAsync();
			}
			catch (Exception ex)
			{
				logger.LogError("Inactive entities cannot be deleted {ex}", ex);
			}
		}
	}
}