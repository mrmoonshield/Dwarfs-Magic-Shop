namespace Dwarf_sMagicShop.Accounts.Domain.Models;

public class CrafterAccount
{
	private List<Social> socials = [];

	private CrafterAccount()
	{
	}

	public CrafterAccount(User user, Guid crafterId)
	{
		User = user;
		CrafterId = crafterId;
	}

	public Guid Id { get; }
	public Guid UserId { get; }
	public User User { get; }
	public Guid CrafterId { get; }
	public IReadOnlyCollection<Social> Socials => socials;

	public void UpdateSocials(List<Social> socials)
	{
		this.socials = socials;
	}
}
