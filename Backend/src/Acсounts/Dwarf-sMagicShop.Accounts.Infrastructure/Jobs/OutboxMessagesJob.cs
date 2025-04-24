using Quartz;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class OutboxMessagesJob : IJob
{
	public Task Execute(IJobExecutionContext context)
	{
		throw new NotImplementedException();
	}
}
