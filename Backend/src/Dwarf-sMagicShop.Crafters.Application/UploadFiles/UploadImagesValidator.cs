using Dwarf_sMagicShop.Core.Validators;
using FluentValidation;

namespace Dwarf_sMagicShop.Crafters.Application.UploadFiles;

public class UploadImagesValidator : AbstractValidator<FileUploadCommand>, ICustomValidator
{
	public UploadImagesValidator()
	{
		RuleFor(a => a.objectName).NotEmpty();
		RuleFor(a => a.objectName).Must(CheckFileExtension).WithMessage("File format must be image");
		RuleFor(a => a.stream).Must(s => s.Length <= 10485760);
	}

	private bool CheckFileExtension(string fileExtension)
	{
		var extension = Path.GetExtension(fileExtension.ToLower());
		if (extension == null) return false;

		switch (extension)
		{
			case ".jpg": return true;
			case ".jpeg": return true;
			case ".png": return true;
			case ".bmp": return true;
			case ".tiff": return true;
			case ".gif": return true;
			case ".webp": return true;
			case ".heic": return true;
			case ".heif": return true;
			default: return false;
		}
	}
}