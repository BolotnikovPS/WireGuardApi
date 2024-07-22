using WireGuardApi.Domain.Contracts;
using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.Domain.Abstractions;

public interface IWireGuardСontrol
{
    Task<WireGuardConfig> GetConfigAsync(string wgInterfaceName, CancellationToken cancellationToken);

    Task ReNewConfigAsync(string wgInterfaceName, WireGuardConfig config, CancellationToken cancellationToken);

    Task<List<AddAutoPeerResponse>> AddAutoPeerAsync(string wgInterfaceName, string comment, CancellationToken cancellationToken);

    Task AddPeerAsync(string wgInterfaceName, string clientPeer, string clientPrivateIp, string comment, CancellationToken cancellationToken);
    
    Task RemovePeerAsync(string wgInterfaceName, string clientPeer, CancellationToken cancellationToken);
    
    Task<string> GetStatisticsAsync(string wgInterfaceName, CancellationToken cancellationToken);

    Task UpAsync(string wgInterfaceName, CancellationToken cancellationToken);
    Task DownAsync(string wgInterfaceName, CancellationToken cancellationToken);
}