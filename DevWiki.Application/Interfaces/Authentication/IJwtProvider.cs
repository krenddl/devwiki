using DevWiki.Domain.Entities;

namespace DevWiki.Application.Interfaces.Authentication
{
    public interface IJwtProvider
    {
        string Generate(User user);
    }
}
