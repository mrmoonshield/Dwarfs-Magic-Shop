using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Validators;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Move;

public class MoveMagicArtefactHandler : IUnitResultHandler<Guid, Guid, MoveMagicArtefactRequest>
{
	private readonly ICrafterRepository crafterRepository;
	private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public MoveMagicArtefactHandler(
		ICrafterRepository crafterRepository,
		ExistingEntitiesValidators existingEntitiesValidators)
	{
		this.crafterRepository = crafterRepository;
		this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(
		Guid crafterId,
		Guid artefactId,
		MoveMagicArtefactRequest request,
		CancellationToken cancellationToken)
	{
		var result = await existingEntitiesValidators.CheckCrafterAsync(crafterId, cancellationToken)
			.WithMagicArtefact(artefactId);

		if (result.IsFailure)
			return result.ToErrorsList();

		var moveResult = result.Value.crafter.MoveArtefact(result.Value.artefact, request.toPosition);

		if (moveResult.IsFailure)
			return moveResult.ToErrorsList();

		await crafterRepository.SaveAsync(result.Value.crafter, cancellationToken);
		return Result.Success<ErrorsList>();
	}
}