using Dwarf_sMagicShop.Accounts.Application.Create;
using Dwarf_sMagicShop.Accounts.Application.Login;
using Dwarf_sMagicShop.Accounts.Application.Requests;
using Dwarf_sMagicShop.API.Extensions;
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
}
