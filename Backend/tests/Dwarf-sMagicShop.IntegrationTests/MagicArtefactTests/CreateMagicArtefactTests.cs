using Microsoft.EntityFrameworkCore;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class CreateMagicArtefactTests : MagicArtefactFixtureTests
{
	public CreateMagicArtefactTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_AddNewMagicArtefactToCrafter_ReturnArtefactExistInDB()
	{
		var crafterResult = await CreateCrafterAsync();
		await CreateMagicArtefactAsync(crafterResult.Value.Id.Value);
		Assert.True(await readDbContextCrafters.MagicArtefacts.AnyAsync());
	}
}