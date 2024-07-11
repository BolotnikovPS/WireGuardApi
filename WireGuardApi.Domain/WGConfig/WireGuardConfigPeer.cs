namespace WireGuardApi.Domain.WGConfig;

public class WireGuardConfigPeer
{
    public string Comment { get; set; }

    public List<ConfigurationValues> ConfigurationValues { get; set; } = [];
}