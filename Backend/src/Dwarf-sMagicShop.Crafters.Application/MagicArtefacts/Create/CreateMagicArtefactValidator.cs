using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;

public class CreateMagicArtefactValidator : AbstractValidator<CreateMagicArtefactCommand>, ICustomValidator
{
	public CreateMagicArtefactValidator()
	{
		RuleFor(a => a.UpdateRequest.Name).NotEmpty().MaximumLength(Constants.MAX_LOW_TEXT_LENGHT);
		RuleFor(a => a.UpdateRequest.Description).NotEmpty().MaximumLength(Constants.MAX_HIGH_TEXT_LENGHT);
	}
}