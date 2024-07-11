using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;

namespace WireGuardApi.Application.CQ.Commands;

public record RebootInterfaceCommand(string WgInterfaceName) : ICommand;

internal class RebootInterfaceCommandHandler(
    IWireGuardСontrol wireGuardСontrol
    )
    : ICommandHandler<RebootInterfaceCommand>
{
    public async Task Handle(RebootInterfaceCommand request, CancellationToken cancellationToken)
    {
        await wireGuardСontrol.DownAsync(request.WgInterfaceName, cancellationToken);
        await wireGuardСontrol.UpAsync(request.WgInterfaceName, cancellationToken);
    }
}