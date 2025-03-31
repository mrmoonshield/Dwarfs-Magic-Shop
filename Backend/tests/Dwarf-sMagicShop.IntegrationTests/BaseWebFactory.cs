using Dwarf_sMagicShop.Core.Database;
using Dwarf_sMagicShop.Crafters.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace Dwarf_sMagicShop.IntegrationTests;

public class BaseWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
	protected PostgreSqlContainer container = new PostgreSqlBuilder()
			.WithImage("postgres")
			.WithDatabase("Dwarf-sMagicShopTests")
			.WithUsername("postgres")
			.WithPassword("postgres")
			.Build();

	protected Respawner respawner = default!;
	protected NpgsqlConnection dbConnection = default!;

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureTestServices(ConfigureTestServices);
	}

	protected virtual void ConfigureTestServices(IServiceCollection services)
	{
		AddDbContexts(services);
	}

	private void AddDbContexts(IServiceCollection collection)
	{
		collection.RemoveAll(typeof(IReadDbContextCrafter));
		collection.RemoveAll(typeof(WriteDbContextCrafters));

		collection.AddScoped<WriteDbContextCrafters>(a => new WriteDbContextCrafters(container.GetConnectionString()));
		collection.AddScoped<IReadDbContextCrafter, ReadDbContextCrafters>(a => new ReadDbContextCrafters(container.GetConnectionString()));

		//collection.AddScoped<WriteDbContextSpecies>(a => new WriteDbContextSpecies(container.GetConnectionString()));
		//collection.AddScoped<IReadDbContextSpecies, ReadDbContextSpecies>(a => new ReadDbContextSpecies(container.GetConnectionString()));
	}

	public virtual async Task InitializeAsync()
	{
		await container.StartAsync();
		using var scope = Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContextCrafters>();
		await dbContext.Database.EnsureCreatedAsync();
		await InitializeRespawnerAsync();
	}

	private async Task InitializeRespawnerAsync()
	{
		dbConnection = new NpgsqlConnection(container.GetConnectionString());
		await dbConnection.OpenAsync();

		respawner = await Respawner.CreateAsync(dbConnection, new RespawnerOptions
		{
			DbAdapter = DbAdapter.Postgres,
		});
	}

	public virtual async Task ResetDatabaseAsync()
	{
		await respawner.ResetAsync(dbConnection);
	}

	public virtual new async Task DisposeAsync()
	{
		await container.StopAsync();
		await container.DisposeAsync();
	}
}