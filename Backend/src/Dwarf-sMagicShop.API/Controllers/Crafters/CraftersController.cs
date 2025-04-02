using Dwarf_sMagicShop.Accounts.Domain;
using Dwarf_sMagicShop.API.Controllers.Accounts;
using Dwarf_sMagicShop.API.Extensions;
using Dwarf_sMagicShop.API.Response;
using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;
using Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;
using Dwarf_sMagicShop.Crafters.Application.Crafters.Delete;
using Dwarf_sMagicShop.Crafters.Application.Crafters.Get;
using Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Delete;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Get;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Move;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Image;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Status;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dwarf_sMagicShop.API.Controllers.Crafters;

[Authorize]
public class CraftersController : BaseController
{
	private readonly ILogger<AccountsController> logger;

	public CraftersController(ILogger<AccountsController> logger)
	{
		this.logger = logger;
	}

	[Permission(Permissions.CREATE_CRAFTER)]
	[HttpPost]
	public async Task<ActionResult<Guid>> Create(
		[FromServices] CreateCrafterHandler crafterHandler,
		[FromBody] CreateCrafterRequest request,
		CancellationToken cancellationToken = default)
	{
		var result = await crafterHandler.ExecuteAsync(request, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Crafter {id} created", result.Value.Id.Value);
		return Ok(Envelope.Ok(result.Value.Id.Value));
	}

	[HttpPut("{id:guid}/main-info")]
	public async Task<ActionResult<Guid>> UpdateMainInfo(
		[FromServices] UpdateMainInfoCrafterHandler crafterHandler,
		[FromRoute] Guid id,
		[FromBody] UpdateMainInfoCrafterRequest request,
		CancellationToken cancellationToken = default)
	{
		var updateCommand = new UpdateMainInfoCrafterCommand(id, request);
		var result = await crafterHandler.ExecuteAsync(updateCommand, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Crafter {id} updated", id);
		return Ok(result.Value);
	}

	[HttpPut("{id:guid}/socials")]
	public async Task<ActionResult<Guid>> UpdateSocials(
		[FromServices] UpdateSocialsCrafterHandler crafterHandler,
		[FromRoute] Guid id,
		[FromBody] IReadOnlyCollection<UpdateSocialsCrafterDto> request,
		CancellationToken cancellationToken = default)
	{
		var updateCommand = new UpdateSocialsCrafterCommand(id, request);
		var result = await crafterHandler.ExecuteAsync(updateCommand, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Crafter {id} updated", id);
		return Ok(result.Value);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult<Guid>> DeleteCrafter(
		[FromServices] DeleteCrafterHandler crafterHandler,
		[FromRoute] Guid id,
		CancellationToken cancellationToken = default)
	{
		var command = new DeleteCrafterCommand(id);
		var result = await crafterHandler.ExecuteAsync(command, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Crafter {id} deleted", id);
		return Ok(result.Value);
	}

	[HttpPost("{id:guid}/magic-artefacts")]
	public async Task<ActionResult<Guid>> AddMagicArtefact(
		[FromServices] CreateMagicArtefactHandler artefactHandler,
		[FromRoute] Guid id,
		[FromForm] CreateMagicArtefactRequest request,
		CancellationToken cancellationToken = default)
	{
		var artefactCommand = new CreateMagicArtefactCommand(
			id,
			request.UpdateRequest);

		await using var stream = request.File.OpenReadStream();
		var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);

		var fileUploadCommand = new FileUploadCommand(
			stream,
			Constants.BUCKET_NAME_IMAGES,
			fileName);

		var artefactResult = await artefactHandler.ExecuteAsync(artefactCommand, fileUploadCommand, cancellationToken);

		if (artefactResult.IsFailure)
			return artefactResult.Error.ToResponse();

		logger.LogInformation("MagicArtefact {id} created", artefactResult.Value.Id.Value);
		return artefactResult.Value.Id.Value;
	}

	[HttpPost("{crafterId:guid}/magic-artefacts/{artefactId:guid}/positions")]
	public async Task<IActionResult> MoveMagicArtefact(
		[FromServices] MoveMagicArtefactHandler moveMagicArtefactHandler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		[FromBody] MoveMagicArtefactRequest request,
		CancellationToken cancellationToken = default)
	{
		var result = await moveMagicArtefactHandler.ExecuteAsync(crafterId, artefactId, request, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("Magic artefact {guid} moved", artefactId);
		return Ok();
	}

	[HttpGet]
	public async Task<ActionResult<PagedList<CrafterDto>>> GetCrafters(
		[FromServices] GetCraftersWithPaginationHandler handler,
		[FromQuery] GetEntitiesWithPaginationQuery paginationQuery,
		[FromQuery] GetFilteredMagicArtefactQuery filterQuery,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(paginationQuery, filterQuery, cancellationToken);
		return result;
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<CrafterDto>> GetCrafter(
		[FromServices] GetCrafterHandler handler,
		[FromRoute] Guid id,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(id, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return result.Value;
	}

	[HttpPut("{crafterId:guid}/magic-artefacts/{artefactId:guid}/infos")]
	public async Task<ActionResult> UpdateMagicArtefactInfo(
		[FromServices] UpdateMagicArtefactInfoHandler artefactHandler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		[FromBody] UpdateMagicArtefactRequest request,
		CancellationToken cancellationToken = default)
	{
		var artefactResult = await artefactHandler.ExecuteAsync(crafterId, artefactId, request, cancellationToken);

		if (artefactResult.IsFailure)
			return artefactResult.Error.ToResponse();

		logger.LogInformation("MagicArtefact {id} updated", artefactId);
		return Ok();
	}

	[HttpPut("{crafterId:guid}/magic-artefacts/{artefactId:guid}/statuses")]
	public async Task<ActionResult> UpdateMagicArtefactStatus(
		[FromServices] UpdateMagicArtefactStatusHandler artefactHandler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		[FromBody] UpdateMagicArtefactStatusRequest request,
		CancellationToken cancellationToken = default)
	{
		var artefactResult = await artefactHandler.ExecuteAsync(crafterId, artefactId, request, cancellationToken);

		if (artefactResult.IsFailure)
			return artefactResult.Error.ToResponse();

		logger.LogInformation("MagicArtefact {id} updated", artefactId);
		return Ok();
	}

	[HttpPut("{crafterId:guid}/magic-artefacts/{artefactId:guid}/images")]
	public async Task<ActionResult> UpdateMagicArtefactImage(
		[FromServices] UpdateMagicArtefactImageHandler artefactHandler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		[FromForm] UpdateMagicArtefactImageRequest request,
		CancellationToken cancellationToken = default)
	{
		await using var stream = request.File.OpenReadStream();
		var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);

		var fileUploadCommand = new FileUploadCommand(
			stream,
			Constants.BUCKET_NAME_IMAGES,
			fileName);

		var artefactResult = await artefactHandler.ExecuteAsync(crafterId, artefactId, fileUploadCommand, cancellationToken);

		if (artefactResult.IsFailure)
			return artefactResult.Error.ToResponse();

		logger.LogInformation("MagicArtefact {id} image updated", artefactId);
		return Ok();
	}

	[HttpDelete("{crafterId:guid}/magic-artefacts/{artefactId:guid}")]
	public async Task<ActionResult> DeleteMagicArtefact(
		[FromServices] DeleteMagicArtefactHandler artefactHandler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		CancellationToken cancellationToken = default)
	{
		var artefactResult = await artefactHandler.ExecuteAsync(crafterId, artefactId, cancellationToken);

		if (artefactResult.IsFailure)
			return artefactResult.Error.ToResponse();

		logger.LogInformation("MagicArtefact {id} updated", artefactId);
		return Ok();
	}

	[HttpGet("/magic-artefacts")]
	public async Task<ActionResult<PagedList<MagicArtefactDto>>> GetMagicArtefacts(
		[FromServices] GetMagicArtefactsWithPaginationHandler handler,
		[FromQuery] GetEntitiesWithPaginationQuery paginationQuery,
		[FromQuery] GetFilteredMagicArtefactQuery filterQuery,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(paginationQuery, filterQuery, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return result.Value;
	}

	[HttpGet("{crafterId:guid}/magic-artefacts/{artefactId:guid}")]
	public async Task<ActionResult<MagicArtefactDto>> GetMagicArtefact(
		[FromServices] GetMagicArtefactHandler handler,
		[FromRoute] Guid crafterId,
		[FromRoute] Guid artefactId,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(crafterId, artefactId, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return result.Value;
	}
}