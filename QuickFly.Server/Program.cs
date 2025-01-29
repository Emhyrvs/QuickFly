using Carter;
using Microsoft.OpenApi.Models;
using QuickFly.Server.Shared.JsonHelper;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:52543", "http://localhost:5000", "https://your-swagger-url.com", "https://127.0.0.1:52543")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
builder.Services.AddCarter();
builder.Services.AddScoped<IJsonFileHelper, JsonFileHelper>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();



app.MapFallbackToFile("/index.html");

app.Run();