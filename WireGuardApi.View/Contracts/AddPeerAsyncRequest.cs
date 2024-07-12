namespace WireGuardApi.View.Contracts;

public record AddPeerAsyncRequest
{
    public required string ClientPeer { get; set; }
    public required string ClientPrivateIp { get; set; }
    public string Comment { get; set; }
}