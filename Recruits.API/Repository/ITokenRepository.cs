using Recruits.API.Model;

namespace Recruits.API.Repository
{
    public interface ITokenRepository
    {
        Tokens Authenticate(Users users);
    }
}
