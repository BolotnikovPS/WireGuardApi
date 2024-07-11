using Microsoft.Extensions.Logging;
using TBotPlatform.Extension;
using WireGuardApi.Application.Abstractions;

namespace WireGuardApi.Infrastructure.Resolves;

internal class WireGuardClient(ILogger<WireGuardClient> logger) : IWireGuardClient
{
    public async Task CreateClientAsync(
        string clientPrivateKey,
        string clientPrivateIp,
        string serverPublicKey,
        string filePath,
        CancellationToken cancellationToken
        )
    {
        var filePathRead = $"{Directory.GetCurrentDirectory()}/Files/WGMobileConfExample.conf";
        logger.LogInformation("Чтение файла по путь {filePathRead}", filePathRead);

        var fileText = await File.ReadAllTextAsync(filePathRead, cancellationToken);

        logger.LogInformation("Замена значения YOUR_CLIENT_PRIVATE_KEY на {clientPrivateKey}", clientPrivateKey);
        logger.LogInformation("Замена значения YOUR_CLIENT_PRIVATE_IP на {clientPrivateIp}", clientPrivateIp);
        logger.LogInformation("Замена значения YOUR_SERVER_PUBLIC_KEY на {serverPublicKey}", serverPublicKey);

        fileText = fileText
                  .Replace("YOUR_CLIENT_PRIVATE_KEY", clientPrivateKey)
                  .Replace("YOUR_CLIENT_PRIVATE_IP", clientPrivateIp)
                  .Replace("YOUR_SERVER_PUBLIC_KEY", serverPublicKey);

        logger.LogInformation("Конфиг для нового клиента: {fileText}. Пишем по пути {filePath}", fileText.ToJson(), filePath);

        await File.WriteAllTextAsync(filePath, fileText, cancellationToken);
    }
}