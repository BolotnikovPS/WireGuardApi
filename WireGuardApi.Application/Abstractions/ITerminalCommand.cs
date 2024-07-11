namespace WireGuardApi.Application.Abstractions;

public interface ITerminalCommand
{
    /// <summary>
    /// Выполняет комманды в терминале
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> ExecuteAsync(string command, CancellationToken cancellationToken);
}