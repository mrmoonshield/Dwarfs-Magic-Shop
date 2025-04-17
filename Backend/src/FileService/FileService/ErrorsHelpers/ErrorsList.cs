using CSharpFunctionalExtensions;
using FluentValidation.Results;
using System.Collections;

namespace FileService.ErrorsHelpers;

public class ErrorsList : IEnumerable<Error>
{
	private readonly List<Error> errors;

	public ErrorsList(IEnumerable<Error> errors)
	{
		this.errors = errors.ToList();
	}

	public IEnumerator<Error> GetEnumerator()
	{
		return errors.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public static ErrorsList Create(List<ValidationFailure> failures)
	{
		var errors = failures.Select(a => Error.Deserialize(a.ErrorMessage, a.PropertyName));
		return new(errors);
	}

	public static implicit operator ErrorsList(Error error)
	{
		return new([error]);
	}
}

public static class ConvertErrorsCollectionToErrorsList
{
	public static ErrorsList ToErrorsList(this IEnumerable<Error> errors)
	{
		return new ErrorsList(errors);
	}
}

