using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WireGuardApi.Application.Abstractions;
using WireGuardApi.Domain.Enums;

namespace WireGuardApi.Infrastructure.ConfigServices;

internal class ConfigService(
    ILogger<ConfigService> logger,
    IConfiguration configuration
    ) : IConfigService
{
    public T GetValueOrNull<T>(EConfigKey key)
    {
        return configuration
              .GetSection(key.ToString())
              .Get<T>(c => c.BindNonPublicProperties = true);
    }

    public string GetValueOrNull(EConfigKey key)
    {
        return configuration[key.ToString()];
    }
}