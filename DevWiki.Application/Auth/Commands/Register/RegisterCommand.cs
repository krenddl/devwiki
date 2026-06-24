using MediatR;
using System;

namespace DevWiki.Application.Auth.Commands.Register
{
    public record RegisterCommand(string Username, string Password) : IRequest<Guid>;
}
