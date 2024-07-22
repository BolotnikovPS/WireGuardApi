using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Converters;
using TBotPlatform.Extension;
using WireGuardApi.Infrastructure;
using WireGuardApi.View.Middleware;

namespace WireGuardApi.View;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        // Add services to the container.

        services
           .AddCors(
                options =>
                {
                    options.AddDefaultPolicy(
                        policy =>
                        {
                            policy
                               .AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                        });
                })
           .AddControllers()
           .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services
           .AddEndpointsApiExplorer()
           .AddSwaggerGen()
           .AddInfrastructure();
        
        services
           .Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"))
           .AddLogging(
                loggingBuilder =>
                {
                    var logFilePath = builder.Configuration.GetValue<string>("LogFilePath");

                    if (logFilePath.IsNull())
                    {
                        throw new("Конфиг LogFilePath пустой.");
                    }

                    var logging = builder.Configuration.GetSection("Logging");

                    if (logging.IsNull())
                    {
                        throw new("Конфиг Logging пустой.");
                    }

                    loggingBuilder
                       .ClearProviders()
                       .AddConfiguration(logging)
                       .AddFile(
                            logFilePath,
                            outputTemplate: "{Timestamp:o} {RequestId,13} [{Level:u3}] ({SourceContext}) {Message} ({EventId:x8}){NewLine}{Exception}"
                            )
                       .AddConsole()
                       .SetMinimumLevel(LogLevel.Trace);
                });

        const string accessPortalTokenAuthenticationHandler = nameof(AccessPortalTokenAuthenticationHandler);

        services
           .AddAuthentication(accessPortalTokenAuthenticationHandler)
           .AddScheme<AuthenticationSchemeOptions, AccessPortalTokenAuthenticationHandler>(accessPortalTokenAuthenticationHandler, null);

        services.AddAuthorization();

        var app = builder.Build();

        app
           .UseSwagger()
           .UseSwaggerUI();
        app
           .UseHttpsRedirection()
           .UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}