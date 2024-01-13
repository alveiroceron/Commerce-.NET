using Common.Logging;
using HealthChecks.UI.Client;
using Identity.Domain;
using Identity.Persistence.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.MigrationsHistoryTable("__EFMigrationHistory", "Identity")
        )
);

builder.Services.AddHealthChecks()
         .AddCheck("self", () => HealthCheckResult.Healthy())
         .AddDbContextCheck<ApplicationDbContext>();


builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("Healthcheck Identity API", "/health");
})
.AddInMemoryStorage();


//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

// Identity Configuration

builder.Services.Configure<IdentityOptions>(options =>
{ 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Event Handlers
builder.Services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(Assembly.Load("Identity.Service.EventHandlers"))
        );


// API controllers
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