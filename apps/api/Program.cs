using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pinoles.Api.Application.Analytics;
using Pinoles.Api.Application.Auth;
using Pinoles.Api.Application.CreditDocuments;
using Pinoles.Api.Application.Customers;
using Pinoles.Api.Application.Dashboard;
using Pinoles.Api.Application.DTOs;
using Pinoles.Api.Application.Interfaces;
using Pinoles.Api.Application.Inventory;
using Pinoles.Api.Application.Items;
using Pinoles.Api.Application.Mapping;
using Pinoles.Api.Application.Notifications;
using Pinoles.Api.Application.Purchase;
using Pinoles.Api.Application.Sales;
using Pinoles.Api.Application.Search;
using Pinoles.Api.Application.Vendors;
using Pinoles.Api.Domain.Constants;
using Pinoles.Api.Infrastructure.Auth;
using Pinoles.Api.Infrastructure.BusinessCentral;
using Pinoles.Api.Infrastructure.Caching;
using Pinoles.Api.Infrastructure.Email;
using Pinoles.Api.Infrastructure.Persistence;
using Pinoles.Api.Presentation.Endpoints;
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

    // JWT options
    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
    var jwtConfig = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

    // Auth services
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddSingleton<LoginRateLimiter>();

    // JWT Bearer authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtConfig.Secret ?? "placeholder-key-not-for-production")),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };
            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(
                        """{"success":false,"error":"Unauthorized","code":"AUTH_REQUIRED"}""");
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = 403;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(
                        """{"success":false,"error":"Forbidden","code":"FORBIDDEN_INSUFFICIENT_ROLE"}""");
                },
            };
        });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdmin", policy =>
            policy.RequireRole(UserRoles.Admin));
        options.AddPolicy("RequireFinancial", policy =>
            policy.RequireRole(UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting));
        options.AddPolicy("RequireDashboard", policy =>
            policy.RequireRole(UserRoles.Admin, UserRoles.Manager, UserRoles.Accounting, UserRoles.Warehouse));
        options.AddPolicy("RequireWarehouse", policy =>
            policy.RequireRole(UserRoles.Admin, UserRoles.Manager, UserRoles.Warehouse));
    });

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

    // Dashboard service
    builder.Services.AddScoped<IDashboardService, DashboardService>();

    // BC mappers
    builder.Services.AddSingleton<IBcMapper<BcCustomer, CustomerListItemDto>, CustomerMapper>();
    builder.Services.AddSingleton<IBcMapper<BcVendor, VendorListItemDto>, VendorMapper>();
    builder.Services.AddSingleton<IBcMapper<BcSalesInvoice, SalesInvoiceListItemDto>, SalesInvoiceMapper>();
    builder.Services.AddSingleton<IBcMapper<BcSalesInvoice, SalesInvoiceDetailDto>, SalesInvoiceDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcSalesInvoice, SalesAdvanceInvoiceDetailDto>, SalesAdvanceInvoiceDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcPurchaseInvoice, PurchaseInvoiceListItemDto>, PurchaseInvoiceMapper>();
    builder.Services.AddSingleton<IBcMapper<BcPurchaseInvoice, PurchaseInvoiceDetailDto>, PurchaseInvoiceDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcPurchaseInvoice, PurchaseAdvanceInvoiceDetailDto>, PurchaseAdvanceInvoiceDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcCreditDocument, CreditDocumentListItemDto>, CreditDocumentMapper>();
    builder.Services.AddSingleton<IBcMapper<BcCreditDocument, CreditDocumentDetailDto>, CreditDocumentDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcItem, ItemListItemDto>, ItemMapper>();
    builder.Services.AddSingleton<IBcMapper<BcItem, ItemProfileDto>, ItemDetailMapper>();
    builder.Services.AddSingleton<IBcMapper<BcStockByLocation, StockByLocationDto>, StockByLocationMapper>();
    builder.Services.AddSingleton<IBcMapper<BcItemLedgerEntry, ItemLedgerEntryDto>, ItemLedgerEntryMapper>();

    // Customer service
    builder.Services.AddScoped<ICustomerService, CustomerService>();

    // Sales service
    builder.Services.AddScoped<ISalesService, SalesService>();

    // Purchase service
    builder.Services.AddScoped<IPurchaseService, PurchaseService>();

    // Vendor service
    builder.Services.AddScoped<IVendorService, VendorService>();

    // Credit-document service (US-016) — unified credit memos / debit memos / storno
    builder.Services.AddScoped<ICreditDocumentService, CreditDocumentService>();

    // Item service (US-017) — first WAREHOUSE-module list service
    builder.Services.AddScoped<IItemService, ItemService>();

    // Inventory service (US-019) — warehouse stock overview (summary KPIs, by location, low stock)
    builder.Services.AddScoped<IInventoryService, InventoryService>();

    // Analytics service (US-020) — revenue/expense trends, top customers/items, period comparison
    builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

    // Search service — aggregates the four list services above (RBAC-gated)
    builder.Services.AddScoped<ISearchService, SearchService>();

    // Notification service (T-005) — aggregates overdue invoices + low-stock items, role-filtered
    builder.Services.AddScoped<INotificationService, NotificationService>();

    // Email service (T-005) — dev no-op logging impl; production wires a real MailKit SMTP
    // implementation via config. US-021 (password reset) depends on IEmailService.
    builder.Services.AddScoped<IEmailService, LoggingEmailService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PinolesDbContext>();
        await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db, app.Logger);
    }

    app.UseSerilogRequestLogging();
    app.UseCors();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.MapAuthEndpoints();
    app.MapUsersEndpoints();
    app.MapDashboardEndpoints();
    app.MapCustomersEndpoints();
    app.MapSalesEndpoints();
    app.MapPurchaseEndpoints();
    app.MapVendorsEndpoints();
    app.MapCreditDocumentsEndpoints();
    app.MapItemsEndpoints();
    app.MapInventoryEndpoints();
    app.MapAnalyticsEndpoints();
    app.MapSearchEndpoints();
    app.MapNotificationsEndpoints();

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
