using Common.Logging;
using Customer.Persistence.Database;
using Customer.Service.Queries;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsHistoryTable("__EFMigrationHistory", "Customer")
    )
);

builder.Services.AddHealthChecks()
         .AddCheck("self", () => HealthCheckResult.Healthy())
         .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("Healthcheck Customer API", "/health");
})
                .AddInMemoryStorage();




builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(Assembly.Load("Customer.Service.EventHandlers"))
);

builder.Services.AddTransient<IClientQueryService, ClientQueryService>();

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


