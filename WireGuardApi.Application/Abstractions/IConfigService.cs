using WireGuardApi.Domain.Enums;

namespace WireGuardApi.Application.Abstractions;

public interface IConfigService
{
    T GetValueOrNull<T>(EConfigKey key);

    string GetValueOrNull(EConfigKey key);
}