using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.Validators;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using FluentValidation;

namespace Dwarf_sMagicShop.Accounts.Application.UpdateCrafter;

public class UpdateSocialsCrafterHandler : IResultHandler<Guid, UpdateSocialsCrafterCommand>
{
	private readonly IAccountRepository accountRepository;
	private readonly IValidator<UpdateSocialsCrafterCommand> validator;
	private readonly ExistingAccountValidator existingAccountValidator;

	public UpdateSocialsCrafterHandler(
		IAccountRepository accountRepository,
		IValidator<UpdateSocialsCrafterCommand> validator,
		ExistingAccountValidator existingAccountValidator)
	{
		this.accountRepository = accountRepository;
		this.validator = validator;
		this.existingAccountValidator = existingAccountValidator;
	}

	public async Task<Result<Guid, ErrorsList>> ExecuteAsync(
		UpdateSocialsCrafterCommand command,
		CancellationToken cancellationToken)
	{
		var validationResult = validator.Validate(command);

		if (!validationResult.IsValid)
			return validationResult.ToErrorsList();

		var result = await existingAccountValidator.CheckCrafterAccountAsync(command.Id, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		List<Social> socials = [];

		foreach (var item in command.Request)
		{
			var social = new Social() { SocialType = item.SocialType, Reference = item.Reference };
			socials.Add(social);
		}

		result.Value!.UpdateSocials(socials);
		await accountRepository.SaveAsync(cancellationToken);
		return command.Id;
	}
}