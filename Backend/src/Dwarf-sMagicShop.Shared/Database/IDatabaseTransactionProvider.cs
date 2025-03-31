using Microsoft.EntityFrameworkCore.Storage;

namespace Dwarf_sMagicShop.Core.Database
{
	public interface IDatabaseTransactionProvider
	{
		Task SaveAsync(CancellationToken cancellationToken);

		Task<IDbContextTransaction> StartTransactionAsync(CancellationToken cancellationToken);
	}
}