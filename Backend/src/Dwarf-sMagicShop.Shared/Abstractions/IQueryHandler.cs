namespace Dwarf_sMagicShop.Core.Abstractions;

public interface IQueryHandler<TResult, TQueryCommand>
{
	Task<TResult> ExecuteAsync(TQueryCommand queryCommand, CancellationToken cancellationToken);
}

public interface IQueryHandler<TResult, TQueryCommand, TQueryCommand1>
{
	Task<TResult> ExecuteAsync(TQueryCommand queryCommand, TQueryCommand1 queryCommand1, CancellationToken cancellationToken);
}