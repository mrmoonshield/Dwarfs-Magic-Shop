using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;

public class UpdateMainInfoCrafterCommandValidator : AbstractValidator<UpdateMainInfoCrafterCommand>
{
	public UpdateMainInfoCrafterCommandValidator()
	{
		RuleFor(a => a.Request.Nickname).MustBeValueObject(Nickname.Create);
		RuleFor(a => a.Request.Experience).GreaterThanOrEqualTo(0);
	}
}