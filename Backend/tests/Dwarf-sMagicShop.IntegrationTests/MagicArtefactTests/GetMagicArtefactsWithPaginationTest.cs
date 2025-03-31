using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class GetMagicArtefactsWithPaginationTest : MagicArtefactFixtureTests
{
	private readonly IResultHandler<
		PagedList<MagicArtefactDto>,
		GetEntitiesWithPaginationQuery,
		GetFilteredMagicArtefactQuery> sut;

	public GetMagicArtefactsWithPaginationTest(MagicArtefactsWebFactory factory) : base(factory)
	{
		sut = scope.ServiceProvider.GetRequiredService<
			IResultHandler<
			PagedList<MagicArtefactDto>,
			GetEntitiesWithPaginationQuery,
			GetFilteredMagicArtefactQuery>>();
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactsWithPaginationWithoutFilter_ReturnArtefacts()
	{
		const int ARTEFACTS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;

		var crafterResult = await CreateCrafterAsync();

		for (var i = 0; i < ARTEFACTS_COUNT; i++)
			await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);

		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		Assert.Equal(ARTEFACTS_COUNT, result.Value.Items.Count);
	}

	[Fact]
	public async Task ExecuteAsync_GetMagicArtefactsWithPaginationWithFilter_Return1Artefact()
	{
		const int ARTEFACTS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;

		var crafterResult = await CreateCrafterAsync();
		List<MagicArtefact> artefacts = [];

		for (var i = 0; i < ARTEFACTS_COUNT; i++)
		{
			var artefactResult = await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
			artefactResult.Value.UpdateStatus(ArtefactStatusType.Crafting);
			artefacts.Add(artefactResult.Value);
		}

		artefacts.Last().UpdateStatus(ArtefactStatusType.OnSale);
		await crafterRepository.SaveAsync(crafterResult.Value, CancellationToken.None);

		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);

		var filterQuery = new GetFilteredMagicArtefactQuery(
			null,
			null,
			null,
			ArtefactStatusType.OnSale);

		var result = await sut.ExecuteAsync(query, filterQuery, CancellationToken.None);

		Assert.Equal(1, result.Value.Count);
	}
}