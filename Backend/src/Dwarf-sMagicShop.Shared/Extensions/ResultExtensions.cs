using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Core.Extensions;

public static class ResultExtensions
{
	public static ErrorsList ToErrorsList<T, E>(this Result<T, E> result) where E : Error
	{
		return new([result.Error]);
	}

	public static ErrorsList ToErrorsList<E>(this UnitResult<E> result) where E : Error
	{
		return new([result.Error]);
	}
}