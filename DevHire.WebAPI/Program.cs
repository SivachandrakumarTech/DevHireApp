using DevHire.Application.ServiceContracts;
using DevHire.Application.Services;
using DevHire.Infrastructure.DBContext;
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
using Microsoft.AspNetCore.Identity;
using DevHire.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;


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

//Registering Jwt Service
builder.Services.AddTransient<IJwtService, JwtService>();


//Adding Developer DB Context for EF core
builder.Services.AddDbContext<DevelopersDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Adding Application DB Context for EF core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//Register AutoMapper   
builder.Services.AddAutoMapper(typeof(DeveloperProfile));

// Configure Identity with user and role types
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<UserStore<User, Role, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>()
    .AddDefaultTokenProviders();

// Add authentication services

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer from appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // Audience from appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)) // Secret key
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var errorMessage = "Unauthorized"; // Default

                if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                {
                    errorMessage = "TokenExpired"; // Custom error for expired token
                }

                var result = JsonSerializer.Serialize(new { error = errorMessage });
                return context.Response.WriteAsync(result);
            }
        };
    });


// Add authorication services

builder.Services.AddAuthorization();

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

app.UseHsts();  // ??
app.UseHttpsRedirection(); // ??

app.UseSerilogRequestLogging(); // Enable Serilog request logging
app.UseHttpLogging();

app.UseCors();
app.UseRouting();        //Identifying action Method based on route
app.UseAuthentication(); //// Enable authentication & authorization middleware
app.UseAuthorization();  //Validates access permissions of the user
app.MapControllers();   //Excute the filter pipeline (Action + filters)
app.Run();