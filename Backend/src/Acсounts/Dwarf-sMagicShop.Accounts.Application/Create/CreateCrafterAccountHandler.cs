using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.MassTransitBuses;
using Dwarf_sMagicShop.Core;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarfs_Magic_Shop.Shared.Contracts.MassTransit;
using MassTransit;
using MassTransit.DependencyInjection;
using System.Security.Claims;

namespace Dwarf_sMagicShop.Accounts.Application.Create;

public class CreateCrafterAccountHandler(
	IAccountRepository accountRepository,
	Bind<IAccountsMassTransitBus, IPublishEndpoint> bind,
	IOutboxRepository outboxRepository) : IUnitResultHandler<string>
{
	public async Task<UnitResult<ErrorsList>> ExecuteAsync(string userId, CancellationToken cancellationToken)
	{
		var userResult = await accountRepository.GetUserByIdAsync(userId!, cancellationToken);

		if (userResult.IsFailure)
			return Errors.NotFound($"User {userId}").ToErrorsList();

		var user = userResult.Value;
		var crafterAccount = user.CreateCrafterAccount(Guid.NewGuid());
		var result = await accountRepository.AddCrafterAccountAsync(crafterAccount, cancellationToken);

		if (result.IsFailure)
			return result.Error.ToErrorsList();

		var crafterId = Guid.NewGuid();
		//await bind.Value.Publish(new CrafterAccountCreatedEvent(crafterId, user.UserName!), cancellationToken);
		await outboxRepository.AddAsync(new CrafterAccountCreatedEvent(crafterId, user.UserName!), cancellationToken);
		return Result.Success<ErrorsList>();
	}
}
