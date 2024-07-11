namespace WireGuardApi.Domain.Enums;

public enum ETerminalCommandType
{
    Up,
    Down,
    AddPeer,
    RemovePeer,
    CopyConfig,
    Statistics,
    GenKey,
    GeneratePublicKey,
    GenQRCode,
    GetServerPublicKey,
}