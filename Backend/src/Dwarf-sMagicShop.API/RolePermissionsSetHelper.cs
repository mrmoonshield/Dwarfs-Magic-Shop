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
				Permissions.READ_CRAFTER,
				Permissions.READ_MAGIC_ARTEFACT,
				Permissions.READ_ACCOUNT,
				Permissions.DELETE_ACCOUNT,
				Permissions.CREATE_SPECIES,
				Permissions.UPDATE_SPECIES,
				Permissions.READ_SPECIES,
				Permissions.DELETE_SPECIES,
			]));

		list.Add((Roles.CRAFTER,
			[
				Permissions.UPDATE_CRAFTER,
				Permissions.READ_CRAFTER,
				Permissions.DELETE_CRAFTER,
				Permissions.CREATE_MAGIC_ARTEFACT,
				Permissions.DELETE_MAGIC_ARTEFACT,
				Permissions.READ_MAGIC_ARTEFACT,
				Permissions.UPDATE_MAGIC_ARTEFACT,
				Permissions.READ_SPECIES,
			]));

		list.Add((Roles.USER,
			[
				Permissions.READ_CRAFTER,
				Permissions.CREATE_CRAFTER,
				Permissions.READ_MAGIC_ARTEFACT,
				Permissions.READ_ACCOUNT,
				Permissions.DELETE_ACCOUNT,
				Permissions.READ_SPECIES,
			]));

		return list;
	}
}
