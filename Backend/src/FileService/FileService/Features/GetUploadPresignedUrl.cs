﻿using Amazon.S3;
using Amazon.S3.Model;
using FileService.Endpoints;
using FileService.Shared;

namespace FileService.Features;

public static class GetUploadPresignedUrl
{
	private record UploadPresignedRequest(string FileName, string ContentType, long Size);

	public sealed class Endpoint : IEndpoint
	{
		public void MapEndpoint(IEndpointRouteBuilder app)
		{
			app.MapPost("files/presigned-url", ExecuteAsync);
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
				ContentType = request.ContentType,
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
