using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;

namespace WireGuardApi.Application.CQ.Commands;

public record DownInterfaceCommand(string WgInterfaceName) : ICommand;

internal class DownInterfaceCommandHandler(
    IWireGuardСontrol wireGuardСontrol
    )
    : ICommandHandler<DownInterfaceCommand>
{
    public Task Handle(DownInterfaceCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.DownAsync(request.WgInterfaceName, cancellationToken);
}