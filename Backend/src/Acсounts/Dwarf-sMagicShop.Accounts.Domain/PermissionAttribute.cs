using Microsoft.AspNetCore.Authorization;

namespace Dwarf_sMagicShop.Accounts.Domain;

public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
	public PermissionAttribute(string code) : base(code)
	{
		Code = code;
	}

	public string Code { get; }
}
