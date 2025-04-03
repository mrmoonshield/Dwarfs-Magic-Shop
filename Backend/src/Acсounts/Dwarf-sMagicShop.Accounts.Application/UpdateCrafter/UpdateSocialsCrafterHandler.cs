using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using FluentValidation;

namespace Dwarf_sMagicShop.Accounts.Application.UpdateCrafter;

public class UpdateSocialsCrafterHandler : IResultHandler<Guid, UpdateSocialsCrafterCommand>
{
	private readonly IAccountRepository accountRepository;
	//private readonly IValidator<UpdateSocialsCrafterCommand> validator;
	//private readonly ExistingEntitiesValidators existingEntitiesValidators;

	public UpdateSocialsCrafterHandler(
		IAccountRepository accountRepository
		//IValidator<UpdateSocialsCrafterCommand> validator
		/*ExistingEntitiesValidators existingEntitiesValidators*/)
	{
		this.accountRepository = accountRepository;
		//this.validator = validator;
		//this.existingEntitiesValidators = existingEntitiesValidators;
	}

	public async Task<Result<Guid, ErrorsList>> ExecuteAsync(
		UpdateSocialsCrafterCommand command,
		CancellationToken cancellationToken)
	{
		//var validationResult = validator.Validate(command);

		//if (!validationResult.IsValid)
		//	return validationResult.ToErrorsList();

		//var result = await existingEntitiesValidators.CheckCrafterAsync(command.Id, cancellationToken);

		//if (result.IsFailure)
		//	return result.ToErrorsList();

		//List<Social> socials = [];

		//foreach (var item in command.Request)
		//{
		//	var socialResult = Social.Create(item.SocialType, item.Reference);
		//	socials.Add(socialResult.Value);
		//}

		//result.Value!.UpdateSocials(socials);
		//await accountRepository.SaveAsync(result.Value, cancellationToken);
		//return command.Id;
		return Guid.Empty;
	}
}