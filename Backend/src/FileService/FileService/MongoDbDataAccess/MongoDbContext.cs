using FileService.Models;
using MongoDB.Driver;

namespace FileService.MongoDbDataAccess;

public class MongoDbContext(IMongoClient mongoClient)
{
	private readonly IMongoDatabase database = mongoClient.GetDatabase("files_db");
	public IMongoCollection<FileData> Files => database.GetCollection<FileData>("files");
}
