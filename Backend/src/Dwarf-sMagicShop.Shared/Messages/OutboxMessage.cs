namespace Dwarf_sMagicShop.Core.Messages;

public sealed class OutboxMessage
{
	public Guid Id { get; set; }
	public required string Type { get; set; } = string.Empty;
	public required string Data { get; set; } = string.Empty;
	public required DateTime OccuredDate { get; set; }
	public DateTime? ProcessedDate { get; set; }
	public string Error { get; set; } = string.Empty;
}
