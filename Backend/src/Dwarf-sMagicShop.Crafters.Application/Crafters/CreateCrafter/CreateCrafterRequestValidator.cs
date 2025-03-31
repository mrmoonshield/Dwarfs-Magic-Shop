using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;

public class CreateCrafterRequestValidator : AbstractValidator<CreateCrafterRequest>
{
	public CreateCrafterRequestValidator()
	{
		RuleFor(a => a.nickname).MustBeValueObject(Nickname.Create);
	}
}