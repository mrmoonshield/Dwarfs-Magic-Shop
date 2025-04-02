namespace Dwarf_sMagicShop.Accounts.Domain.Models;

public class RolePermission
{
	public Guid RoleId { get; set; }
	public Role Role { get; set; } = default!;
	public Guid PermissionId { get; set; }
	public Permission Permission { get; set; } = default!;
}
