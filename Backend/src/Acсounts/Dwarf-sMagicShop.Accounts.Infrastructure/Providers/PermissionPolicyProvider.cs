using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Providers;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
	public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
	{
	}

	public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		return base.GetPolicyAsync(policyName);
	}
}
