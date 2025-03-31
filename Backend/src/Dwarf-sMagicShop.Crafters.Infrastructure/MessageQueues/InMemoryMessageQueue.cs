using Dwarf_sMagicShop.Core.Messages;
using System.Threading.Channels;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.MessageQueues;

public class InMemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
{
	private readonly Channel<TMessage> channel = Channel.CreateUnbounded<TMessage>();

	public async ValueTask WriteAsync(TMessage[] messages, CancellationToken cancellationToken)
	{
		foreach (var message in messages)
			await channel.Writer.WriteAsync(message, cancellationToken);
	}

	public async ValueTask<TMessage> ReadAsync(CancellationToken cancellationToken)
	{
		return await channel.Reader.ReadAsync(cancellationToken);
	}
}