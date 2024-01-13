using Common.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Order.Persistence.Database;
using Order.Service.Proxies;
using Order.Service.Proxies.Catalog;
using Order.Service.Queries;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationHistory", "Order")
        )
);

builder.Services.AddHealthChecks()
         .AddCheck("self", () => HealthCheckResult.Healthy())
         .AddDbContextCheck<ApplicationDbContext>();


builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("Healthcheck Order API", "/health");
}).AddInMemoryStorage();

// Api Urls
builder.Services.Configure<ApiUrls>(
        opts => builder.Configuration.GetSection("ApiUrls").Bind(opts));

// Azure Service Bus ConnectionString
builder.Services.Configure<AzureServiceBus>(
    opts => builder.Configuration.GetSection("AzureServiceBus").Bind(opts)
);

// Proxies
builder.Services.AddHttpClient<ICatalogProxy, CatalogHttpProxy>();
//builder.Services.AddTransient<ICatalogProxy, CatalogQueueProxy>();


// Event handlers
builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(Assembly.Load("Order.Service.EventHandlers")));

// Query services
builder.Services.AddTransient<IOrderQueryService, OrderQueryService>();


builder.Services.AddControllers();

//Add authentication
var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretKey"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    ILoggerFactory loggerFactory = app.Services.GetService<ILoggerFactory>();
    loggerFactory.AddSyslog(
            builder.Configuration.GetValue<string>("Papertrail:host"),
            builder.Configuration.GetValue<int>("Papertrail:port"));
}

app.UseAuthorization();
app.UseAuthentication();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(opt => opt.UIPath = "/health-ui");

app.Run();
