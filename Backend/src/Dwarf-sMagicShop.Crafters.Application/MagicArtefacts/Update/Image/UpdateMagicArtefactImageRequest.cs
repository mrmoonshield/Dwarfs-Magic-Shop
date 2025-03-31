using Microsoft.AspNetCore.Http;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Image;

public record UpdateMagicArtefactImageRequest(IFormFile File);