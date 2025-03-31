using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record Position
{
	private Position(int value)
	{
		Value = value;
	}

	public int Value { get; }

	public static Result<Position, Error> Create(int position)
	{
		if (position < 1)
			return Errors.ValueIsInvalid("Position number");

		return new Position(position);
	}

	public static Position CreateFromDatabase(int value) => new Position(value);
}