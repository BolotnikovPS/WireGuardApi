namespace WireGuardApi.Domain.WGConfig;

public class WireGuardConfig
{
    public WireGuardConfigInterface Interface { get; set; }
    public List<WireGuardConfigPeer> Peers { get; set; }
}