using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.Application.Abstractions;

public interface IWireGuardConfig
{
    /// <summary>
    /// Читает конфиг ВПН
    /// </summary>
    /// <param name="wgConfigPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Root> ReadConfigAsync(string wgConfigPath, CancellationToken cancellationToken);

    /// <summary>
    /// Сохраняет конфиг ВПН
    /// </summary>
    /// <param name="wgConfigPath"></param>
    /// <param name="root"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveConfigAsync(string wgConfigPath, Root root, CancellationToken cancellationToken);
}