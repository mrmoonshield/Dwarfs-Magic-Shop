using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Shared;

namespace FileService.Features;

public static class GetDownloadPresignedUrl
{
	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("files/{key:guid}/presigned-url", ExecuteAsync);
		}
	}

	private static async Task<IResult> ExecuteAsync(
		IAmazonS3 amazonS3,
		Guid key)
	{
		try
		{
			var getPreSignedUrlRequest = new GetPreSignedUrlRequest
			{
				BucketName = Constants.BUCKET_NAME_VIDEOS,
				Key = key.ToString(),
				Expires = DateTime.UtcNow.AddHours(24),
				Verb = HttpVerb.GET,
				Protocol = Protocol.HTTP,
			};

			var url = await amazonS3.GetPreSignedURLAsync(getPreSignedUrlRequest);
			return Results.Ok(new { key, url });
		}
		catch (AmazonS3Exception ex)
		{
			return Results.BadRequest($"Generating presigned url failed: {ex}");
		}
	}
}
