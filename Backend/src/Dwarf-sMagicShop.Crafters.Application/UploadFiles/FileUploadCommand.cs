namespace Dwarf_sMagicShop.Crafters.Application.UploadFiles;

public record FileUploadCommand(Stream stream, string bucketName, string objectName);