using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Shared;

namespace FileService.Features;

public static class GetMultipartUploadPresignedUrl
{
	private record UploadPresignedRequest(string UploadId, int PartNumber);

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("files/multipart-presigned-url", ExecuteAsync);
		}
	}

	private static async Task<IResult> ExecuteAsync(
		UploadPresignedRequest request,
		IAmazonS3 amazonS3)
	{
		try
		{
			var key = Guid.NewGuid();

			var getPreSignedUrlRequest = new GetPreSignedUrlRequest
			{
				BucketName = Constants.BUCKET_NAME_VIDEOS,
				Key = key.ToString(),
				Expires = DateTime.UtcNow.AddHours(24),
				Verb = HttpVerb.PUT,
				Protocol = Protocol.HTTP,
				UploadId = request.UploadId,
				PartNumber = request.PartNumber,
			};

			var url = await amazonS3.GetPreSignedURLAsync(getPreSignedUrlRequest);
			return Results.Ok(new { key, url });
		}
		catch (AmazonS3Exception ex)
		{
			return Results.BadRequest($"Generating multipart presigned url failed: {ex}");
		}
	}
}
