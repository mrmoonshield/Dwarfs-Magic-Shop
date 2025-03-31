using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Validators;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Status
{
	public class UpdateMagicArtefactStatusHandler : IUnitResultHandler<Guid, Guid, UpdateMagicArtefactStatusRequest>
	{
		private readonly ValidatorsProvider validatorsProvider;
		private readonly ICrafterRepository crafterRepository;

		public UpdateMagicArtefactStatusHandler(
			ValidatorsProvider validatorsProvider,
			ICrafterRepository crafterRepository)
		{
			this.validatorsProvider = validatorsProvider;
			this.crafterRepository = crafterRepository;
		}

		public async Task<UnitResult<ErrorsList>> ExecuteAsync(Guid crafterId, Guid artefactId, UpdateMagicArtefactStatusRequest command, CancellationToken cancellationToken)
		{
			var existResult = await validatorsProvider
				.GetValidator<ExistingEntitiesValidators>()
				.CheckCrafterAsync(crafterId, cancellationToken)
				.WithMagicArtefact(artefactId);

			if (existResult.IsFailure)
				return existResult.ToErrorsList();

			var commandValidationResult = validatorsProvider.GetValidator<UpdateMagicArtefactStatusValidator>().Validate(command);

			if (!commandValidationResult.IsValid)
				return commandValidationResult.ToErrorsList();

			existResult.Value.artefact.UpdateStatus(command.ArtefactStatusType);

			var result = await crafterRepository.SaveAsync(existResult.Value.crafter, cancellationToken);

			if (result.IsFailure)
				return result.ToErrorsList();

			return Result.Success<ErrorsList>();
		}
	}
}