using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Shared;

namespace FileService.Features;

public static class CompleteMultipartUpload
{
	private record CompleteMultipartRequest(string UploadId, List<PartEtagInfo> Parts);
	private record PartEtagInfo(int PartNumber, string ETag);

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("files/{key:guid}/complete-multipart-upload", ExecuteAsync);
		}
	}

	private static async Task<IResult> ExecuteAsync(
		CompleteMultipartRequest request,
		Guid key,
		IAmazonS3 amazonS3,
		CancellationToken cancellationToken)
	{
		try
		{
			var completeMultipartUploadRequest = new CompleteMultipartUploadRequest
			{
				BucketName = Constants.BUCKET_NAME_VIDEOS,
				Key = key.ToString(),
				UploadId = request.UploadId,
				PartETags = request.Parts.Select(a => new PartETag(a.PartNumber, a.ETag)).ToList()
			};

			var url = await amazonS3.CompleteMultipartUploadAsync(completeMultipartUploadRequest, cancellationToken);
			return Results.Ok(key);
		}
		catch (AmazonS3Exception ex)
		{
			return Results.BadRequest($"Complete multipart uploading failed: {ex}");
		}
	}
}
