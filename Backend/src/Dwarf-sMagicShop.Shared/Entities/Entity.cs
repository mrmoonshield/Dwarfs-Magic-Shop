namespace Dwarf_sMagicShop.Core.Entities;

public abstract class Entity<TId> where TId : notnull
{
	protected Entity()
	{ }

	protected Entity(TId id) => Id = id;

	public TId Id { get; private set; } = default!;
}