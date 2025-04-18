using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FileService.Models;

public class FileData
{
	[BsonId]
	[BsonGuidRepresentation(GuidRepresentation.Standard)]
	public Guid Id { get; init; }
	public string Path { get; init; } = string.Empty;
	public DateTime UploadDate { get; init; }
	public long FileSize { get; init; }
	public string ContentType { get; init; } = string.Empty;
}
