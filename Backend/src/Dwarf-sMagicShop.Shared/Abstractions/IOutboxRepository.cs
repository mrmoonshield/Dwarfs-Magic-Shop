namespace Dwarf_sMagicShop.Core.Abstractions;

public interface IOutboxRepository
{
	Task AddAsync<TMessage>(TMessage message, CancellationToken cancellationToken);
}
