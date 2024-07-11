using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.View.Contracts;

public class ReNewConfigRequest
{
    public required string WgInterfaceName { get; set; }

    public required WireGuardConfig Config { get; set; }
}