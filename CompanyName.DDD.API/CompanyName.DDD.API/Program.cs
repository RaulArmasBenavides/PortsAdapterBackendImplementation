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
Console.WriteLine($"Authority: {builder.Configuration.GetSection("Auth").GetSection("Authority").Value} ");
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
app.MapControllers().RequireAuthorization();
app.UseCors("local");
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
Console.WriteLine("Server started");
app.Run();