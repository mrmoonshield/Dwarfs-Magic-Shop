using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;

public class UpdateSocialsCrafterCommandValidator : AbstractValidator<UpdateSocialsCrafterCommand>
{
	public UpdateSocialsCrafterCommandValidator()
	{
		RuleForEach(a => a.Request).MustBeValueObject(b => Social.Create(b.SocialType, b.Reference));
	}
}