using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.Delete;

public class DeleteCrafterCommandValidator : AbstractValidator<DeleteCrafterCommand>
{
	public DeleteCrafterCommandValidator()
	{
		RuleFor(a => a.Id).NotEmpty();
	}
}