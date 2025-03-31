using Dwarf_sMagicShop.Crafters.Application.FileProvider;
using Dwarf_sMagicShop.Crafters.Application.UploadFiles;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Dwarf_sMagicShop.IntegrationTests.MagicArtefactTests;

public class MagicArtefactsWebFactory : BaseWebFactory
{
	protected override void ConfigureTestServices(IServiceCollection services)
	{
		base.ConfigureTestServices(services);
		AddFileProvider(services);
	}

	private static void AddFileProvider(IServiceCollection collection)
	{
		var fileProviderMock = Substitute.For<IFileProvider>();

		fileProviderMock.UploadFileAsync(Arg.Any<FileUploadCommand>(), Arg.Any<CancellationToken>())
			.Returns("file.jpg");

		collection.AddScoped(a => fileProviderMock);
	}
}