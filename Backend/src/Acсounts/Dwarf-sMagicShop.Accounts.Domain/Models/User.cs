using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Domain.Models;

public class User : IdentityUser<Guid>
{
	public Role Role { get; set; } = default!;
	public CrafterAccount CreateCrafterAccount(Guid crafterId) => new CrafterAccount(this, crafterId);
}
