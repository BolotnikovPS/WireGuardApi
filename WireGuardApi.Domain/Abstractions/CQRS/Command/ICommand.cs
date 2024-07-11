using MediatR;

namespace WireGuardApi.Domain.Abstractions.CQRS.Command;

public interface ICommand
    : IRequest;

public interface ICommand<out TResponse>
    : IRequest<TResponse>;