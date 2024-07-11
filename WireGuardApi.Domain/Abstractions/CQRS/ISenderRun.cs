using WireGuardApi.Domain.Abstractions.CQRS.Command;
using WireGuardApi.Domain.Abstractions.CQRS.Query;

namespace WireGuardApi.Domain.Abstractions.CQRS;

/// <summary>
/// Скрытие интерфейса IMediator
/// </summary>
public interface ISenderRun
{
    /// <summary>
    /// Скрытый вызов Mediatr
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken);

    /// <summary>
    /// Скрытый вызов Mediatr
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken);

    /// <summary>
    /// Скрытый вызов Mediatr
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendAsync<TRequest>(TRequest request, CancellationToken cancellationToken) where TRequest : ICommand;
}