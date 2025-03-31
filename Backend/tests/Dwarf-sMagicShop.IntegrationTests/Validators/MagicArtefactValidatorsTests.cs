using AutoFixture;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Create;
using Dwarf_sMagicShop.Crafters.Application.MagicArtefacts.Update.Info;
using Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests.Validators;

public class MagicArtefactValidatorsTests : MagicArtefactFixtureTests
{
	public MagicArtefactValidatorsTests(MagicArtefactsWebFactory factory) : base(factory)
	{
	}

	[Fact]
	public void Validate_CreateMagicArtefactCommandWithWrongName_ReturnFailure()
	{
		var request = fixture.Build<UpdateMagicArtefactRequest>()
			.With(a => a.Name, "")
			.Create();

		var command = new CreateMagicArtefactCommand(Guid.NewGuid(), request);
		var validator = scope.ServiceProvider.GetRequiredService<CreateMagicArtefactValidator>();
		var result = validator.Validate(command);

		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_CreateMagicArtefactCommandWithWrongDescription_ReturnFailure()
	{
		var request = fixture.Build<UpdateMagicArtefactRequest>()
			.With(a => a.Description, "")
			.Create();

		var command = new CreateMagicArtefactCommand(Guid.NewGuid(), request);
		var validator = scope.ServiceProvider.GetRequiredService<CreateMagicArtefactValidator>();
		var result = validator.Validate(command);

		Assert.False(result.IsValid);
	}

	[Fact]
	public void Validate_UpdateMagicArtefactRequestWithWrongSpecies_ReturnFailure()
	{
		const string INPUT_STRING = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

		var request = fixture.Build<UpdateMagicArtefactRequest>()
			.With(a => a.Species, INPUT_STRING)
			.Create();

		var validator = scope.ServiceProvider.GetRequiredService<UpdateMagicArtefactValidator>();
		var result = validator.Validate(request);

		Assert.False(result.IsValid);
	}
}