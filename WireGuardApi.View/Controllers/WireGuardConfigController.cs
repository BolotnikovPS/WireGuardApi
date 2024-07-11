using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireGuardApi.Application.CQ.Commands;
using WireGuardApi.Application.CQ.Queries;
using WireGuardApi.Domain.Abstractions.CQRS;
using WireGuardApi.Domain.Contracts;
using WireGuardApi.Domain.WGConfig;
using WireGuardApi.View.Contracts;

namespace WireGuardApi.View.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class WireGuardConfigController(
    ILogger<WireGuardConfigController> logger,
    ISenderRun senderRun
    ) : Controller
{
    [HttpGet]
    public Task<WireGuardConfig> GetConfigAsync(GetConfigRequest request, CancellationToken cancellationToken)
    {
        var query = new ConfigQuery(request.WgInterfaceName);

        return senderRun.SendAsync(query, cancellationToken);
    }
        
    [HttpPost]
    public Task ReNewConfigAsync(ReNewConfigRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfigCommand(request.WgInterfaceName, request.Config);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpGet]
    [Route("statistics")]
    public Task<string> GetStatisticsAsync(GetStatisticsRequest request, CancellationToken cancellationToken)
    {
        var query = new StatisticsQuery(request.WgInterfaceName);

        return senderRun.SendAsync(query, cancellationToken);
    }

    [HttpPost]
    [Route("addPeer")]
    public Task AddPeerAsync(AddPeerAsyncRequest request, CancellationToken cancellationToken)
    {
        var command = new AddPeerCommand(request.WgInterfaceName, request.ClientPeer, request.ClientPrivateIp, request.Comment);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("addAutoPeer")]
    public Task<List<AddAutoPeerResponse>> AddAutoPeerAsync(AddAutoPeerAsyncRequest request, CancellationToken cancellationToken)
    {
        var command = new AddAutoPeerCommand(request.WgInterfaceName, request.Comment);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("up")]
    public Task UpInterfaceAsync(UpDownRequest request, CancellationToken cancellationToken)
    {
        var command = new UpInterfaceCommand(request.WgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("down")]
    public Task DownInterfaceAsync(UpDownRequest request, CancellationToken cancellationToken)
    {
        var command = new DownInterfaceCommand(request.WgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }

    [HttpPost]
    [Route("reboot")]
    public Task RebootInterfaceAsync(UpDownRequest request, CancellationToken cancellationToken)
    {
        var command = new RebootInterfaceCommand(request.WgInterfaceName);

        return senderRun.SendAsync(command, cancellationToken);
    }
}