using Dwarf_sMagicShop.Core.Enums;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;

public record GetFilteredMagicArtefactQuery(
	string? ContainsString,
	string? ContainsEffect,
	string? Location,
	ArtefactStatusType? ArtefactStatusType);