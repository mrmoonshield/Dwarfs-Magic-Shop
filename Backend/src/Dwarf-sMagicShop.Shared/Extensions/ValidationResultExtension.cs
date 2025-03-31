using Dwarf_sMagicShop.Core.ErrorsHelpers;
using FluentValidation.Results;

namespace Dwarf_sMagicShop.Core.Extensions;

public static class ValidationResultExtension
{
	public static ErrorsList ToErrorsList(this ValidationResult validationResult)
	{
		return ErrorsList.Create(validationResult.Errors);
	}
}