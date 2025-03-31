using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Crafters.Application.Crafters.Delete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.CrafterTests;

public class DeleteCrafterTests : BaseFixtureTests<BaseWebFactory>
{
	public DeleteCrafterTests(BaseWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_DeleteCrafter_ReturnCrafterIsDeletetedTrue()
	{
		var crafterResult = await CreateCrafterAsync();
		var deleteCommand = new DeleteCrafterCommand(crafterResult.Value.Id.Value);
		var sut = scope.ServiceProvider.GetRequiredService<IResultHandler<Guid, DeleteCrafterCommand>>();
		await sut.ExecuteAsync(deleteCommand, CancellationToken.None);
		var crafter = await readDbContextCrafters.Crafters.FirstOrDefaultAsync();
		Assert.True(crafter!.IsDeleted);
	}
}