using Dwarf_sMagicShop.Accounts.Domain.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Dwarf_sMagicShop.Accounts.Application.RequirementsHandlers;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
	protected override async Task HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionRequirement requirement)
	{
		var permission = context.User.Claims.FirstOrDefault(a => a.Type == "Permission");

		if (permission == null) return;

		if (permission.Value == requirement.Code) 
			context.Succeed(requirement);

		await Task.CompletedTask;
	}
}
