using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;

public class UpdateSocialsCrafterHandler : IResultHandler<Guid, UpdateSocialsCrafterCommand>
{
	private readonly ICrafterRepository crafterRepository;
	private readonly IValidator<UpdateSocialsCrafterCommand> validator;
	private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public UpdateSocialsCrafterHandler(
		ICrafterRepository crafterRepository,
		IValidator<UpdateSocialsCrafterCommand> validator,
		ExistingEntitiesValidators existingEntitiesValidators)
	{
		this.crafterRepository = crafterRepository;
		this.validator = validator;
		this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<Result<Guid, ErrorsList>> ExecuteAsync(
		UpdateSocialsCrafterCommand command,
		CancellationToken cancellationToken)
	{
		var validationResult = validator.Validate(command);

		if (!validationResult.IsValid)
			return validationResult.ToErrorsList();

		var result = await existingEntitiesValidators.CheckCrafterAsync(command.Id, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		List<Social> socials = [];

		foreach (var item in command.Request)
		{
			var socialResult = Social.Create(item.SocialType, item.Reference);
			socials.Add(socialResult.Value);
		}

		result.Value!.UpdateSocials(socials);
		await crafterRepository.SaveAsync(result.Value, cancellationToken);
		return command.Id;
	}
}