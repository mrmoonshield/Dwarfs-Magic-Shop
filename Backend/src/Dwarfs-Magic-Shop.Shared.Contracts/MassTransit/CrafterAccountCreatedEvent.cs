namespace Dwarfs_Magic_Shop.Shared.Contracts.MassTransit;

public record CrafterAccountCreatedEvent(Guid CrafterId, string UserName);
