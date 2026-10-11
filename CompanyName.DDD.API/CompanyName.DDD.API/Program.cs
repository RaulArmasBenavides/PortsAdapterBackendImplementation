using CompanyName.DDD.API.Extensions;
using CompanyName.DDD.API.Middleware;
using CompanyName.DDD.Domain.Common.EFCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings/appsettings.json", optional: false, reloadOnChange: true);
// Environment variables must win over the JSON file, so they are added again after it.
builder.Configuration.AddEnvironmentVariables();

// Secretos fuera del repositorio: ConnectionStrings__Operations (variable de entorno o User Secrets).
if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("Operations")))
{
    throw new InvalidOperationException("ConnectionStrings:Operations is not configured. Set it with user secrets or the variable ConnectionStrings__Operations.");
}
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
{
    o.Authority = builder.Configuration.GetSection("Auth").GetSection("Authority").Value;
    o.Audience = builder.Configuration.GetSection("Auth").GetSection("Audience").Value;
    o.RequireHttpsMetadata = false;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "local", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Operations"),
        sqlServerOptionsAction: builder =>
        {
            builder.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: null);
        });
});
DIExtension.AddConfigurations(builder.Services);
OptionsExtension.AddConfiguration(builder.Services, builder.Configuration);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (environment.Equals("Local"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("local");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.Run();