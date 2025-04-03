using Dwarf_sMagicShop.Core.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Accounts.Application.UpdateCrafter;

public class UpdateSocialsCrafterCommandValidator : AbstractValidator<UpdateSocialsCrafterCommand>
{
	public UpdateSocialsCrafterCommandValidator()
	{
		//RuleForEach(a => a.Request).MustBeValueObject(b => Social.Create(b.SocialType, b.Reference));
	}
}