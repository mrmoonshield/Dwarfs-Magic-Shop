using Dwarf_sMagicShop.Core.Messages;
using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Backgrounds;

public class FileCleanerService : BackgroundService
{
	private readonly IMessageQueue<string> messageQueue;
	private readonly IServiceScopeFactory serviceScopeFactory;

	public FileCleanerService(
		IMessageQueue<string> messageQueue,
		IServiceScopeFactory serviceScopeFactory)
	{
		this.messageQueue = messageQueue;
		this.serviceScopeFactory = serviceScopeFactory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using var scope = serviceScopeFactory.CreateScope();
		var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

		while (!stoppingToken.IsCancellationRequested)
		{
			var name = await messageQueue.ReadAsync(stoppingToken);
			await fileProvider.DeleteFileAsync(name, Core.Constants.BUCKET_NAME_IMAGES, stoppingToken);
		}
	}
}