using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Messages;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Dwarf_sMagicShop.Crafters.Application.Validators;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Image;

public class UpdateMagicArtefactImageHandler : IUnitResultHandler<Guid, Guid, FileUploadCommand>
{
	private readonly ValidatorsProvider validatorsProvider;
	private readonly IFileProvider fileProvider;
	private readonly ICrafterRepository crafterRepository;
	private readonly IMessageQueue<string> messageQueue;

	public UpdateMagicArtefactImageHandler(
		ValidatorsProvider validatorsProvider,
		IFileProvider fileProvider,
		ICrafterRepository crafterRepository,
		IMessageQueue<string> messageQueue)
	{
		this.validatorsProvider = validatorsProvider;
		this.fileProvider = fileProvider;
		this.crafterRepository = crafterRepository;
		this.messageQueue = messageQueue;
	}

	public async Task<UnitResult<ErrorsList>> ExecuteAsync(
		Guid crafterId,
		Guid artefactId,
		FileUploadCommand command,
		CancellationToken cancellationToken)
	{
		var existResult = await validatorsProvider.GetValidator<ExistingEntitiesValidators>()
			.CheckCrafterAsync(crafterId, cancellationToken)
			.WithMagicArtefact(artefactId);

		if (existResult.IsFailure)
			return existResult.ToErrorsList();

		var fileUploadValidationResult = validatorsProvider.GetValidator<UploadImagesValidator>()
				.Validate(command);

		if (!fileUploadValidationResult.IsValid)
			return fileUploadValidationResult.ToErrorsList();

		var fileResult = Domain.Models.File.Create(command.objectName);

		if (fileResult.IsFailure)
			return fileResult.ToErrorsList();

		var uploadResult = await fileProvider.UploadFileAsync(command, cancellationToken);

		if (uploadResult.IsFailure)
			return uploadResult.ToErrorsList();

		await messageQueue.WriteAsync([existResult.Value.artefact.ImageFile.Name], cancellationToken);
		existResult.Value.artefact.AddImageFile(fileResult.Value);

		var result = await crafterRepository.SaveAsync(existResult.Value.crafter, cancellationToken);

		if (result.IsFailure)
			return result.ToErrorsList();

		return Result.Success<ErrorsList>();
	}
}