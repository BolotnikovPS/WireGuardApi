using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WireGuardApi.Application.CQ.Commands;
using WireGuardApi.Application.CQ.Queries;
using WireGuardApi.Domain.Abstractions.CQRS;
using WireGuardApi.Domain.WGConfig;
using WireGuardApi.View.Contracts;

namespace WireGuardApi.View.Controllers;

[ApiController]
[Route("[controller]/{wgInterfaceName}")]
[Authorize]
public class WireGuardConfigController(
    ILogger<WireGuardConfigController> logger,
    ISenderRun senderRun
    ) : Controller
{
    [HttpGet]
    public Task<WireGuardConfig> GetConfigAsync(string wgInterfaceName, CancellationToken cancellationToken)
    {
        var query = new ConfigQuery(wgInterfaceName);

        return senderRun.SendAsync(query, cancellationToken);
    }

    [HttpPost]
    public Task ReNewConfigAsync(string wgInterfaceName, ReNewConfigRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfigCommand(wgInterfaceName, request.Config);

        return senderRun.SendAsync(command, cancellationToken);
    }
}