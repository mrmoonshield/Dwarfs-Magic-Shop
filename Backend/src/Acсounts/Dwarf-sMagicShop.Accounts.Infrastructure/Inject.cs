using Dwarf_sMagicShop.Accounts.Application.Abstracts;
using Dwarf_sMagicShop.Accounts.Application.RequirementsHandlers;
using Dwarf_sMagicShop.Accounts.Domain.Models;
using Dwarf_sMagicShop.Accounts.Domain.Requirements;
using Dwarf_sMagicShop.Accounts.Infrastructure.DbContexts;
using Dwarf_sMagicShop.Accounts.Infrastructure.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dwarf_sMagicShop.Accounts.Infrastructure;

public static class Inject
{
	public static IServiceCollection AddInfrastructureAccounts(this IServiceCollection services)
	{
		services
			.AddScoped<AccountDbContext>()
			.AddScoped<ITokenProvider, JwtTokenProvider>()
			.AddIdentity()
			.AddAutorizationService()
			.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();

		return services;
	}

	public static WebApplicationBuilder AddInfrastructureAccountsBuilder(this WebApplicationBuilder builder)
	{
		builder.Services.Configure<JwtSettings>(
			builder.Configuration.GetSection(nameof(JwtSettings)));

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
				var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
				?? throw JwtSettings.GetException();

				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
					ValidateLifetime = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true
				};
			});
	}

	private static IServiceCollection AddAutorizationService(this IServiceCollection services)
	{
		return services.AddAuthorization(options =>
			{
				//options.DefaultPolicy = new AuthorizationPolicyBuilder()
				//	.RequireClaim("Role", "User")
				//	.RequireAuthenticatedUser()
				//	.Build();

				options.AddPolicy("CreateCrafterRequirement", policy =>
				{
					policy.AddRequirements(new PermissionRequirement("Crafter"));
				});
			});
	}
}