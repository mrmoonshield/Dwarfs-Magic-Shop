using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.Dtos;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;

public class MagicArtefactFilter
{
	public static IQueryable<MagicArtefactDto> Filter(GetFilteredMagicArtefactQuery query, IReadDbContextCrafter readDbContext)
	{
		var artefactsQueryable = readDbContext.MagicArtefacts
			.Where(a => !a.IsDeleted);

		if (query.ContainsString != null)
		{
			artefactsQueryable = artefactsQueryable.Where(a =>
			a.Name.ToLower() == query.ContainsString.ToLower()
			|| a.Description.ToLower() == query.ContainsString.ToLower());
		}

		if (query.ContainsEffect != null)
		{
			artefactsQueryable = artefactsQueryable.Where(a => a.Effect.ToLower() == query.ContainsEffect.ToLower());
		}

		if (query.Location != null)
		{
			artefactsQueryable = artefactsQueryable.Where(a => a.Location.ToLower() == query.Location.ToLower());
		}

		if (query.ArtefactStatusType != null)
		{
			artefactsQueryable = artefactsQueryable.Where(a => a.ArtefactStatusType == query.ArtefactStatusType);
		}

		return artefactsQueryable;
	}
}