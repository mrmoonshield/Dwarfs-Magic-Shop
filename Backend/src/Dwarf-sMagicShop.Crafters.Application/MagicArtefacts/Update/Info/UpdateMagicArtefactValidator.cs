using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

public class UpdateMagicArtefactValidator : AbstractValidator<UpdateMagicArtefactRequest>, ICustomValidator
{
	public UpdateMagicArtefactValidator()
	{
		RuleFor(a => a.Species).CheckNullableValueIfValueNotNull(
			(a) => a?.Length <= Constants.MAX_LOW_TEXT_LENGHT,
			Errors.InvalidOperation($"Species length must be less or equal {Constants.MAX_LOW_TEXT_LENGHT}"));

		RuleFor(a => a.Effect).CheckNullableValueIfValueNotNull(
			(a) => a?.Length <= Constants.MAX_HIGH_TEXT_LENGHT,
			Errors.InvalidOperation($"Effect length must be less or equal {Constants.MAX_HIGH_TEXT_LENGHT}"));

		RuleFor(a => a.Location).CheckNullableValueIfValueNotNull(
			(a) => a?.Length <= Constants.MAX_LOW_TEXT_LENGHT,
			Errors.InvalidOperation($"Location length must be less or equal {Constants.MAX_LOW_TEXT_LENGHT}"));

		RuleFor(a => a.Weight).GreaterThanOrEqualTo(0);
		RuleFor(a => a.RareType).IsInEnum();
		RuleFor(a => a.HandedAmountType).IsInEnum();

		RuleFor(a => a.Name).NotEmpty().MaximumLength(Constants.MAX_LOW_TEXT_LENGHT);
		RuleFor(a => a.Description).NotEmpty().MaximumLength(Constants.MAX_HIGH_TEXT_LENGHT);
	}
}