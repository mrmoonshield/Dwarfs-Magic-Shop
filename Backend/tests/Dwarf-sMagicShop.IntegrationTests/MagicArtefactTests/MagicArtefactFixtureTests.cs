using AutoFixture;
using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class MagicArtefactFixtureTests : BaseFixtureTests<MagicArtefactsWebFactory>
{
	public MagicArtefactFixtureTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	protected byte[] CreateFakeImage()
	{
		var fakeImageSize = 5 * 1024 * 1024;
		byte[] fakeImageData = new byte[fakeImageSize];
		Random random = new Random();
		random.NextBytes(fakeImageData);
		return fakeImageData;
	}

	protected async Task<Result<MagicArtefact, ErrorsList>> CreateMagicArtefactAsync(Guid crafterId)
	{
		var sut = scope.ServiceProvider.GetRequiredService<
			IResultHandler<MagicArtefact,
			CreateMagicArtefactCommand,
			FileUploadCommand>>();

		var createArtefactCommand = fixture.Build<CreateMagicArtefactCommand>()
			.With(a => a.crafterId, crafterId)
			.Create();

		var artefactResult = await sut.ExecuteAsync(createArtefactCommand, CreateFileUploadCommand(), CancellationToken.None);
		return artefactResult;
	}

	protected FileUploadCommand CreateFileUploadCommand()
	{
		var fileUploadCommand = new FileUploadCommand(new MemoryStream(CreateFakeImage()), "bucket", $"{Guid.NewGuid}.jpg");
		return fileUploadCommand;
	}
}