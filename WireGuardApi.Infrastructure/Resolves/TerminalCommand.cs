using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;
using WireGuardApi.Application.Abstractions;

namespace WireGuardApi.Infrastructure.Resolves;

internal class TerminalCommand(ILogger<TerminalCommand> logger) : ITerminalCommand
{
    public async Task<string> ExecuteAsync(string command, CancellationToken cancellationToken)
    {
        var log = new StringBuilder($"Выполняется комманда - {command}");
        var isError = false;
        try
        {
            using var process = new Process();
            process.StartInfo = new()
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };
            process.Start();

            log.AppendLine($"Ожидание выполнения комманды - {command}");

            await process.WaitForExitAsync(cancellationToken);

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);

            log.AppendLine($"ReadToEndAsync: Результат выполнения комманды {command}: {output}");

            return output;
        }
        catch (Exception ex)
        {
            isError = true;

            logger.LogInformation(ex, log.ToString());

            throw;
        }
        finally
        {
            if (!isError)
            {
                logger.LogInformation(log.ToString());
            }
        }
    }
}