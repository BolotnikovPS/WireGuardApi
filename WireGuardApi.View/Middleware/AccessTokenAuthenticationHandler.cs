using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WireGuardApi.Application.CQ.Queries;
using WireGuardApi.Domain.Abstractions.CQRS;

namespace WireGuardApi.View.Middleware;

public class AccessPortalTokenAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISenderRun senderRun
    ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private AuthenticateResult Result { get; set; }

    public const string TokenHeaderName = "X-TOKEN";

    protected async Task<AuthenticateResult> HandleAuthenticateInternalAsync()
    {
        var claims = new List<Claim>();
        var xToken = GetFromHeader(Request.Headers, TokenHeaderName, throwIfNotFount: true);

        var cancellationToken = Context.Request.HttpContext.RequestAborted;

        var query = new AuthenticationTokenQuery(xToken);
        var isAuth = await senderRun.SendAsync(query, cancellationToken);

        if (!isAuth)
        {
            throw new AuthenticationException("Вы не авторизованы.");
        }

        claims.Add(new(TokenHeaderName, xToken));

        var ticket = BuildAuthenticationTicket(claims);
        return AuthenticateResult.Success(ticket);
    }


    protected sealed override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            Result = await HandleAuthenticateInternalAsync();
        }
        catch (Exception ex)
        {
            Result = AuthenticateResult.Fail(ex);
        }

        return Result;
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        if (Result?.Failure != null)
        {
            throw Result.Failure;
        }

        return base.HandleChallengeAsync(properties);
    }

    protected AuthenticationTicket BuildAuthenticationTicket(List<Claim> claims)
    {
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new System.Security.Principal.GenericPrincipal(identity, roles: null);
        return new(principal, Scheme.Name);
    }

    protected static string GetFromHeader(IHeaderDictionary headers, string headerName, bool throwIfNotFount)
    {
        var found = headers.TryGetValue(headerName, out var header);

        if (found)
        {
            return header;
        }

        if (throwIfNotFount)
        {
            throw new AuthenticationException($"Запрос должен содержать заголовок {headerName}");
        }

        return null;
    }
}