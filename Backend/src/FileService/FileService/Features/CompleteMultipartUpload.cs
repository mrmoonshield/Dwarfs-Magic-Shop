using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Models;
using FileService.MongoDbDataAccess;
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
		MongoDbFileRepository mongoDbFileRepository,
		CancellationToken cancellationToken)
	{
		try
		{
			var fileId = Guid.NewGuid();

			var completeMultipartUploadRequest = new CompleteMultipartUploadRequest
			{
				BucketName = Constants.BUCKET_NAME_VIDEOS,
				Key = key.ToString(),
				UploadId = request.UploadId,
				PartETags = request.Parts.Select(a => new PartETag(a.PartNumber, a.ETag)).ToList()
			};

			var uploadResponse = await amazonS3.CompleteMultipartUploadAsync(completeMultipartUploadRequest, cancellationToken);
			var metadataResponse = await amazonS3.GetObjectMetadataAsync(Constants.BUCKET_NAME_VIDEOS, key.ToString(), cancellationToken);

			var fileData = new FileData
			{
				Id = fileId,
				ContentType = metadataResponse.Headers.ContentType,
				UploadDate = DateTime.UtcNow,
				FileSize = uploadResponse.ContentLength,
				Path = key.ToString()
			};

			await mongoDbFileRepository.AddFileAsync(fileData, cancellationToken);
			return Results.Ok(key);
		}
		catch (AmazonS3Exception ex)
		{
			return Results.BadRequest($"Complete multipart uploading failed: {ex}");
		}
	}
}
