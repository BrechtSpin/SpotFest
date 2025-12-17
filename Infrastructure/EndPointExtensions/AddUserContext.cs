using Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace Infrastructure.EndPointExtensions;

public static partial class EndpointExtensions
{
    public static RouteHandlerBuilder AddUserContext(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<AddUserContextEndpointFilter>();
    }

    private class AddUserContextEndpointFilter : IEndpointFilter
    {
        public ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var userGuid = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userGuid))
            {
                Activity.Current?.SetBaggage(BaggageKeys.UserGuid, userGuid);
            }
            return next(context);
        }
    }
}

