using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Providers;

public class DatabaseTransactionProvider : IDatabaseTransactionProvider
{
	private readonly WriteDbContextCrafters dbContext;

	public DatabaseTransactionProvider(WriteDbContextCrafters dbContext)
	{
		this.dbContext = dbContext;
	}

	public async Task<IDbContextTransaction> StartTransactionAsync(CancellationToken cancellationToken)
	{
		return await dbContext.Database.BeginTransactionAsync(cancellationToken);
	}

	public async Task SaveAsync(CancellationToken cancellationToken)
	{
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}