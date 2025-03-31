using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Move;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class MoveMagicArtefactTests : MagicArtefactFixtureTests
{
	public MoveMagicArtefactTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_MoveMagicArtefactFrom1PosToEnd_ReturnEndPos()
	{
		const int ARTEFACTS_COUNT = 5;
		var crafterResult = await CreateCrafterAsync();
		MagicArtefact magicArtefact = default!;

		for (int i = 0; i < ARTEFACTS_COUNT; i++)
		{
			var result = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
			if (i == 0) magicArtefact = result.Value;
		}

		var sut = scope.ServiceProvider.GetRequiredService<IUnitResultHandler<Guid, Guid, MoveMagicArtefactRequest>>();
		var command = new MoveMagicArtefactRequest(ARTEFACTS_COUNT);
		await sut.ExecuteAsync(crafterResult.Value.Id.Value, magicArtefact.Id.Value, command, CancellationToken.None);
		Assert.Equal(ARTEFACTS_COUNT, magicArtefact.Position.Value);
	}
}