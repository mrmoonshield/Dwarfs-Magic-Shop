using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Dwarf_sMagicShop.Crafters.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
	private readonly IMinioClient minioClient;
	private readonly ILogger<MinioProvider> logger;

	public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
	{
		this.minioClient = minioClient;
		this.logger = logger;
	}

	public async Task<Result<string, Error>> UploadFileAsync(
		FileUploadCommand request,
		CancellationToken cancellationToken)
	{
		try
		{
			await CheckBucket(request, cancellationToken);

			var args = new PutObjectArgs()
				.WithBucket(request.bucketName)
				.WithStreamData(request.stream)
				.WithObjectSize(request.stream.Length)
			.WithObject(request.objectName);

			var result = await minioClient.PutObjectAsync(args, cancellationToken);
			return request.objectName;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "File {file} can not be uploaded", request.objectName);
			return Errors.InvalidOperation("File can not be uploaded");
		}
	}

	private async Task CheckBucket(FileUploadCommand request, CancellationToken cancellationToken)
	{
		var bucketExistsArgs = new BucketExistsArgs().WithBucket(request.bucketName);
		var exist = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

		if (!exist)
		{
			var makeBucketArgs = new MakeBucketArgs().WithBucket(request.bucketName);
			await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
		}
	}

	public async Task<UnitResult<Error>> DeleteFileAsync(
		string fileName,
		string bucketName, CancellationToken cancellationToken)
	{
		var statArgs = new StatObjectArgs()
			.WithBucket(bucketName)
			.WithObject(fileName);

		var exist = await minioClient.StatObjectAsync(statArgs, cancellationToken);
		if (exist == null)
			return Result.Success<Error>();

		try
		{
			var removeObjectArgs = new RemoveObjectArgs()
				.WithBucket(bucketName)
				.WithObject(fileName);

			await minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);
			logger.LogInformation("File {file} in {bucketName} has been deleted", fileName, bucketName);
			return Result.Success<Error>();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Fail to delete file {fileName} in {bucketName}", fileName, bucketName);
			return Errors.InvalidOperation($"Fail to delete file {fileName} in {bucketName}");
		}
	}

	public async Task<Result<string, Error>> ReplaceFileAsync(
		FileUploadCommand command,
		string oldFileName,
		CancellationToken cancellationToken)
	{
		await DeleteFileAsync(oldFileName, command.bucketName, cancellationToken);
		var result = await UploadFileAsync(command, cancellationToken);
		return result;
	}

	public async Task<Result<string, Error>> GetFileAsync(string bucketName, string fileName)
	{
		try
		{
			var args = new PresignedGetObjectArgs()
				.WithBucket(bucketName)
				.WithObject(fileName);

			var result = await minioClient.PresignedGetObjectAsync(args);
			return result;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "File can not be recieved");
			return Error.Failure("GetFile.failure", "File can not be recieved");
		}
	}
}