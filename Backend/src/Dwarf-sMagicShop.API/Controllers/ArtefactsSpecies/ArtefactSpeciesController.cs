using Dwarf_sMagicShop.Accounts.Domain.Attributes;
using Dwarf_sMagicShop.API.Extensions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies.Delete;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies.Get;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dwarf_sMagicShop.API.Controllers.ArtefactsSpecies;

[Authorize]
public class ArtefactSpeciesController : BaseController
{
	[Permission("Crafter")]
	[HttpGet]
	public async Task<ActionResult<IReadOnlyCollection<ArtefactSpeciesDto>>> Get(
		[FromServices] GetArtefactSpeciesHandler handler,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(cancellationToken);
		return Ok(result.Value);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(
		[FromServices] DeleteArtefactSpeciesHandler handler,
		[FromServices] ILogger<ArtefactSpeciesController> logger,
		[FromRoute] Guid id,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(id, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Species {id} was deleted", id);
		return Ok(result);
	}
}