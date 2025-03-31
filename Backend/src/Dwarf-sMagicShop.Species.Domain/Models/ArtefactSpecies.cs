using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;

namespace Dwarf_sMagicShop.Species.Domain.Models;

public class ArtefactSpecies : Core.Entities.Entity<ArtefactSpeciesID>
{
	private ArtefactSpecies(ArtefactSpeciesID id, string name) : base(id)
	{
		Name = name;
	}

	public string Name { get; private set; }

	public static Result<ArtefactSpecies, Error> Create(ArtefactSpeciesID id, string name)
	{
		if (id == null)
			return Errors.ValueIsNull("ID");
		if (string.IsNullOrWhiteSpace(name))
			return Errors.ValueIsEmpty("Name");

		return new ArtefactSpecies(id, name);
	}
}