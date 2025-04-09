namespace Dwarf_sMagicShop.Accounts.Domain.Settings;

public class JwtSettings
{
	public string Issuer { get; init; } = string.Empty;
	public string Audience { get; init; } = string.Empty;
	public string Key { get; init; } = string.Empty;
	public string ExpiredTimeMinutes { get; init; } = string.Empty;

	public static Exception GetException()
	{
		return new ApplicationException("JwtSettings configuration not found");
	}
}
