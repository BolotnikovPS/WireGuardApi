using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Query;
using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.Application.CQ.Queries;

public record ConfigQuery(string WgInterfaceName) : IQuery<WireGuardConfig>;

internal class ConfigQueryHandler(IWireGuardСontrol wireGuardСontrol)
    : IQueryHandler<ConfigQuery, WireGuardConfig>
{
    public Task<WireGuardConfig> Handle(ConfigQuery request, CancellationToken cancellationToken)
        => wireGuardСontrol.GetConfigAsync(request.WgInterfaceName, cancellationToken);
}