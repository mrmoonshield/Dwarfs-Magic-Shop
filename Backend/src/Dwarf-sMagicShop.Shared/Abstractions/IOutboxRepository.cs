using Dwarf_sMagicShop.Core.Messages;

namespace Dwarf_sMagicShop.Core.Abstractions;

public interface IOutboxRepository
{
	Task AddAsync<TMessage>(TMessage message, CancellationToken cancellationToken);
	Task<IEnumerable<OutboxMessage>> GetMessagesAsync(CancellationToken cancellationToken);
}
