using System.Collections.Frozen;
using TBotPlatform.Extension;
using WireGuardApi.Domain.Abstractions.Helpers;
using WireGuardApi.Domain.Enums;

namespace WireGuardApi.Application.Helpers;

internal class TerminalCommandGeneratorHelper : ITerminalCommandGeneratorHelper
{
    private FrozenDictionary<ETerminalCommandType, string> DataCollection { get; } = new Dictionary<ETerminalCommandType, string>
    {
        [ETerminalCommandType.Up] = "wg-quick up {0}",
        [ETerminalCommandType.Down] = "wg-quick down {0}",
        [ETerminalCommandType.AddPeer] = "wg set {0} peer {1} allowed-ips {2}",
        [ETerminalCommandType.RemovePeer] = "wg set {0} peer {1} remove",
        [ETerminalCommandType.CopyConfig] = "cp {0} {1}",
        [ETerminalCommandType.Statistics] = "wg show",
        [ETerminalCommandType.GenKey] = "wg genkey",
        [ETerminalCommandType.GeneratePublicKey] = "echo {0} | wg pubkey",
        [ETerminalCommandType.GenQRCode] = "qrencode -t png -o {0} -r {1}",
        [ETerminalCommandType.GetServerPublicKey] = "wg show {0} | grep -e \"public key\" | cut -d: -f2",
    }.ToFrozenDictionary();

    public string CreateCommand(
        ETerminalCommandType type,
        params string[] parameters
        )
    {
        var command = DataCollection.GetValueRefOrNullRef(type);

        return parameters.IsNull()
            ? command
            : string.Format(command, parameters);
    }
}