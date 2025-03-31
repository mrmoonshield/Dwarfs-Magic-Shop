using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

namespace Dwarf_sMagicShop.API.Controllers.Crafters;

public record CreateMagicArtefactRequest(
	IFormFile File,
	UpdateMagicArtefactRequest UpdateRequest);