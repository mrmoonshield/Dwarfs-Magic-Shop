using Dwarf_sMagicShop.Accounts.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Dwarf_sMagicShop.Accounts.Application.RequirementsHandlers;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionAttribute permission)
	{
		var claim = context.User.Claims.FirstOrDefault(a => a.Type == "Permission");

		if (claim == null) return;

		if (claim.Value == permission.Code) 
			context.Succeed(permission);

		await Task.CompletedTask;
	}
}
