using FluentValidation;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using Linka.Domain.Helpers;
using MediatR;

namespace Linka.Application.Features.Users.Commands
{
    public sealed record AuthenticateRequest(string Username, string Password) : IRequest<AuthenticateResponse>;

    public sealed record AuthenticateResponse(string Token);

    public sealed class AuthenticateHandler
        (
        IUserRepository userRepository,
        JwtBuilder jwtBuilder
        ) : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
    {
        private string AuthenticationFailedMessage = "Verifique seus dados.";
        public async Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUsername(request.Username, cancellationToken) ?? throw new Exception(AuthenticationFailedMessage);

            if (PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                var token = await jwtBuilder.GenerateJwtAuthToken(user, cancellationToken);

                return new AuthenticateResponse(token);
            }
            else
            {
                throw new Exception(AuthenticationFailedMessage);
            }
        }
    }

    public sealed class AuthenticateValidator : AbstractValidator<AuthenticateRequest>
    {
        public AuthenticateValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
