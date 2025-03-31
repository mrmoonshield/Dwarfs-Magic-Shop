using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record File
{
	private File(string name)
	{
		Name = name;
	}

	public string Name { get; }

	public static Result<File, Error> Create(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return Errors.ValueIsEmpty("Name");
		else return new File(name);
	}

	public static File CreateFileFromBD(string name) => new File(name);
}