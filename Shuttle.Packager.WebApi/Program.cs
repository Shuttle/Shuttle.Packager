using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Shuttle.Packager.WebApi;
using Shuttle.Packager.WebApi.Endpoints;
using Shuttle.Packager.WebApi.Repositories;

var webApplicationBuilder = WebApplication.CreateBuilder(args);
var services = webApplicationBuilder.Services;
var configuration = webApplicationBuilder.Configuration;

services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

services
    .Configure<PackagerOptions>(configuration.GetSection(PackagerOptions.SectionName))
    .AddSingleton<IValidateOptions<PackagerOptions>, PackagerOptionsValidator>()
    .AddSingleton<IProjectRepository, InMemoryProjectRepository>()
    .AddHttpClient()
    .AddEndpointsApiExplorer()
    .AddOpenApi(options =>
    {
        options.AddSchemaTransformer((schema, _, _) =>
        {
            schema.Title = schema.Title?.Replace("+", "_");
            return Task.CompletedTask;
        });
    })
    .AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

var app = webApplicationBuilder.Build();

app.UseCors();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("Shuttle Packager API")
        .WithTheme(ScalarTheme.DeepSpace)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app
    .MapPackageSourceEndpoints()
    .MapProjectEndpoints();

app.Run();