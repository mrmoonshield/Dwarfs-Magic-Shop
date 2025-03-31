using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Core.Abstractions;

public interface IResultHandler<TResultValue>
{
	Task<Result<TResultValue, ErrorsList>> ExecuteAsync(CancellationToken cancellationToken);
}

public interface IResultHandler<TResultValue, TCommand>
{
	Task<Result<TResultValue, ErrorsList>> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IResultHandler<TResultValue, TCommand, TCommand1>
{
	Task<Result<TResultValue, ErrorsList>> ExecuteAsync(
		TCommand command, TCommand1 command1, CancellationToken cancellationToken);
}

public interface IResultHandler<TResultValue, TCommand, TCommand1, TCommand2>
{
	Task<Result<TResultValue, ErrorsList>> ExecuteAsync(
		TCommand command, TCommand1 command1, TCommand2 command2, CancellationToken cancellationToken);
}

public interface IUnitResultHandler<TCommand>
{
	Task<UnitResult<ErrorsList>> ExecuteAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IUnitResultHandler<TCommand, TCommand1>
{
	Task<UnitResult<ErrorsList>> ExecuteAsync(
		TCommand command, TCommand1 command1, CancellationToken cancellationToken);
}

public interface IUnitResultHandler<TCommand, TCommand1, TCommand2>
{
	Task<UnitResult<ErrorsList>> ExecuteAsync(
		TCommand command, TCommand1 command1, TCommand2 command2, CancellationToken cancellationToken);
}