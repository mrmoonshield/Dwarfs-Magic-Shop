namespace Dwarf_sMagicShop.Core.ErrorsHelpers;

public record Error
{
	private const string SEPARATOR = "|";
	public string Code { get; }
	public string Message { get; }
	public ErrorType ErrorType { get; }
	public string? PropertyName { get; }

	private Error(string code, string message, ErrorType errorType, string? propertyName = null)
	{
		Code = code;
		Message = message;
		ErrorType = errorType;
		PropertyName = propertyName;
	}

	public static Error Empty() =>
		new Error("", "", ErrorType.Empty);

	public static Error Validation(string code, string message) =>
		new Error(code, message, ErrorType.Validation);

	public static Error NotFound(string code, string message) =>
		new Error(code, message, ErrorType.NotFound);

	public static Error Failure(string code, string message) =>
		new Error(code, message, ErrorType.Failure);

	public static Error Conflict(string code, string message) =>
		new Error(code, message, ErrorType.Conflict);

	public string Serialize()
	{
		return string.Join(SEPARATOR, Code, Message, ErrorType);
	}

	public static Error Deserialize(string serializedStr, string? propertyName = null)
	{
		var splitArr = serializedStr.Split(SEPARATOR);

		if (splitArr.Length < 3)
		{
			switch (splitArr.Length)
			{
				case 1:
					return new Error("value.invalid", serializedStr, ErrorType.Failure);

				default:
					throw new ArgumentException("Invalid serialized format");
			}
		}

		if (Enum.TryParse<ErrorType>(splitArr[2], out var errorType) == false)
			throw new ArgumentException("Invalid serialized format");

		return new Error(splitArr[0], splitArr[1], errorType, propertyName);
	}

	public ErrorsList ToErrorsList()
	{
		return new([this]);
	}
}

public enum ErrorType
{
	Empty, Validation, NotFound, Failure, Conflict
}