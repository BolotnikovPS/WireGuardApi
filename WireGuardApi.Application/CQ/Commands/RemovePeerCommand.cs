using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;

namespace WireGuardApi.Application.CQ.Commands;

public record RemovePeerCommand(string WgInterfaceName, string ClientPeer) : ICommand;

internal class RemovePeerCommandHandler(
    IWireGuardСontrol wireGuardСontrol
    )
    : ICommandHandler<RemovePeerCommand>
{
    public Task Handle(RemovePeerCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.RemovePeerAsync(request.WgInterfaceName, request.ClientPeer, cancellationToken);
}