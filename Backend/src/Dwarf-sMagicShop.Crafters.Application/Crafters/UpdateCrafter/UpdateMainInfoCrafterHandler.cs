using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;

public class UpdateMainInfoCrafterHandler : IResultHandler<Guid, UpdateMainInfoCrafterCommand>
{
	private readonly ICrafterRepository crafterRepository;
	private readonly IValidator<UpdateMainInfoCrafterCommand> validator;
	private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public UpdateMainInfoCrafterHandler(
		ICrafterRepository crafterRepository,
		IValidator<UpdateMainInfoCrafterCommand> validator,
		ExistingEntitiesValidators existingEntitiesValidators)
	{
		this.crafterRepository = crafterRepository;
		this.validator = validator;
		this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<Result<Guid, ErrorsList>> ExecuteAsync(
		UpdateMainInfoCrafterCommand crafterCommand,
		CancellationToken cancellationToken)
	{
		var validationResult = validator.Validate(crafterCommand);

		if (!validationResult.IsValid)
			return validationResult.ToErrorsList();

		var result = await existingEntitiesValidators.CheckCrafterAsync(crafterCommand.Id, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		var nicknameResult = Nickname.Create(crafterCommand.Request.Nickname);
		result.Value!.Update(nicknameResult.Value, crafterCommand.Request.Experience);
		await crafterRepository.SaveAsync(result.Value, cancellationToken);
		return crafterCommand.Id;
	}
}