using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class GetCraftersArtefactFilteredTests : MagicArtefactFixtureTests
{
	private readonly IQueryHandler<PagedList<CrafterDto>, GetEntitiesWithPaginationQuery, GetFilteredMagicArtefactQuery> sut;

	public GetCraftersArtefactFilteredTests(MagicArtefactsWebFactory factory) : base(factory)
	{
		sut = scope.ServiceProvider.GetRequiredService<
			IQueryHandler<PagedList<CrafterDto>,
			GetEntitiesWithPaginationQuery,
			GetFilteredMagicArtefactQuery>>();
	}

	private async Task<List<Crafter>> CreateCraftersWithCountAsync(int count)
	{
		List<Crafter> crafters = [];

		for (int i = 0; i < count; i++)
		{
			var result = await CreateCrafterAsync();
			crafters.Add(result.Value);
		}

		return crafters;
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithArtefactStatusTypeFilter_Return1Crafter()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;

		var crafters = await CreateCraftersWithCountAsync(CRAFTERS_COUNT);

		for (int i = 0; i < crafters.Count; i++)
		{
			await CreateMagicArtefactAsync(crafters[i].Id.Value);
		}

		var crafter = crafters[^1];
		crafter.Artefacts.First().UpdateStatus(ArtefactStatusType.OnSale);
		await crafterRepository.SaveAsync(crafter, CancellationToken.None);

		var paginationQuery = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);

		var filterQuery = new GetFilteredMagicArtefactQuery(
			null,
			null,
			null,
			ArtefactStatusType.OnSale);

		var result = await sut.ExecuteAsync(paginationQuery, filterQuery, CancellationToken.None);

		Assert.Single(result.Items);
		Assert.Equal(crafter.Id.Value, result.Items.First().Id);
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithContainsStringFilter_Return1Crafter()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;
		const string CONTAINS_STRING = "Axe";

		var crafters = await CreateCraftersWithCountAsync(CRAFTERS_COUNT);

		for (int i = 0; i < crafters.Count; i++)
		{
			await CreateMagicArtefactAsync(crafters[i].Id.Value);
		}

		var crafter = crafters[^1];
		var artefact = crafter.Artefacts.First();

		artefact.UpdateInfo(
			artefact.SpeciesId,
			artefact.Effect,
			artefact.Rare,
			artefact.Location,
			artefact.Weight,
			artefact.HandedAmountType,
			CONTAINS_STRING,
			artefact.Description);

		await crafterRepository.SaveAsync(crafter, CancellationToken.None);

		var paginationQuery = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);

		var filterQuery = new GetFilteredMagicArtefactQuery(
			CONTAINS_STRING,
			null,
			null,
			null);

		var result = await sut.ExecuteAsync(paginationQuery, filterQuery, CancellationToken.None);

		Assert.Single(result.Items);
		Assert.Equal(crafter.Id.Value, result.Items.First().Id);
	}
}