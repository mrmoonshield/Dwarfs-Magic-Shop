using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Shared;

namespace FileService.Features;

public static class StartMultipartUpload
{
	private record MultipartUploadRequest(string FileName, string ContentType, long Size);

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("files/{key:guid}/multipart", ExecuteAsync);
		}
	}

	private static async Task<IResult> ExecuteAsync(
		MultipartUploadRequest request,
		Guid key,
		IAmazonS3 amazonS3,
		CancellationToken cancellationToken)
	{
		try
		{
			var initiateMultipartUploadRequest = new InitiateMultipartUploadRequest
			{
				BucketName = Constants.BUCKET_NAME_VIDEOS,
				Key = key.ToString()
			};

			var response = await amazonS3.InitiateMultipartUploadAsync(
				initiateMultipartUploadRequest,
				cancellationToken);

			return Results.Ok(new { key, response.UploadId });
		}
		catch (AmazonS3Exception ex)
		{
			return Results.BadRequest($"Multipart uploading failed: {ex}");
		}
	}
}
