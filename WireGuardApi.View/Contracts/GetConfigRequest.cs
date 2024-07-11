namespace WireGuardApi.View.Contracts;

public record GetConfigRequest
{
    public required string WgInterfaceName { get; set; }
}