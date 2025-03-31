using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public class CrafterBalance : Core.Entities.Entity<CrafterBalanceID>
{
	public CrafterBalance(CrafterBalanceID id, CrafterID crafterId) : base(id)
	{
		CrafterId = crafterId;
	}

	public CrafterID CrafterId { get; private set; }
	public decimal Balance { get; private set; }

	public static Result<CrafterBalance, Error> Create(CrafterBalanceID id, CrafterID crafterId)
	{
		if (id == null)
			return Errors.ValueIsNull("ID");
		if (crafterId == null)
			return Errors.ValueIsNull("CrafterID");

		return new CrafterBalance(id, crafterId);
	}
}