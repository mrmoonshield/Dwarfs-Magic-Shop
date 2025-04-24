using Dwarf_sMagicShop.Core.Abstractions;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Outbox;

public class OutboxMessagesService(IOutboxRepository outboxRepository)
{
	public async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		var messages = await outboxRepository.GetMessagesAsync(cancellationToken);
		if (!messages.Any()) return;
	}
}
