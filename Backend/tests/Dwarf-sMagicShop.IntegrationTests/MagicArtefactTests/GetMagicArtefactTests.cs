using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class GetMagicArtefactTests : MagicArtefactFixtureTests
{
	private readonly IResultHandler<MagicArtefactDto, Guid, Guid> sut;

	public GetMagicArtefactTests(MagicArtefactsWebFactory factory) : base(factory)
	{
		sut = scope.ServiceProvider.GetRequiredService<IResultHandler<MagicArtefactDto, Guid, Guid>>();
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefact_ReturnArtefact()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);

		var result = await sut.ExecuteAsync(
			crafterResult.Value.Id.Value,
			artefactResult.Value.Id.Value,
			CancellationToken.None);

		Assert.Equal(artefactResult.Value.Id.Value, result.Value.Id);
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactWithWrongCrafterId_ReturnFailure()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);

		var result = await sut.ExecuteAsync(
			Guid.Empty,
			artefactResult.Value.Id.Value,
			CancellationToken.None);

		Assert.False(result.IsSuccess);
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactWithWrongArtefactId_ReturnFailure()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);

		var result = await sut.ExecuteAsync(
			crafterResult.Value.Id.Value,
			Guid.Empty,
			CancellationToken.None);

		Assert.False(result.IsSuccess);
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactWithDeleteTrue_ReturnFailure()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		artefactResult.Value.Delete();
		await crafterRepository.SaveAsync(crafterResult.Value, CancellationToken.None);

		var result = await sut.ExecuteAsync(
			crafterResult.Value.Id.Value,
			artefactResult.Value.Id.Value,
			CancellationToken.None);

		Assert.False(result.IsSuccess);
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactWithCrafterDeleteTrue_ReturnFailure()
	{
		var crafterResult = await CreateCrafterAsync();
		var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		crafterResult.Value.Delete();
		await crafterRepository.SaveAsync(crafterResult.Value, CancellationToken.None);

		var result = await sut.ExecuteAsync(
			crafterResult.Value.Id.Value,
			artefactResult.Value.Id.Value,
			CancellationToken.None);

		Assert.False(result.IsSuccess);
	}
}