using Dwarf_sMagicShop.Core.Enums;

namespace Dwarf_sMagicShop.Core.Dtos;

public class SocialDto
{
	public SocialType SocialType { get; set; }
	public string Reference { get; set; } = string.Empty;
}