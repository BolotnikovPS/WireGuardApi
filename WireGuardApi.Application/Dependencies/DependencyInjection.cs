using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WireGuardApi.Application.Helpers;
using WireGuardApi.Application.Templates;
using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS;
using WireGuardApi.Domain.Abstractions.Helpers;

namespace WireGuardApi.Application.Dependencies;

public static partial class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services
           .AddMap(executingAssembly)
           .AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssemblies(executingAssembly);
                    cfg.NotificationPublisher = new TaskWhenAllPublisher();
                })
           .AddScoped<ISenderRun, SenderRun>()
           .AddSingleton<ITerminalCommandGeneratorHelper, TerminalCommandGeneratorHelper>()
           .AddScoped<IWireGuardСontrol, WireGuardСontrol>();

        return services;
    }
}