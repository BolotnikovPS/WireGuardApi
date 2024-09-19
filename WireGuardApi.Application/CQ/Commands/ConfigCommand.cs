using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;
using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.Application.CQ.Commands;

public record ConfigCommand(string WgInterfaceName, WireGuardConfig Config) : ICommand;

internal class ConfigCommandHandler(IWireGuardСontrol wireGuardСontrol)
    : ICommandHandler<ConfigCommand>
{
    public Task Handle(ConfigCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.ReNewConfigAsync(request.WgInterfaceName, request.Config, cancellationToken);
}