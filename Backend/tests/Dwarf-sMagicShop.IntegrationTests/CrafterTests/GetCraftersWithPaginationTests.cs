using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Dtos;
using Dwarf_sMagicShop.Core.Shared;
using Dwarf_sMagicShop.Core.Shared.Queries;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.CrafterTests;

public class GetCraftersWithPaginationTests : BaseFixtureTests<BaseWebFactory>
{
	private readonly IQueryHandler<PagedList<CrafterDto>, GetEntitiesWithPaginationQuery, GetFilteredMagicArtefactQuery> sut;

	public GetCraftersWithPaginationTests(BaseWebFactory factory) : base(factory)
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
	public async Task ExecuteAsync_GetCraftersWithPaginationWithoutFilters_ReturnCraftersCount()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;

		await CreateCraftersWithCountAsync(CRAFTERS_COUNT);
		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		Assert.Equal(CRAFTERS_COUNT, result.Items.Count);
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithPaginationWithoutFilters_ReturnNotDeletedCraftersCount()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 10;

		var craftersList = await CreateCraftersWithCountAsync(CRAFTERS_COUNT);
		var crafter = craftersList[^1];
		crafter.Delete();
		await crafterRepository.SaveAsync(crafter, CancellationToken.None);
		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		var expected = CRAFTERS_COUNT - 1;
		Assert.Equal(expected, result.Items.Count);
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithPaginationWithoutFilters_Return2CraftersInFirstPage()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 2;

		await CreateCraftersWithCountAsync(CRAFTERS_COUNT);
		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		Assert.Equal(PAGE_SIZE, result.Items.Count);
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithPaginationWithoutFilters_Return1CraftersInLastPage()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 2;
		const int PAGE_SIZE = 2;

		await CreateCraftersWithCountAsync(CRAFTERS_COUNT);
		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		var expected = CRAFTERS_COUNT - PAGE_SIZE;
		Assert.Equal(expected, result.Items.Count);
	}

	[Fact]
	public async Task ExecuteAsync_GetCraftersWithPaginationWithoutFilters_ReturnPagesCount()
	{
		const int CRAFTERS_COUNT = 3;
		const int PAGE = 1;
		const int PAGE_SIZE = 2;
		const int EXPECTED_PAGE_COUNT = 2;

		await CreateCraftersWithCountAsync(CRAFTERS_COUNT);
		var query = new GetEntitiesWithPaginationQuery(PAGE, PAGE_SIZE);
		var result = await sut.ExecuteAsync(query, null!, CancellationToken.None);

		Assert.Equal(EXPECTED_PAGE_COUNT, result.Count);
	}
}