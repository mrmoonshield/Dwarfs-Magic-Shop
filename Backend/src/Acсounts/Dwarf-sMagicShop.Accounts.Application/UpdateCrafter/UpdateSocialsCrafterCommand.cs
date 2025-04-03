namespace Dwarf_sMagicShop.Accounts.Application.UpdateCrafter;

public record UpdateSocialsCrafterCommand(Guid Id, IReadOnlyCollection<UpdateSocialsCrafterDto> Request);