using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Core.Validators;

public class ValidatorsProvider
{
	private readonly IServiceProvider serviceProvider;

	public ValidatorsProvider(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}

	public TValidator GetValidator<TValidator>() where TValidator : ICustomValidator
	{
		return serviceProvider.GetRequiredService<TValidator>();
	}
}