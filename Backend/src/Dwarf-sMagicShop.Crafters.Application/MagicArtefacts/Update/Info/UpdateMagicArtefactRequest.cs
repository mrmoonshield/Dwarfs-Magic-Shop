using Dwarf_sMagicShop.Core.Enums;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

public record UpdateMagicArtefactRequest(
	string Name,
	string Description,
	string? Species,
	string? Effect,
	ArtefactRareType? RareType,
	string? Location,
	float? Weight,
	HandedAmountType? HandedAmountType);