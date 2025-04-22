using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using Dwarfs_Magic_Shop.Shared.Contracts.MediatRRequests.MagicArtefacts;
using MediatR;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

public class UpdateMagicArtefactInfoHandler : IUnitResultHandler<Guid, Guid, UpdateMagicArtefactRequest>
{
	private readonly ValidatorsProvider validatorsProvider;
	private readonly ICrafterRepository crafterRepository;
	private readonly IMediator mediator;

	public UpdateMagicArtefactInfoHandler(
		ValidatorsProvider validatorsProvider,
		ICrafterRepository crafterRepository,
		IMediator mediator)
	{
		this.validatorsProvider = validatorsProvider;
		this.crafterRepository = crafterRepository;
		this.mediator = mediator;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(Guid crafterId, Guid artefactId, UpdateMagicArtefactRequest command, CancellationToken cancellationToken)
	{
		var existResult = await validatorsProvider
			.GetValidator<ExistingEntitiesValidators>()
			.CheckCrafterAsync(crafterId, cancellationToken)
			.WithMagicArtefact(artefactId);

		if (existResult.IsFailure)
			return existResult.ToErrorsList();

		var commandValidationResult = validatorsProvider.GetValidator<UpdateMagicArtefactValidator>().Validate(command);

		if (!commandValidationResult.IsValid)
			return commandValidationResult.ToErrorsList();

		Guid? speciesGuid = null;

		if (command.Species != null)
		{
			var speciesGuidResult = await mediator.Send(new GetSpeciesGuidRequest(
							command.Species),
							cancellationToken);

			if (speciesGuidResult.IsFailure)
				return speciesGuidResult.Error;

			speciesGuid = speciesGuidResult.Value;
		}

		existResult.Value.artefact.UpdateInfo(
			speciesGuid,
			command.Effect,
			command.RareType,
			command.Location,
			command.Weight,
			command.HandedAmountType,
			command.Name,
			command.Description);

		var result = await crafterRepository.SaveAsync(existResult.Value.crafter, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		return Result.Success<ErrorsList>();
	}
}