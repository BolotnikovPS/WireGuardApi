using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;

namespace WireGuardApi.Application.CQ.Commands;

public record UpInterfaceCommand(string WgInterfaceName) : ICommand;

internal class UpInterfaceCommandHandler(IWireGuardСontrol wireGuardСontrol)
    : ICommandHandler<UpInterfaceCommand>
{
    public Task Handle(UpInterfaceCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.UpAsync(request.WgInterfaceName, cancellationToken);
}