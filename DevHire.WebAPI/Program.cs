using ServiceContracts;
using Services;
using RepositoryContracts;
using Repositories;
using Microsoft.EntityFrameworkCore;
using Entities;
using DBContext;
using Mapping;
using Serilog;
using Middleware;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Replace built-in logging with Serilog
builder.Host.UseSerilog();

// Read configuration from appsettings.json for Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();




//Adding Http Logging Option
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Request |
    Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Response;
});

//Registering Developer Service
builder.Services.AddTransient<IDevelopersService, DevelopersService>();

//Registering Developers Repository
builder.Services.AddTransient<IDevelopersRepository, DevelopersRepository>();

//Adding DB Context for EF core
builder.Services.AddDbContext<DevelopersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Register AutoMapper   
builder.Services.AddAutoMapper(typeof(DeveloperProfile));

//Add Controllers
builder.Services.AddControllers( options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddNewtonsoftJson();



// Step 1: Add API versioning
var apiVersioningBuilder = builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Show supported API versions in response headers
    options.AssumeDefaultVersionWhenUnspecified = true; // Assume v1 when no version is specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default API version
    options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Read version from URL
    options.ApiVersionReader = new HeaderApiVersionReader("api-version"); // Read version from header 
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version"); // Read version from query string
});

// Step 2: Add API Explorer (Fixing the issue)
apiVersioningBuilder.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Step 3: Add Swagger with API versioning
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = $"My API {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        });
    }
});

// Adding CORS: localhost:4200
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy( policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4201" , "http://localhost:4202", "https://your-production-url.com") // Change for production
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();




// This middleware is tailored for development environments,
// presenting a detailed error page when exceptions arise, facilitating efficient debugging.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    //Swagger Middleware
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");

    });
}
else
{
    //app.UseExceptionHandler(); // In Build exception handling Middleware
    app.UseExceptionHandlingMiddleware();//Using Extension Method for Custom exception handling Middleware
}

app.UseSerilogRequestLogging(); // Enable Serilog request logging
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();