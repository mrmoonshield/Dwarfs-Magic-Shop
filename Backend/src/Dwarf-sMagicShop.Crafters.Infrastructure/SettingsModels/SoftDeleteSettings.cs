namespace Dwarf_sMagicShop.Crafters.Infrastructure.SettingsModels;

public class SoftDeleteSettings
{
	public int CheckInactiveEntitiesPeriodHours { get; init; }
	public int DeletionPeriodDays { get; init; }

	public static Exception GetException()
	{
		return new ApplicationException("SoftDeleteSettings configuration not found");
	}
}