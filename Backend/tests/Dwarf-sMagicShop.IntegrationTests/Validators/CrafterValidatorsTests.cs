using AutoFixture;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;
using Dwarf_sMagicShop.Crafters.Application.Crafters.Delete;
using Dwarf_sMagicShop.Crafters.Application.Crafters.UpdateCrafter;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.Validators;

public class CrafterValidatorsTests : BaseFixtureTests<BaseWebFactory>
{
	public CrafterValidatorsTests(BaseWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public void Validate_CreateCrafterRequest_ReturnSuccess()
	{
		var command = new CreateCrafterRequest("Nick");
		var validator = scope.ServiceProvider.GetRequiredService<CreateCrafterRequestValidator>();
		var result = validator.Validate(command);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_CreateCrafterRequest_ReturnFailure()
	{
		var command = new CreateCrafterRequest("");
		var validator = scope.ServiceProvider.GetRequiredService<CreateCrafterRequestValidator>();
		var result = validator.Validate(command);
		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_DeleteCrafterCommand_ReturnSuccess()
	{
		var command = new DeleteCrafterCommand(Guid.NewGuid());
		var validator = scope.ServiceProvider.GetRequiredService<DeleteCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_DeleteCrafterCommand_ReturnFailure()
	{
		var command = new DeleteCrafterCommand(Guid.Empty);
		var validator = scope.ServiceProvider.GetRequiredService<DeleteCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateMainInfoCrafterCommand_ReturnSuccess()
	{
		var command = fixture.Create<UpdateMainInfoCrafterCommand>();
		var validator = scope.ServiceProvider.GetRequiredService<UpdateMainInfoCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateMainInfoCrafterCommandWithWrongNickname_ReturnFailure()
	{
		var request = new UpdateMainInfoCrafterRequest("", 0);

		var command = fixture.Build<UpdateMainInfoCrafterCommand>()
			.With(a => a.Request, request)
			.Create();

		var validator = scope.ServiceProvider.GetRequiredService<UpdateMainInfoCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateMainInfoCrafterCommandWithWrongExperience_ReturnFailure()
	{
		var request = new UpdateMainInfoCrafterRequest("Nick", -1);

		var command = fixture.Build<UpdateMainInfoCrafterCommand>()
			.With(a => a.Request, request)
			.Create();

		var validator = scope.ServiceProvider.GetRequiredService<UpdateMainInfoCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateCrafterSocialsCommand_ReturnSuccess()
	{
		var dto = new UpdateSocialsCrafterDto(SocialType.Telegramm, "https://t.me/economika/36681");
		var command = new UpdateSocialsCrafterCommand(Guid.NewGuid(), [dto]);
		var validator = scope.ServiceProvider.GetRequiredService<UpdateSocialsCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.True(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateCrafterSocialsCommand_ReturnFailure()
	{
		var dto = new UpdateSocialsCrafterDto(SocialType.Telegramm, "");
		var command = new UpdateSocialsCrafterCommand(Guid.NewGuid(), [dto]);
		var validator = scope.ServiceProvider.GetRequiredService<UpdateSocialsCrafterCommandValidator>();
		var result = validator.Validate(command);
		Assert.False(result.IsValid);
	}
}