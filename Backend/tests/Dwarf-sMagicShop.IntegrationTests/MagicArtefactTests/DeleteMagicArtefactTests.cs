using Dwarf_sMagicShop.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class DeleteMagicArtefactTests : MagicArtefactFixtureTests
{
	public DeleteMagicArtefactTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_DeleteMagicArtefact_ReturnIsDeletedTrue()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactCreateResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		var artefact = artefactCreateResult.Value;
		var sut = scope.ServiceProvider.GetRequiredService<IUnitResultHandler<Guid, Guid>>();
		var result = await sut.ExecuteAsync(crafterResult.Value.Id.Value, artefact.Id.Value, CancellationToken.None);

		Assert.True(artefact.IsDeleted);
	}
}