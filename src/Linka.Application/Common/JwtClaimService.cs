using Linka.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System;

namespace Linka.Application.Common;
public class JwtClaimService : IJwtClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtClaimService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
    }

    public string GetUserId()
    {
        return GetClaimValue(ClaimTypes.NameIdentifier);
    }

    public string GetUserType()
    {
        return GetClaimValue("type");
    }
}
