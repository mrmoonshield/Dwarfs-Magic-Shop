using Microsoft.AspNetCore.Authorization;

namespace Dwarf_sMagicShop.Accounts.Domain.Requirements;

public class PermissionRequirement : AuthorizeAttribute, IAuthorizationRequirement
{
	public PermissionRequirement(string code)
	{
		Code = code;
	}

	public string Code { get; }
}
