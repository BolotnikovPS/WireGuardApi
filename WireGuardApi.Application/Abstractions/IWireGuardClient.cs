namespace WireGuardApi.Application.Abstractions;

public interface IWireGuardClient
{
    /// <summary>
    /// Добавляет клиента к ВПН
    /// </summary>
    /// <param name="clientPrivateKey"></param>
    /// <param name="clientPrivateIp"></param>
    /// <param name="serverPublicKey"></param>
    /// <param name="filePath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task CreateClientAsync(string clientPrivateKey, string clientPrivateIp, string serverPublicKey, string filePath, CancellationToken cancellationToken);
}