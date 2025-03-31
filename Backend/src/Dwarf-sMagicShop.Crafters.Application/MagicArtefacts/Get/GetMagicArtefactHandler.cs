using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Application.Validators;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Get;

public class GetMagicArtefactHandler : IResultHandler<MagicArtefactDto, Guid, Guid>
{
	private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public GetMagicArtefactHandler(ExistingEntitiesValidators existingEntitiesValidators)
	{
		this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<Result<MagicArtefactDto, ErrorsList>> ExecuteAsync(
		Guid crafterId,
		Guid artefactId,
		CancellationToken cancellationToken)
	{
		var result = await existingEntitiesValidators.CheckCrafterDtoAsync(crafterId, cancellationToken)
			.WithMagicArtefactDto(artefactId);

		if (result.IsFailure)
			return result.ToErrorsList();

		return result.Value.artefact;
	}
}