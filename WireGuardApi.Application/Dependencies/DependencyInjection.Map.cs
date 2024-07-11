using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WireGuardApi.Application.Templates;
using WireGuardApi.Domain.Abstractions;

namespace WireGuardApi.Application.Dependencies;

public static partial class DependencyInjection
{
    public static IServiceCollection AddMap(this IServiceCollection services, Assembly executingAssembly)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(executingAssembly);

        services
           .AddSingleton<IMapper>(new Mapper(typeAdapterConfig))
           .AddScoped<IMap, Map>();

        return services;
    }
}