using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Enums;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Dwarf_sMagicShop.Crafters.Domain.Models;

public record Social
{
	[JsonConstructor]
	private Social()
	{
	}

	private Social(SocialType socialType, string reference)
	{
		SocialType = socialType;
		Reference = reference;
	}

	public SocialType SocialType { get; }
	public string Reference { get; } = default!;

	public static Result<Social, Error> Create(SocialType type, string reference)
	{
		if (string.IsNullOrWhiteSpace(reference))
			return Errors.ValueIsEmpty("Reference");

		string url = reference.Trim();
		Regex regex = new Regex(@"^((http|https):\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[$$@!\$&'$$\*\+,;=.]*$");

		if (!regex.IsMatch(url))
		{
			return Errors.ValueIsInvalid("Reference");
		}

		return new Social(type, reference);
	}
}