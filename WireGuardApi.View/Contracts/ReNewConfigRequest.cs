using WireGuardApi.Domain.WGConfig;

namespace WireGuardApi.View.Contracts;

public class ReNewConfigRequest
{
    public required WireGuardConfig Config { get; set; }
}