using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireGuardApi.Application.CQ.Commands;
using WireGuardApi.Application.CQ.Queries;
using WireGuardApi.Domain.Abstractions.CQRS;
using WireGuardApi.Domain.Contracts;
using WireGuardApi.View.Contracts;

namespace WireGuardApi.View.Controllers;

[ApiController]
[Route("[controller]/{wgInterfaceName}")]
[Authorize]
public class WireGuardCommandController(
    ILogger<WireGuardCommandController> logger,
    ISenderRun senderRun
    ) : Controller
{
    [HttpGet]
    [Route("statistics")]
    public Task<string> GetStatisticsAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var query = new StatisticsQuery(wgInterfaceName);

        return senderRun.SendAsync(query, cancellationToken);
    }

    [HttpPost]
    [Route("addPeer")]
    public Task AddPeerAsync(string wgInterfaceName, AddPeerAsyncRequest request, CancellationToken cancellationToken)
    {
        var command = new AddPeerCommand(wgInterfaceName, request.ClientPeer, request.ClientPrivateIp, request.Comment);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("addAutoPeer")]
    public Task<List<AddAutoPeerResponse>> AddAutoPeerAsync(string wgInterfaceName, AddAutoPeerAsyncRequest request, CancellationToken cancellationToken)
    {
        var command = new AddAutoPeerCommand(wgInterfaceName, request.Comment);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("up")]
    public Task UpInterfaceAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = new UpInterfaceCommand(wgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("down")]
    public Task DownInterfaceAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = new DownInterfaceCommand(wgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("reboot")]
    public Task RebootInterfaceAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var command = new RebootInterfaceCommand(wgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }
}