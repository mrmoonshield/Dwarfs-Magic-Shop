using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;

public record CreateMagicArtefactCommand(
	Guid crafterId,
	UpdateMagicArtefactRequest UpdateRequest);