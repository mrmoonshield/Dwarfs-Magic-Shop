using AutoFixture;
using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.Abstractions;
using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using Dwarf_sMagicShop.Crafters.Application.Crafters;
using Dwarf_sMagicShop.Crafters.Application.Crafters.CreateCrafter;
using Dwarf_sMagicShop.Crafters.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.IntegrationTests;

public class BaseFixtureTests<TWebFactory> : IClassFixture<TWebFactory>, IAsyncLifetime where TWebFactory : BaseWebFactory
{
	protected readonly IServiceScope scope;
	protected readonly IReadDbContextCrafter readDbContextCrafters;

	protected readonly IReadDbContextSpecies readDbContextSpecies;
	protected readonly Fixture fixture;

	protected readonly TWebFactory factory;
	protected readonly ICrafterRepository crafterRepository;

	public BaseFixtureTests(TWebFactory factory)
	{
		scope = factory.Services.CreateScope();
		readDbContextCrafters = scope.ServiceProvider.GetRequiredService<IReadDbContextCrafter>();
		readDbContextSpecies = scope.ServiceProvider.GetRequiredService<IReadDbContextSpecies>();
		fixture = new Fixture();
		this.factory = factory;
		crafterRepository = scope.ServiceProvider.GetRequiredService<ICrafterRepository>();
	}

	public virtual async Task DisposeAsync()
	{
		scope.Dispose();
		await factory.ResetDatabaseAsync();
	}

	public virtual Task InitializeAsync()
	{ return Task.CompletedTask; }

	protected async Task<Result<Crafter, ErrorsList>> CreateCrafterAsync()
	{
		var crafterHandler = scope.ServiceProvider.GetRequiredService<IResultHandler<Crafter, CreateCrafterRequest>>();
		var command = fixture.Create<CreateCrafterRequest>();
		var result = await crafterHandler.ExecuteAsync(command, CancellationToken.None);
		return result;
	}
}