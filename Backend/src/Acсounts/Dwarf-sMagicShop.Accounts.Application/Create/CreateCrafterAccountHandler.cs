using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarfs_Magic_Shop.Shared.Contracts.RabbitMQ;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Dwarf_sMagicShop.Accounts.Application.Create;

public class CreateCrafterAccountHandler(
	IAccountRepository accountRepository,
	UserManager<User> userManager) : IUnitResultHandler<string, Guid>, IConsumer<CrafterCreatedEvent>
{
	public async Task Consume(ConsumeContext<CrafterCreatedEvent> context)
	{
		//await ExecuteAsync(,context.Message.CrafterId, context.CancellationToken);
		await Task.CompletedTask;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(string userId, Guid crafterId, CancellationToken cancellationToken)
	{
		var user = await userManager.FindByIdAsync(userId);

		if (user == null)
			return Errors.NotFound(userId.ToString()).ToErrorsList();

		var crafterAccount = user.CreateCrafterAccount(crafterId);
		var result = await accountRepository.AddCrafterAccountAsync(crafterAccount, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		return Result.Success<ErrorsList>();
	}
}
