using Microsoft.Extensions.DependencyInjection;
using WireGuardApi.Application.Abstractions;
using WireGuardApi.Application.Abstractions.Helpers;
using WireGuardApi.Application.Dependencies;
using WireGuardApi.Infrastructure.ConfigServices;
using WireGuardApi.Infrastructure.Helpers;
using WireGuardApi.Infrastructure.Resolves;

namespace WireGuardApi.Infrastructure;

public static partial class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
           .AddScoped<IConfigService, ConfigService>()
           .AddScoped<IConductorHelper, ConductorHelper>()
           .AddScoped<ITerminalCommand, TerminalCommand>()
           .AddScoped<IWireGuardConfig, WireGuardConfig>()
           .AddScoped<IWireGuardClient, WireGuardClient>()
           .AddApplication();

        return services;
    }
}