using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Validators;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Delete;

public class DeleteMagicArtefactHandler : IUnitResultHandler<Guid, Guid>
{
	private readonly ValidatorsProvider validatorsProvider;
	private readonly ICrafterRepository crafterRepository;

	public DeleteMagicArtefactHandler(
		ValidatorsProvider validatorsProvider,
		ICrafterRepository crafterRepository)
	{
		this.validatorsProvider = validatorsProvider;
		this.crafterRepository = crafterRepository;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(Guid crafterId, Guid artefactId, CancellationToken cancellationToken)
	{
		var existResult = await validatorsProvider
			.GetValidator<ExistingEntitiesValidators>()
			.CheckCrafterAsync(crafterId, cancellationToken)
			.WithMagicArtefact(artefactId);

		if (existResult.IsFailure)
			return existResult.ToErrorsList();

		existResult.Value.artefact.Delete();
		var result = await crafterRepository.SaveAsync(existResult.Value.crafter, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		return Result.Success<ErrorsList>();
	}
}