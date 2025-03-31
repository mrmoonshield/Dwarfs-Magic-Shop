using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record Nickname
{
	public string Value { get; }

	private Nickname(string nickname)
	{
		Value = nickname;
	}

	public static Result<Nickname, Error> Create(string nickname)
	{
		if (string.IsNullOrWhiteSpace(nickname))
			return Errors.ValueIsEmpty("Nickname");
		return new Nickname(nickname);
	}

	public static Nickname CreateFromDatabase(string nickname) =>
		new Nickname(nickname);
}