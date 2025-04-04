using Dwarf_sMagicShop.Accounts.Infrastructure;
using Dwarf_sMagicShop.API;
using Dwarf_sMagicShop.API.Middlewares;
using Dwarf_sMagicShop.Crafters.Application;
using Dwarf_sMagicShop.Crafters.Infrastructure;
using Dwarf_sMagicShop.Species.Application;
using Dwarf_sMagicShop.Species.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()
	.WriteTo.Debug()
	.WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
		?? throw new ArgumentNullException("Seq"))
	.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
	.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
	.CreateLogger();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSerilog();
builder.Services.AddAuthorization();

builder.Services
	.AddApi()
	.AddApplicationCrafters()
	.AddInfrastructureCrafters(builder.Configuration)
	.AddApplicationSpecies()
	.AddInfrastructureSpecies(builder.Configuration)
	.AddApplicationAccounts()
	.AddInfrastructureAccounts();

builder
	.AddInfrastructureCraftersBuilder()
	.AddInfrastructureAccountsBuilder();

var app = builder.Build();
app.UseExeptionsHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program;