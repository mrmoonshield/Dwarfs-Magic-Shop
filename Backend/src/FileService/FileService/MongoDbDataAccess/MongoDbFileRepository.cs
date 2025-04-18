using CSharpFunctionalExtensions;
using FileService.ErrorsHelpers;
using FileService.Models;
using MongoDB.Driver;

namespace FileService.MongoDbDataAccess;

public class MongoDbFileRepository(MongoDbContext mongoDbContext)
{
	public async Task AddFileAsync(FileData file, CancellationToken cancellationToken)
	{
		await mongoDbContext.Files.InsertOneAsync(file, cancellationToken: cancellationToken);
	}

	public async Task<IReadOnlyCollection<FileData>> GetFileDatasAsync(
		List<Guid> fileIds,
		CancellationToken cancellationToken)
	{
		var fileDatas = await mongoDbContext.Files.Find(a => fileIds.Contains(a.Id))
			.ToListAsync(cancellationToken);

		return fileDatas;
	}

	public async Task<UnitResult<Error>> DeleteFileDatasAsync(
		List<Guid> fileIds,
		CancellationToken cancellationToken)
	{
		var result = await mongoDbContext.Files.DeleteManyAsync(a => fileIds.Contains(a.Id), cancellationToken);

		if (result.DeletedCount < fileIds.Count)
			return Errors.Failure("Not all elements where deleted");

		return Result.Success<Error>();
	}
}
