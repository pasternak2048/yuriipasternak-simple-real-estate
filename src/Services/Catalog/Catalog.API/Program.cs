using Catalog.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();

app.UseCustomMiddlewares();

app.MapControllers();

app.UseCors("AllowFromGateway");

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API v1");
		options.RoutePrefix = string.Empty;
	});

	await app.InitializeDatabaseAsync();
}

app.Run();
