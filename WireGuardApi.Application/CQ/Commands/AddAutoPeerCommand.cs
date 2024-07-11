using WireGuardApi.Domain.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Command;
using WireGuardApi.Domain.Contracts;

namespace WireGuardApi.Application.CQ.Commands;

public record AddAutoPeerCommand(string WgInterfaceName, string Comment) : ICommand<List<AddAutoPeerResponse>>;

internal class AddAutoPeerCommandHandler(
    IWireGuardСontrol wireGuardСontrol
    )
    : ICommandHandler<AddAutoPeerCommand, List<AddAutoPeerResponse>>
{
    public Task<List<AddAutoPeerResponse>> Handle(AddAutoPeerCommand request, CancellationToken cancellationToken)
        => wireGuardСontrol.AddAutoPeerAsync(request.WgInterfaceName, request.Comment, cancellationToken);
}