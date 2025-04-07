using Dwarf_sMagicShop.Accounts.Application.Create;
using Dwarf_sMagicShop.Accounts.Application.Login;
using Dwarf_sMagicShop.Accounts.Application.Requests;
using Dwarf_sMagicShop.Accounts.Application.UpdateCrafter;
using Dwarf_sMagicShop.Accounts.Domain.Attributes;
using Dwarf_sMagicShop.API.Extensions;
using Dwarf_sMagicShop.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dwarf_sMagicShop.API.Controllers.Accounts;

public class AccountsController : BaseController
{
	private readonly ILogger<AccountsController> logger;

	public AccountsController(ILogger<AccountsController> logger)
	{
		this.logger = logger;
	}

	[HttpPost("registration")]
	public async Task<ActionResult> Register(
		[FromServices] CreateAccountHandler handler,
		[FromBody] AccountUserRequest request,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(request, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		logger.LogInformation("User {name} has been registered", request.UserName);
		return Ok();
	}

	[HttpPost("login")]
	public async Task<ActionResult<string>> Login(
		[FromServices] LoginUserHandler handler,
		[FromBody] AccountUserRequest request,
		CancellationToken cancellationToken = default)
	{
		var result = await handler.ExecuteAsync(request, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToResponse();

		return result.Value;
	}

	[Authorize]
	[Permission(Permissions.UPDATE_CRAFTER)]
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
}
