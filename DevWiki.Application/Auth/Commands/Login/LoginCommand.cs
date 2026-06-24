using MediatR;

namespace DevWiki.Application.Auth.Commands.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<string>;
}
