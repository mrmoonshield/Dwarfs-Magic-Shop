namespace Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;

public record UpdateSocialsCrafterCommand(Guid Id, IReadOnlyCollection<UpdateSocialsCrafterDto> Request);