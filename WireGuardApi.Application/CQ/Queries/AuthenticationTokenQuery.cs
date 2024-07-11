using TBotPlatform.Extension;
using WireGuardApi.Application.Abstractions;
using WireGuardApi.Domain.Abstractions.CQRS.Query;
using WireGuardApi.Domain.Enums;

namespace WireGuardApi.Application.CQ.Queries;

public record AuthenticationTokenQuery(string Token) : IQuery<bool>;

internal class AuthenticationTokenQueryHandler(
    IConfigService configService
    )
    : IQueryHandler<AuthenticationTokenQuery, bool>
{
    public Task<bool> Handle(AuthenticationTokenQuery request, CancellationToken cancellationToken)
    {
        var tokens = configService.GetValueOrNull(EConfigKey.Tokens);

        return Task.FromResult(
            tokens.CheckAny() 
            && tokens.Contains(request.Token)
            );
    }
}