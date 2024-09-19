using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;

namespace WireGuardApi.Application.CQ.Commands;

public record AddPeerCommand(string WgInterfaceName, string ClientPeer, string ClientPrivateIp, string Comment) : ICommand;

internal class AddPeerCommandHandler(IWireGuardСontrol wireGuardСontrol)
    : ICommandHandler<AddPeerCommand>
{
    public Task Handle(AddPeerCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.AddPeerAsync(request.WgInterfaceName, request.ClientPeer, request.ClientPrivateIp, request.Comment, cancellationToken);
}