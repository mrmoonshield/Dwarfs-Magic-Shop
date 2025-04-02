using Dwarf_sMagicShop.Accounts.Infrastructure;
using Dwarf_sMagicShop.Core;

namespace Dwarf_sMagicShop.API;

public class RolePermissionsSetHelper
{
	private readonly AccountsSeeder accountsSeeder;

	public RolePermissionsSetHelper(AccountsSeeder accountsSeeder)
	{
		this.accountsSeeder = accountsSeeder;
	}

	public void SetRolePermissions()
	{
		Task.Run(() => accountsSeeder.SeedAsync(GetRolePermissionsList()));
	}

	private List<(string role, string[] permissions)> GetRolePermissionsList()
	{
		List<(string role, string[] permissions)> list = [];

		list.Add((Roles.ADMIN,
			[
				Permissions.READ_CRAFTER
			]));

		list.Add((Roles.CRAFTER,
			[
				Permissions.UPDATE_CRAFTER,
				Permissions.READ_CRAFTER,
				Permissions.DELETE_CRAFTER
			]));

		list.Add((Roles.USER,
			[
				Permissions.READ_CRAFTER,
				Permissions.CREATE_CRAFTER
			]));

		return list;
	}
}
