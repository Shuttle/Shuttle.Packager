using Microsoft.Extensions.Options;
using Shuttle.Packager.WebApi;
using Shuttle.Packager.WebApi.Endpoints;
using Shuttle.Packager.WebApi.Repositories;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.AddSwaggerGen();
webApplicationBuilder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

webApplicationBuilder.Services.Configure<PackagerOptions>(webApplicationBuilder.Configuration.GetSection(PackagerOptions.SectionName));

webApplicationBuilder.Services
    .AddSingleton<IValidateOptions<PackagerOptions>, PackagerOptionsValidator>()
    .AddSingleton<IProjectRepository, InMemoryProjectRepository>();

webApplicationBuilder.Services.AddCors(options =>
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

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.MapProjectEndpoints();

app.Run();