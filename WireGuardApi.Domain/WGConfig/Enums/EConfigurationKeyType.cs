namespace WireGuardApi.Domain.WGConfig.Enums;

public enum EConfigurationKeyType
{
    None = 0,
    Address,
    SaveConfig,
    PostUp,
    PostDown,
    ListenPort,
    PrivateKey,
    PublicKey,
    AllowedIPs,
}