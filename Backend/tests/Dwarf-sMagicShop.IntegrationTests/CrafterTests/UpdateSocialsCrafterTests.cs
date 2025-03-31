using AutoFixture;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.CrafterTests;

public class UpdateSocialsCrafterTests : BaseFixtureTests<BaseWebFactory>
{
	public UpdateSocialsCrafterTests(BaseWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public async Task ExecuteAsync_UpdateSocialsCrafter_ReturnSuccess()
	{
		var crafterResult = await CreateCrafterAsync();
		var sut = scope.ServiceProvider.GetRequiredService<IResultHandler<Guid, UpdateSocialsCrafterCommand>>();
		List<UpdateSocialsCrafterDto> socials = [new(SocialType.Telegramm, "https://t.me/economika/36681")];

		var command = fixture.Build<UpdateSocialsCrafterCommand>()
			.With(a => a.Id, crafterResult.Value.Id.Value)
			.With(a => a.Request, socials)
			.Create();

		var result = await sut.ExecuteAsync(command, CancellationToken.None);
		Assert.True(result.IsSuccess);
	}
}