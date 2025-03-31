using AutoFixture;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Status;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class UpdateMagicArtefactStatusTests : MagicArtefactFixtureTests
{
	public UpdateMagicArtefactStatusTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_UpdateMagicArtefactStatus_ReturnSuccess()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactCreateResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		var artefact = artefactCreateResult.Value;
		var sut = scope.ServiceProvider.GetRequiredService<IUnitResultHandler<Guid, Guid, UpdateMagicArtefactStatusRequest>>();
		var command = fixture.Create<UpdateMagicArtefactStatusRequest>();

		var result = await sut.ExecuteAsync(crafterResult.Value.Id.Value, artefact.Id.Value, command, CancellationToken.None);
		Assert.True(result.IsSuccess);
	}
}