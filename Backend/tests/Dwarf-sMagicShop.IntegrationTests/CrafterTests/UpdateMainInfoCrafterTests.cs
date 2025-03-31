using AutoFixture;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.CrafterTests;

public class UpdateMainInfoCrafterTests : BaseFixtureTests<BaseWebFactory>
{
	public UpdateMainInfoCrafterTests(BaseWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_UpdateMainInfoCrafter_ReturnSuccess()
	{
		var crafterResult = await CreateCrafterAsync();
		var sut = scope.ServiceProvider.GetRequiredService<IResultHandler<Guid, UpdateMainInfoCrafterCommand>>();

		var command = fixture.Build<UpdateMainInfoCrafterCommand>()
			.With(a => a.Id, crafterResult.Value.Id.Value)
			.Create();

		var result = await sut.ExecuteAsync(command, CancellationToken.None);
		Assert.True(result.IsSuccess);
	}
}