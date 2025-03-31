﻿using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Core.Extensions;
using Dwarf_sMagicShop.Core.Messages;
using Dwarf_sMagicShop.Core.Validators;
using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Dwarf_sMagicShop.Crafters.Application.Validators;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Dwarf_sMagicShop.Species.Application.ArtefactsSpecies;
using Dwarf_sMagicShop.Species.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;

public class CreateMagicArtefactHandler : IResultHandler<MagicArtefact, CreateMagicArtefactCommand, FileUploadCommand>
{
	private readonly ISpeciesRepository speciesRepository;
	private readonly IDatabaseTransactionProvider dbTransactionProvider;
	private readonly IFileProvider fileProvider;
	private readonly ILogger<CreateMagicArtefactHandler> logger;
	private readonly IMessageQueue<string> messageQueue;
	private readonly ValidatorsProvider validatorsProvider;

	public CreateMagicArtefactHandler(
		ISpeciesRepository speciesRepository,
		IDatabaseTransactionProvider dbTransactionProvider,
		IFileProvider fileProvider,
		ILogger<CreateMagicArtefactHandler> logger,
		IMessageQueue<string> messageQueue,
		ValidatorsProvider validatorsProvider)
	{
		this.speciesRepository = speciesRepository;
		this.dbTransactionProvider = dbTransactionProvider;
		this.fileProvider = fileProvider;
		this.logger = logger;
		this.messageQueue = messageQueue;
		this.validatorsProvider = validatorsProvider;
	}

	public async Task<Result<MagicArtefact, ErrorsList>> ExecuteAsync(
		CreateMagicArtefactCommand artefactCommand,
		FileUploadCommand fileUploadCommand,
		CancellationToken cancellationToken)
	{
		var createArtefactValidationResult = validatorsProvider.GetValidator<CreateMagicArtefactValidator>()
			.Validate(artefactCommand);

		if (!createArtefactValidationResult.IsValid)
			return createArtefactValidationResult.ToErrorsList();

		var transaction = dbTransactionProvider.StartTransactionAsync(cancellationToken);

		try
		{
			var crafterResult = await validatorsProvider.GetValidator<ExistingEntitiesValidators>()
				.CheckCrafterAsync(artefactCommand.crafterId, cancellationToken);

			if (crafterResult.IsFailure)
				return crafterResult.ToErrorsList();

			var artefactId = MagicArtefactID.NewMagicArtefactID;
			var createArtefactResult = MagicArtefact.Create(artefactId, artefactCommand.UpdateRequest.Name, artefactCommand.UpdateRequest.Description);

			if (createArtefactResult.IsFailure)
				return createArtefactResult.ToErrorsList();

			var addResult = crafterResult.Value!.AddNewArtefact(createArtefactResult.Value);

			if (addResult.IsFailure)
				return addResult.ToErrorsList();

			var requestValidationResult = validatorsProvider.GetValidator<UpdateMagicArtefactValidator>()
				.Validate(artefactCommand.UpdateRequest);

			if (!requestValidationResult.IsValid)
				return requestValidationResult.ToErrorsList();

			ArtefactSpeciesID artefactSpeciesID = ArtefactSpeciesID.EmptyArtefactSpeciesID;

			if (artefactCommand.UpdateRequest.Species != null)
			{
				var checkSpeciesResult = await SpeciesShared.CheckSpecies(
					artefactCommand.UpdateRequest.Species,
					speciesRepository,
					cancellationToken);

				if (checkSpeciesResult.IsFailure)
					return checkSpeciesResult.ToErrorsList();

				artefactSpeciesID = checkSpeciesResult.Value.Id;
			}

			createArtefactResult.Value.UpdateInfo(
				artefactSpeciesID,
				artefactCommand.UpdateRequest.Effect,
				artefactCommand.UpdateRequest.RareType,
				artefactCommand.UpdateRequest.Location,
				artefactCommand.UpdateRequest.Weight,
				artefactCommand.UpdateRequest.HandedAmountType);

			var fileUploadValidationResult = validatorsProvider.GetValidator<UploadImagesValidator>()
				.Validate(fileUploadCommand);

			if (!fileUploadValidationResult.IsValid)
				return fileUploadValidationResult.ToErrorsList();

			var fileResult = Domain.Models.File.Create(fileUploadCommand.objectName);

			if (fileResult.IsFailure)
				return fileResult.ToErrorsList();

			createArtefactResult.Value.AddImageFile(fileResult.Value);

			await dbTransactionProvider.SaveAsync(cancellationToken);
			var uploadResult = await fileProvider.UploadFileAsync(fileUploadCommand, cancellationToken);

			if (uploadResult.IsFailure)
			{
				await messageQueue.WriteAsync([fileUploadCommand.objectName], cancellationToken);
				return uploadResult.ToErrorsList();
			}

			await transaction.Result.CommitAsync(cancellationToken);
			return createArtefactResult.Value;
		}
		catch (Exception ex)
		{
			transaction.Result.Rollback();
			logger.LogError(ex, "Could not add new magic artefact");
			return Errors.InvalidOperation("Could not add new magic artefact").ToErrorsList();
		}
	}
}