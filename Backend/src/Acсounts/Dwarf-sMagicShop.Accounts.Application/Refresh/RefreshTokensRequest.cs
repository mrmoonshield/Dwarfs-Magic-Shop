namespace Dwarf_sMagicShop.Accounts.Application.Refresh;

public record RefreshTokensRequest(string AccessToken, Guid RefreshToken);
