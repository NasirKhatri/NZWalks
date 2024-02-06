using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenInterface
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
