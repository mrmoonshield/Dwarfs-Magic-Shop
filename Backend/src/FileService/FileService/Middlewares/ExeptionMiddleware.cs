using FileService.ErrorsHelpers;
using FileService.Response;

namespace FileService.Middlewares;

public class ExeptionMiddleware
{
	private readonly RequestDelegate next;
	private readonly ILogger<ExeptionMiddleware> logger;

	public ExeptionMiddleware(RequestDelegate next, ILogger<ExeptionMiddleware> logger)
	{
		this.next = next;
		this.logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, ex.Message);
			var errorsList = Error.Failure("internal.server.error", ex.Message).ToErrorsList();
			var envelop = Envelope.Error(errorsList);
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			await context.Response.WriteAsJsonAsync(envelop);
		}
	}
}

public static class ExeptionMiddlewareExtensions
{
	public static IApplicationBuilder UseExeptionsHandler(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ExeptionMiddleware>();
	}
}