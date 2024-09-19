using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Query;

namespace WireGuardApi.Application.CQ.Queries;

public record StatisticsQuery(string WgInterfaceName) : IQuery<string>;

internal class StatisticsQueryHandler(IWireGuardСontrol wireGuardСontrol)
    : IQueryHandler<StatisticsQuery, string>
{
    public Task<string> Handle(StatisticsQuery request, CancellationToken cancellationToken)
        => wireGuardСontrol.GetStatisticsAsync(request.WgInterfaceName, cancellationToken);
}