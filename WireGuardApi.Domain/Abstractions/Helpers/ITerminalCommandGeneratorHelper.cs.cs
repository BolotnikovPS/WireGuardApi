using WireGuardApi.Domain.Enums;

namespace WireGuardApi.Domain.Abstractions.Helpers;

public interface ITerminalCommandGeneratorHelper
{
    public string CreateCommand(
        ETerminalCommandType type,
        params string[] parameters
        );
}