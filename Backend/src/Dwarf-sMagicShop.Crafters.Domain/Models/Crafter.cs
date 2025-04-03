using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Entities;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public sealed class Crafter : SoftDeletableEntity<CrafterID>
{
	private List<MagicArtefact> artefacts = [];

	private Crafter(CrafterID id, Nickname nickname) : base(id)
	{
		Nickname = nickname;
	}

	public Nickname Nickname { get; private set; } = default!;
	public int Experience { get; private set; }
	public IReadOnlyCollection<MagicArtefact> Artefacts => artefacts;

	public static Result<Crafter, Error> Create(CrafterID id, Nickname nickname)
	{
		if (id == null)
			return Errors.ValueIsNull("ID");
		if (nickname == null)
			return Errors.ValueIsNull("Nickname");

		return new Crafter(id, nickname);
	}

	public UnitResult<Error> AddNewArtefact(MagicArtefact artefact)
	{
		if (artefact == null)
			return Errors.ValueIsNull("Magic artefact");

		if (artefacts.Contains(artefact))
			return Errors.ValueIsAlreadyExist("Magic artefact");

		var result = artefact.CreatePositionNumber(artefacts.Count + 1);

		if (result.IsSuccess)
			artefacts.Add(artefact);
		return result;
	}

	public int CalculateArtefactsCraftedAmount()
	{
		return artefacts.Count(a => a.ArtefactStatusType != ArtefactStatusType.Crafting);
	}

	public int CalculateArtefactsOnSaleAmount()
	{
		return artefacts.Count(a => a.ArtefactStatusType == ArtefactStatusType.OnSale);
	}

	public int CalculateArtefactsInRentAmount()
	{
		return artefacts.Count(a => a.ArtefactStatusType == ArtefactStatusType.InRent);
	}

	public int CalculateArtefactsSoldAmount()
	{
		return artefacts.Count(a => a.ArtefactStatusType == ArtefactStatusType.Sold);
	}

	public Result<MagicArtefact, Error> GetArtefactById(MagicArtefactID id)
	{
		var artefact = artefacts.Find(a => a.Id == id);

		if (artefact == null)
			return Errors.NotFound(id.Value);
		return artefact;
	}

	public Result<MagicArtefact, Error> GetArtefactByPos(int pos)
	{
		if (artefacts.Count == 0)
			return Errors.NotFound("Item in position");

		var artefact = artefacts.Find(a => a.Position.Value == pos);

		if (artefact == null)
			return Errors.NotFound("Item in position");

		return artefact;
	}

	public void Update(Nickname nickname, int experience)
	{
		Nickname = nickname;
		Experience = experience;
	}

	public override void Delete()
	{
		base.Delete();

		foreach (var item in artefacts)
		{
			item.Delete();
		}
	}

	public override void Restore()
	{
		base.Restore();

		foreach (var item in artefacts)
		{
			item.Restore();
		}
	}

	public UnitResult<Error> MoveArtefact(MagicArtefact artefact, int toPos)
	{
		if (toPos < 1 || toPos > artefacts.Count)
			return Errors.ValueIsInvalid("Target position");

		artefacts.Remove(artefact);

		if (toPos < artefacts.Count)
			artefacts.Insert(toPos - 1, artefact);
		else artefacts.Add(artefact);

		for (int i = 0; i < artefacts.Count; i++)
		{
			artefacts[i].CreatePositionNumber(i + 1);
		}

		return Result.Success<Error>();
	}

	public void RemoveArtefact(MagicArtefact artefact)
	{
		artefacts.Remove(artefact);
	}
}