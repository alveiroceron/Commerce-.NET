using Api.Gateway.WebClient.Config;
using Common.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAppsettingBinding(builder.Configuration)
                .AddProxiesRegistration(builder.Configuration);

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


app.UseAuthorization();
app.UseAuthentication();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();

app.Run();