namespace Dwarf_sMagicShop.Crafters.Infrastructure.SettingsModels;

public class MinioSettings
{
	public string Endpoint { get; init; } = string.Empty;
	public string AccessKey { get; init; } = string.Empty;
	public string SecretKey { get; init; } = string.Empty;
	public bool WithSsl { get; init; } = false;

	public static Exception GetException()
	{
		return new ApplicationException("Minio configuration not found");
	}
}