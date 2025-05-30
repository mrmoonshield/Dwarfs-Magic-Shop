﻿using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Messages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Repositories;

public class OutboxRepository(AccountDbContext accountDbContext) : IOutboxRepository
{
	public async Task AddAsync<TMessage>(TMessage message, CancellationToken cancellationToken)
	{
		var outboxMessage = new OutboxMessage
		{
			Type = typeof(TMessage).FullName!,
			Data = JsonSerializer.Serialize(message),
			OccuredDate = DateTime.UtcNow,
		};

		await accountDbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
	}

	public async Task<IEnumerable<OutboxMessage>> GetMessagesAsync(CancellationToken cancellationToken)
	{
		var messages = await accountDbContext.OutboxMessages
			.Where(a => a.ProcessedDate == null)
			.OrderBy(a => a.OccuredDate)
			.Take(10)
			.ToListAsync(cancellationToken);

		return messages;
	}
}
