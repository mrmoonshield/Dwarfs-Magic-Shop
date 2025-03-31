using Dwarf_sMagicShop.API.Response;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Microsoft.AspNetCore.Mvc;

namespace Dwarf_sMagicShop.API.Extensions;

public static class ResponseExtensions
{
	public static ActionResult ToResponse(this ErrorsList errors)
	{
		if (!errors.Any())
			return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };

		var distinctTypes = errors.Select(a => a.ErrorType).Distinct().ToList();

		var statusCode = distinctTypes.Count() > 1
			? StatusCodes.Status500InternalServerError
			: CalculateStatusCode(distinctTypes.First());

		var envelop = Envelope.Error(errors);
		return new ObjectResult(envelop) { StatusCode = statusCode };
	}

	private static int CalculateStatusCode(ErrorType errorType)
	{
		var statusCode = errorType switch
		{
			ErrorType.Empty => StatusCodes.Status400BadRequest,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Failure => StatusCodes.Status500InternalServerError,
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			_ => StatusCodes.Status500InternalServerError,
		};

		return statusCode;
	}

	//public static ActionResult ToValidationErrorResponse(this ValidationResult result)
	//{
	//	if (result.IsValid)
	//		throw new InvalidOperationException("Result can not be succeed");

	//	var validationErrors = result.Errors;

	//	var responseErrors = from validationError in validationErrors
	//						 let errorMessage = validationError.ErrorMessage
	//						 let error = Error.Deserialize(errorMessage)
	//						 select new ResponseError(error.Code, error.Message, validationError.PropertyName);

	//	var envelope = Envelope.Error(responseErrors);
	//	return new ObjectResult(envelope) { StatusCode = StatusCodes.Status400BadRequest };
	//}
}