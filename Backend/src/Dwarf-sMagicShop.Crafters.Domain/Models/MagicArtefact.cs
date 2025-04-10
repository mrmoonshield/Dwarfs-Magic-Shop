using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Entities;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public class MagicArtefact : SoftDeletableEntity<MagicArtefactID>
{
	public MagicArtefact(MagicArtefactID id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		CreationDate = DateTime.UtcNow;
	}

	public string Name { get; private set; }
	public string Description { get; private set; }
	public Guid? SpeciesId { get; private set; } = default!;
	public string Effect { get; private set; } = default!;
	public ArtefactRareType Rare { get; private set; } = default!;
	public string Location { get; private set; } = default!;
	public float Weight { get; private set; } = default!;
	public HandedAmountType HandedAmountType { get; private set; }
	public ArtefactStatusType ArtefactStatusType { get; private set; }
	public DateTime CreationDate { get; private set; }
	public File ImageFile { get; private set; } = default!;
	public Position Position { get; private set; } = default!;

	public static Result<MagicArtefact, Error> Create(MagicArtefactID id, string name, string description)
	{
		if (id == null)
			return Errors.ValueIsNull("ID");
		if (string.IsNullOrWhiteSpace(name))
			return Errors.ValueIsEmpty("Name");
		if (string.IsNullOrWhiteSpace(description))
			return Errors.ValueIsEmpty("Description");

		return new MagicArtefact(id, name, description);
	}

	public UnitResult<Error> CreatePositionNumber(int position)
	{
		var result = Position.Create(position);

		if (result.IsSuccess)
		{
			Position = result.Value;
			return Result.Success<Error>();
		}

		return result.Error;
	}

	public void AddImageFile(File file) => ImageFile = file;

	public void UpdateInfo(
		Guid? speciesID,
		string? effect,
		ArtefactRareType? rareType,
		string? location,
		float? weight,
		HandedAmountType? handedAmountType,
		string? name = null,
		string? description = null)
	{
		if (speciesID != null) SpeciesId = speciesID;
		if (effect != null) Effect = effect;
		if (location != null) Location = location;
		if (rareType != null) Rare = rareType.Value;
		if (handedAmountType != null) HandedAmountType = handedAmountType.Value;
		if (weight != null) Weight = weight.Value;
		if (name != null) Name = name;
		if (description != null) Description = description;
	}

	public void UpdateStatus(ArtefactStatusType status) => ArtefactStatusType = status;
}