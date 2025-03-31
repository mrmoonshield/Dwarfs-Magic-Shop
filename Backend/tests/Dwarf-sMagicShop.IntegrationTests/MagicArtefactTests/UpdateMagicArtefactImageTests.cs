using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class UpdateMagicArtefactImageTests : MagicArtefactFixtureTests
{
	public UpdateMagicArtefactImageTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_UpdateMagicArtefactImage_ReturnSuccess()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactCreateResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		var artefact = artefactCreateResult.Value;
		var sut = scope.ServiceProvider.GetRequiredService<IUnitResultHandler<Guid, Guid, FileUploadCommand>>();
		var result = await sut.ExecuteAsync(crafterResult.Value.Id.Value, artefact.Id.Value, CreateFileUploadCommand(), CancellationToken.None);
		Assert.True(result.IsSuccess);
	}
}