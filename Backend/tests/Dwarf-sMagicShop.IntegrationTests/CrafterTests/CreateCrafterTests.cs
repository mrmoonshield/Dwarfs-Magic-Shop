using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.IntegrationTests.CrafterTests;

public class CreateCrafterTests : BaseFixtureTests<BaseWebFactory>
{
	public CreateCrafterTests(BaseWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_CreateCrafter_ReturnSuccessAndNotNull()
	{
		var result = await CreateCrafterAsync();
		var crafter = await readDbContextCrafters.Crafters.FirstOrDefaultAsync();
		Assert.True(result.IsSuccess);
		Assert.NotNull(crafter);
	}
}