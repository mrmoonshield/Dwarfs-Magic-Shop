using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Domain.Models;

namespace Dwarf_sMagicShop.Crafters.Application.Validators;

public class ExistingEntitiesValidators : ICustomValidator
{
	private readonly ICrafterRepository crafterRepository;

	public ExistingEntitiesValidators(ICrafterRepository crafterRepository)
	{
		this.crafterRepository = crafterRepository;
	}

	public async Task<Result<Crafter?, Error>> CheckCrafterAsync(Guid id, CancellationToken cancellationToken)
	{
		var crafterResult = await crafterRepository.GetByIDAsync(CrafterID.Create(id), cancellationToken);
		return crafterResult;
	}

	public async Task<Result<CrafterDto?, Error>> CheckCrafterDtoAsync(Guid id, CancellationToken cancellationToken)
	{
		var crafterResult = await crafterRepository.GetByIDDtoAsync(id, cancellationToken);
		return crafterResult;
	}
}

public static class ExistingEntitiesValidatorsExtension
{
	public static async Task<Result<(Crafter crafter, MagicArtefact artefact), Error>> WithMagicArtefact(
		this Task<Result<Crafter?, Error>> resultTask, Guid artefactId)
	{
		var result = resultTask.Result;

		if (result.IsFailure)
			return result.Error;

		var artefactResult = result.Value!.GetArtefactById(MagicArtefactID.Create(artefactId));

		if (artefactResult.IsFailure)
			return artefactResult.Error;

		await Task.CompletedTask;
		return (result.Value, artefactResult.Value);
	}

	public static async Task<Result<(CrafterDto crafter, MagicArtefactDto artefact), Error>> WithMagicArtefactDto(
		this Task<Result<CrafterDto?, Error>> resultTask, Guid artefactId)
	{
		var result = resultTask.Result;

		if (result.IsFailure)
			return result.Error;

		var artefact = result.Value!.Artefacts.FirstOrDefault(a => a.Id == artefactId);

		if (artefact == null)
			return Errors.NotFound(artefactId);

		await Task.CompletedTask;
		return (result.Value, artefact);
	}
}