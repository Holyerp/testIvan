using Microsoft.EntityFrameworkCore;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Pinoles.Api.Infrastructure.Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .WriteTo.Console());

    // Services
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "Pinoles API", Version = "v1" });
    });

    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:3000"];
        options.AddDefaultPolicy(policy =>
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials());
    });

    builder.Services.AddMemoryCache();

    // Database
    builder.Services.AddDbContext<PinolesDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // BC options
    builder.Services.Configure<BcOptions>(builder.Configuration.GetSection(BcOptions.SectionName));

    // Cache
    builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

    // BC services — mock or real based on config
    var bcOptions = builder.Configuration.GetSection(BcOptions.SectionName).Get<BcOptions>() ?? new BcOptions();
    if (bcOptions.UseMock)
    {
        builder.Services.AddSingleton<IBcHttpClient, MockBcHttpClient>();
        // No BcAuthService needed in mock mode
    }
    else
    {
        builder.Services.AddSingleton<IBcAuthService, BcAuthService>();
        builder.Services.AddHttpClient<IBcHttpClient, BcHttpClient>();
    }

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseCors();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Health check
    app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
       .AllowAnonymous();

    Log.Information("Pinoles API starting on {Env}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
