using Dwarf_sMagicShop.Accounts.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Dwarf_sMagicShop.Accounts.Infrastructure.Providers;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
	public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
	{
		var policy = new AuthorizationPolicyBuilder()
			.RequireAuthenticatedUser();

		return Task.FromResult(policy.Build());
	}

	public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
	{
		return Task.FromResult<AuthorizationPolicy?>(null);
	}

	public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
	{
		if (string.IsNullOrWhiteSpace(policyName))
			return Task.FromResult<AuthorizationPolicy?>(null);

		var policy = new AuthorizationPolicyBuilder()
			.AddRequirements(new PermissionAttribute(policyName));

		return Task.FromResult(policy?.Build());
	}
}
