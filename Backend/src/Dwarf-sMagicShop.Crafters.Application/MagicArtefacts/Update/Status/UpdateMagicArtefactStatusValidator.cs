using Dwarf_sMagicShop.Core.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Status;

public class UpdateMagicArtefactStatusValidator : AbstractValidator<UpdateMagicArtefactStatusRequest>, ICustomValidator
{
	public UpdateMagicArtefactStatusValidator()
	{
		RuleFor(a => a.ArtefactStatusType).IsInEnum();
	}
}