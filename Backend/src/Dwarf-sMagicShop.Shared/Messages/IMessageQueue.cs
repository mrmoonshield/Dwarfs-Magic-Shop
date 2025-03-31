namespace Dwarf_sMagicShop.Core.Messages;

public interface IMessageQueue<TMessage>
{
	ValueTask<TMessage> ReadAsync(CancellationToken cancellationToken);

	ValueTask WriteAsync(TMessage[] messages, CancellationToken cancellationToken);
}