using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;

public class UpdateMagicArtefactInfoHandler : IUnitResultHandler<Guid, Guid, UpdateMagicArtefactRequest>
{
	private readonly ValidatorsProvider validatorsProvider;
	private readonly ISpeciesRepository speciesRepository;
	private readonly ICrafterRepository crafterRepository;

	public UpdateMagicArtefactInfoHandler(
		ValidatorsProvider validatorsProvider,
		ISpeciesRepository speciesRepository,
		ICrafterRepository crafterRepository)
	{
		this.validatorsProvider = validatorsProvider;
		this.speciesRepository = speciesRepository;
		this.crafterRepository = crafterRepository;
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

		var speciesResult = await SpeciesShared.CheckSpeciesAsync(command.Species!, speciesRepository, cancellationToken);

		existResult.Value.artefact.UpdateInfo(
			speciesResult.Value.Id.Value,
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