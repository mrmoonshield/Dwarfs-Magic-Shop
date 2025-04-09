using Dwarf_sMagicShop.Accounts.Application;
using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.Check;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Domain.Settings;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Accounts.Infrastructure.Providers;
using Dwarf_sMagicShop.Accounts.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dwarf_sMagicShop.Accounts.Infrastructure;

public static class Inject
{
	public static IServiceCollection AddInfrastructureAccounts(this IServiceCollection services)
	{
		services
			.AddIdentity()
			.AddScoped<AccountDbContext>()
			.AddScoped<IAccountRepository, AccountRepository>()
			.AddScoped<ITokenProvider, JwtTokenProvider>()
			.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
			.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>()
			.AddAuthorization()
			.AddSingleton<AccountsSeeder>();

		return services;
	}

	public static WebApplicationBuilder AddInfrastructureAccountsBuilder(this WebApplicationBuilder builder)
	{
		builder.Services
			.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)))
			.Configure<RefreshSettings>(builder.Configuration.GetSection(nameof(RefreshSettings)))
			.Configure<AdminSettings>(builder.Configuration.GetSection(AdminSettings.ADMIN));

		AddJwtBearerService(builder);
		return builder;
	}

	private static IServiceCollection AddIdentity(this IServiceCollection services)
	{
		services.AddIdentity<User, Role>(options =>
				{
					options.Password.RequireNonAlphanumeric = false;
				})
				.AddEntityFrameworkStores<AccountDbContext>()
				.AddDefaultTokenProviders();

		return services;
	}

	private static void AddJwtBearerService(WebApplicationBuilder builder)
	{
		builder.Services
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.MapInboundClaims = false;
				var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
				?? throw JwtSettings.GetException();

				options.TokenValidationParameters = JwtParametersFactory
				.GetTokenValidationParameters(jwtSettings, true);
			});
	}
}