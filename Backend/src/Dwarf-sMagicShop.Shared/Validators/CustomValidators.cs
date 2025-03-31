using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using FluentValidation;

namespace Dwarf_sMagicShop.Core.Validators;

public static class CustomValidators
{
	public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
		this IRuleBuilder<T, TElement> ruleBuilder,
		Func<TElement, Result<TValueObject, Error>> factoryMethod)
	{
		return ruleBuilder.Custom((value, context) =>
		{
			var result = factoryMethod(value);
			if (result.IsSuccess) return;
			context.AddFailure(result.Error.Serialize());
		});
	}

	public static IRuleBuilderOptionsConditions<T, TElement> CheckNullableValueIfValueNotNull<T, TElement, E>(
		this IRuleBuilder<T, TElement> ruleBuilder,
		Predicate<TElement> conditionMethod, E error) where E : Error
	{
		return ruleBuilder.Custom((value, context) =>
		{
			if (value == null) return;
			var result = conditionMethod(value);
			if (result) return;
			context.AddFailure(error.Serialize());
		});
	}
}