using DevWiki.Application.Interfaces;
using DevWiki.Application.Interfaces.Authentication;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace DevWiki.Application.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
            {
                throw new ValidationException("Invalid username or password.");
            }

            var token = _jwtProvider.Generate(user);
            return Task.FromResult(token);
        }
    }
}
