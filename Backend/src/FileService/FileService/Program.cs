using Amazon.S3;
using FileService.Endpoints;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmazonS3>(a =>
{
	var config = new AmazonS3Config
	{
		ServiceURL = "http://localhost:9000",
		ForcePathStyle = true,
		UseHttp = true
	};

	return new AmazonS3Client("minioadmin", "minioadmin", config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapEndpoints();
app.Run();
