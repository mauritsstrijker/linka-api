namespace Linka.Application.Common
{
    public interface IJwtClaimService
    {
        string GetClaimValue(string claimType);
        string GetUserId();
        string GetUserType();
    }
}
