using Dwarf_sMagicShop.Crafters.Domain.Models;

namespace Dwarf_sMagicShop.UnitTests;

public class CrafterTests
{
	private void AddArtefactsToCrafter(Crafter crafter, int count)
	{
		for (var i = 0; i < count; i++)
		{
			var magicArtefactID = MagicArtefactID.NewMagicArtefactID;
			var artefact = MagicArtefact.Create(magicArtefactID, $"Artefact{i}", "desc").Value;
			crafter.AddNewArtefact(artefact);
		}
	}

	[Fact]
	public void AddNewArtefact_AddFirstItem_ReturnSuccessResult()
	{
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		var magicArtefactID = MagicArtefactID.NewMagicArtefactID;
		var artefact = MagicArtefact.Create(magicArtefactID, "Artefact", "desc").Value;
		var result = crafter.AddNewArtefact(artefact);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public void AddNewArtefact_CheckArtefactPositionEquals1_ReturnSuccessResult()
	{
		const int FIRST_POS = 1;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		var magicArtefactID = MagicArtefactID.NewMagicArtefactID;
		var artefact = MagicArtefact.Create(magicArtefactID, "Artefact", "desc").Value;
		crafter.AddNewArtefact(artefact);

		Assert.Equal(FIRST_POS, artefact.Position.Value);
	}

	[Fact]
	public void MoveArtefact_MoveFromEndToPos1_ReturnSuccessResult()
	{
		const int FIRST_POS = 1;
		const int ARTEFACT_AMOUNT = 5;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		AddArtefactsToCrafter(crafter, ARTEFACT_AMOUNT);
		var artefact = crafter.GetArtefactByPos(crafter.Artefacts.Count).Value;
		var moveResult = crafter.MoveArtefact(artefact, 1);

		Assert.Equal(FIRST_POS, artefact.Position.Value);
	}

	[Fact]
	public void MoveArtefact_MoveFromEndToPos0_ReturnFailureResult()
	{
		const int ARTEFACT_AMOUNT = 5;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		AddArtefactsToCrafter(crafter, ARTEFACT_AMOUNT);
		var artefact = crafter.GetArtefactByPos(crafter.Artefacts.Count).Value;
		var moveResult = crafter.MoveArtefact(artefact, 0);

		Assert.True(moveResult.IsFailure);
	}

	[Fact]
	public void MoveArtefact_MoveFromEndToPos3_ReturnSuccessResult()
	{
		const int ARTEFACT_AMOUNT = 5;
		const int TARGET_POS = 3;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		AddArtefactsToCrafter(crafter, ARTEFACT_AMOUNT);
		var artefact = crafter.GetArtefactByPos(crafter.Artefacts.Count).Value;
		var moveResult = crafter.MoveArtefact(artefact, TARGET_POS);

		Assert.Equal(TARGET_POS, artefact.Position.Value);
	}

	[Fact]
	public void MoveArtefact_MoveFromPos1ToEnd_ReturnSuccessResult()
	{
		const int FIRST_POS = 1;
		const int ARTEFACT_AMOUNT = 5;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		AddArtefactsToCrafter(crafter, ARTEFACT_AMOUNT);
		var artefact = crafter.GetArtefactByPos(FIRST_POS).Value;
		var moveResult = crafter.MoveArtefact(artefact, crafter.Artefacts.Count);

		Assert.Equal(ARTEFACT_AMOUNT, artefact.Position.Value);
	}

	[Fact]
	public void MoveArtefact_MoveFromPos1ToOutOfRange_ReturnFailureResult()
	{
		const int FIRST_POS = 1;
		const int ARTEFACT_AMOUNT = 5;
		var crafter = Crafter.Create(CrafterID.NewCrafterID, Nickname.Create("Nick").Value).Value;
		AddArtefactsToCrafter(crafter, ARTEFACT_AMOUNT);
		var artefact = crafter.GetArtefactByPos(FIRST_POS).Value;
		var moveResult = crafter.MoveArtefact(artefact, ARTEFACT_AMOUNT + 1);

		Assert.True(moveResult.IsFailure);
	}
}