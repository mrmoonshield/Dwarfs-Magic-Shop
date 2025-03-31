using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;

namespace Dwarf_sMagicShop.Crafters.Application.FileProvider;

public interface IFileProvider
{
	Task<UnitResult<Error>> DeleteFileAsync(string fileName, string bucketName, CancellationToken cancellationToken);

	Task<Result<string, Error>> GetFileAsync(string bucketName, string fileName);

	Task<Result<string, Error>> ReplaceFileAsync(
		FileUploadCommand command,
		string oldFileName,
		CancellationToken cancellationToken);

	Task<Result<string, Error>> UploadFileAsync(
		FileUploadCommand fileUploadCommand,
		CancellationToken cancellationToken);
}