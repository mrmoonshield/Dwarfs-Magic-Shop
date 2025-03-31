namespace Dwarf_sMagicShop.Core.Entities;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
	public bool IsDeleted { get; private set; }
	public DateTime DeletionDate { get; private set; }

	protected SoftDeletableEntity()
	{ }

	protected SoftDeletableEntity(TId id) : base(id)
	{
	}

	public virtual void Delete()
	{
		IsDeleted = true;
		DeletionDate = DateTime.UtcNow;
	}

	public virtual void Restore()
	{
		IsDeleted = false;
	}
}