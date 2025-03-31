namespace Dwarf_sMagicShop.Core.Extensions;

public static class StringContainsToLower
{
	public static bool ContainsToLower(this string str, string containsStr)
	{
		return str.ToLower().Contains(containsStr.ToLower());
	}
}