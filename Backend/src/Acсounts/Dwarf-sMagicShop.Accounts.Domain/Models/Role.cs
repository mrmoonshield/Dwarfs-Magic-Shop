using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Domain.Models;

public class Role : IdentityRole<Guid>
{
	public List<RolePermission> RolePermissions { get; set; } = [];
}
