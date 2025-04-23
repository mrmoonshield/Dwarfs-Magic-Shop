using Dwarf_sMagicShop.Crafters.Domain.Models;
using Dwarfs_Magic_Shop.Shared.Contracts.MassTransit;
using MassTransit;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;

public class CreateCrafterHandler(ICrafterRepository crafterRepository)
	: IConsumer<CrafterAccountCreatedEvent>
{
	public async Task Consume(ConsumeContext<CrafterAccountCreatedEvent> context)
	{
		var request = context.Message;
		var nicknameResult = Nickname.Create(request.UserName);
		var crafterID = CrafterID.Create(request.CrafterId);
		var crafterResult = Crafter.Create(crafterID, nicknameResult.Value);
		await crafterRepository.AddAsync(crafterResult.Value, context.CancellationToken);
	}
}

public class CreateCrafterHandlerDefinition: ConsumerDefinition<CreateCrafterHandler>
{
	protected override void ConfigureConsumer(
		IReceiveEndpointConfigurator endpointConfigurator, 
		IConsumerConfigurator<CreateCrafterHandler> consumerConfigurator, 
		IRegistrationContext context)
	{
		endpointConfigurator.UseMessageRetry(a =>
		{
			a.Incremental(5, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
		});
	}
}