namespace WireGuardApi.Domain.WGConfig;

public class WireGuardConfigInterface
{
    public string Address { get; set; }
    public string PostUp { get; set; }
    public string PostDown { get; set; }
    public string ListenPort { get; set; }
    public string PrivateKey { get; set; }
    public string SaveConfig { get; set; }
}