using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Dwarfs_Magic_Shop.Shared.Contracts.RabbitMQ;
using FluentValidation;
using MassTransit;

namespace Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;

public class CreateCrafterHandler : IResultHandler<Crafter, CreateCrafterRequest>
{
	private readonly ICrafterRepository crafterRepository;
	private readonly IValidator<CreateCrafterRequest> validator;
	private readonly IPublishEndpoint publishEndpoint;

	public CreateCrafterHandler(
		ICrafterRepository crafterRepository,
		IValidator<CreateCrafterRequest> validator,
		IPublishEndpoint publishEndpoint)
	{
		this.crafterRepository = crafterRepository;
		this.validator = validator;
		this.publishEndpoint = publishEndpoint;
	}

	public async Task<Result<Crafter, ErrorsList>> ExecuteAsync(CreateCrafterRequest request, CancellationToken cancellationToken)
	{
		var validationResult = validator.Validate(request);

		if (!validationResult.IsValid)
			return validationResult.ToErrorsList();

		var nicknameResult = Nickname.Create(request.nickname);
		if (nicknameResult.IsFailure)
			return nicknameResult.ToErrorsList();

		var existCrafterResult = await crafterRepository.GetByNicknameAsync(nicknameResult.Value, cancellationToken);

		if (existCrafterResult.IsSuccess)
			return Errors.ValueIsInvalid("Nickname").ToErrorsList();

		var crafterID = CrafterID.NewCrafterID;
		var crafterResult = Crafter.Create(crafterID, nicknameResult.Value);

		if (crafterResult.IsFailure)
			return crafterResult.ToErrorsList();

		await publishEndpoint.Publish(new CrafterCreatedEvent(crafterID.Value), cancellationToken);
		return Errors.ValueIsInvalid("Nickname").ToErrorsList();
		var result = await crafterRepository.AddAsync(crafterResult.Value, cancellationToken);
		return result.Value;
	}
}