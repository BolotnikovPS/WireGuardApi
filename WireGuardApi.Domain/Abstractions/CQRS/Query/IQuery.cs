using MediatR;

namespace WireGuardApi.Domain.Abstractions.CQRS.Query;

public interface IQuery<out TResponse>
    : IRequest<TResponse>;