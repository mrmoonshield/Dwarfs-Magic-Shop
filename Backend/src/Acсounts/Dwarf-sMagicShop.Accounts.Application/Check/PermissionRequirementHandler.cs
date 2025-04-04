using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Attributes;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Accounts.Application.Check;

public class PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionAttribute>
{
	protected override async Task<IEnumerable<AuthorizationFailureReason>> HandleRequirementAsync(
		AuthorizationHandlerContext context,
		PermissionAttribute permission)
	{
		using var scope = serviceScopeFactory.CreateAsyncScope();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
		var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepository>();

		var roleClaim = context.User.Claims.FirstOrDefault(a => a.Type == CustomClaims.ROLE);
		var idClaim = context.User.Claims.FirstOrDefault(a => a.Type == CustomClaims.USER_ID);

		if (roleClaim == null || idClaim == null)
			return context.FailureReasons;

		var user = await userManager.FindByIdAsync(idClaim.Value);

		if (user == null)
			return context.FailureReasons;

		var userPermissions = await accountRepository.GetPermissionsAsync(user, CancellationToken.None);

		if (userPermissions.Any(a => a.Code == permission.Code))
		{
			context.Succeed(permission);
		}

		return new List<AuthorizationFailureReason>();
	}
}
