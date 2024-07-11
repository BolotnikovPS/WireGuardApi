namespace WireGuardApi.View.Contracts;

public record AddAutoPeerAsyncRequest
{
    public required string WgInterfaceName { get; set; }
    public string Comment { get; set; }
}