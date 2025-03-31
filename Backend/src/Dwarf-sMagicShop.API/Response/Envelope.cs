using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.API.Response;

public record Envelope
{
	public object? Result { get; }
	public DateTime? TimeCreated { get; }
	public ErrorsList? Errors { get; }

	private Envelope(object? result, ErrorsList? errors = null)
	{
		Result = result;
		Errors = errors;
		TimeCreated = DateTime.UtcNow;
	}

	public static Envelope Ok(object? result = null) => new(result);
	public static Envelope Error(ErrorsList errors) => new(null, errors);
}

public record ResponseError(string? ErrorCode, string ErrorMessage, string? InvalidField);