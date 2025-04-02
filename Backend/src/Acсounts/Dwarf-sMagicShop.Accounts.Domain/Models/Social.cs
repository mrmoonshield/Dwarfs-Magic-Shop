using Dwarf_sMagicShop.Core.Enums;

namespace Dwarf_sMagicShop.Accounts.Domain.Models;

public class Social
{
	public SocialType SocialType { get; set; }
	public string Reference { get; set; } = string.Empty;
}