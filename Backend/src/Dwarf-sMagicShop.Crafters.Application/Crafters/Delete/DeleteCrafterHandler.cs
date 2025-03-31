using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.Delete;

public class DeleteCrafterHandler : IResultHandler<Guid, DeleteCrafterCommand>
{
	private readonly ICrafterRepository crafterRepository;
	private readonly IValidator<DeleteCrafterCommand> validator;
	private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public DeleteCrafterHandler(
		ICrafterRepository crafterRepository,
		IValidator<DeleteCrafterCommand> validator,
		ExistingEntitiesValidators existingEntitiesValidators)
	{
		this.crafterRepository = crafterRepository;
		this.validator = validator;
		this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<Result<Guid, ErrorsList>> ExecuteAsync(DeleteCrafterCommand command, CancellationToken cancellationToken)
	{
		var validationResult = validator.Validate(command);

		if (!validationResult.IsValid)
			return validationResult.ToErrorsList();

		var result = await existingEntitiesValidators.CheckCrafterAsync(command.Id, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		result.Value!.Delete();
		await crafterRepository.SaveAsync(result.Value, cancellationToken);
		return command.Id;
	}
}