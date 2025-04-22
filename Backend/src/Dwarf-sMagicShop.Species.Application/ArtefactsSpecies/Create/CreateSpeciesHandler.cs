using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarfs_Magic_Shop.Shared.Contracts.MediatRRequests.MagicArtefacts;
using MediatR;

namespace Dwarf_sMagicShop.Species.Application.ArtefactsSpecies.Create;

public class CreateSpeciesHandler(ISpeciesRepository speciesRepository)
	: IRequestHandler<GetSpeciesGuidRequest, Result<Guid, ErrorsList>>
{
	public async Task<Result<Guid, ErrorsList>> Handle(
		GetSpeciesGuidRequest request,
		CancellationToken cancellationToken)
	{
		var checkSpeciesResult = await SpeciesShared.CheckSpeciesAsync(
			request.Species,
			speciesRepository,
			cancellationToken);

		if (checkSpeciesResult.IsFailure)
			return checkSpeciesResult.Error.ToErrorsList();

		return checkSpeciesResult.Value.Id.Value;
	}
}
