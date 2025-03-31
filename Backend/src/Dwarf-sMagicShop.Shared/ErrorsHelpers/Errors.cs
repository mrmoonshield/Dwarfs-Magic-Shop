namespace Dwarf_sMagicShop.Core.ErrorsHelpers;

public static class Errors
{
	public static Error ValueIsInvalid(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.Validation("value.invalid", $"{str} is invalid");
	}

	public static Error NotFound(Guid? id)
	{
		var str = id.ToString() ?? "id";
		return Error.NotFound("id.not.found", $"{str} not found");
	}

	public static Error NotFound(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.NotFound("value.not.found", $"{str} not found");
	}

	public static Error ValueIsRequared(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.Validation("value.requared", $"{str} is requared");
	}

	public static Error ValueIsEmpty(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.Validation("value.empty", $"{str} can not be empty");
	}

	public static Error ValueIsNull(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.Validation("value.null", $"{str} can not be null");
	}

	public static Error ValueIsAlreadyExist(string? keyStringPart)
	{
		var str = keyStringPart ?? "value";
		return Error.Validation("value.already.exist", $"{str} is already exist");
	}

	public static Error InvalidOperation(string message)
	{
		return Error.Failure("invalid.operation", message);
	}
}