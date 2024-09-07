using FluentValidation;
using Linka.Application.Repositories;
using Linka.Domain.Helpers;
using MediatR;

namespace Linka.Application.Features.Users.Commands
{
    public sealed record AuthenticateRequest(string Username, string Password) : IRequest<AuthenticateResponse>;

    public sealed record AuthenticateResponse(string Token);

    public sealed class AuthenticateHandler(IUserRepository userRepository) : IRequestHandler<AuthenticateRequest, AuthenticateResponse>
    {
        public async Task<AuthenticateResponse> Handle(AuthenticateRequest request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUsername(request.Username, cancellationToken) ?? throw new Exception("Verifique seus dados.");

            if (PasswordHelper.VerifyPassword(request.Password, user.Password))
            {
                return new AuthenticateResponse("abc");
            }
            else
            {
                throw new Exception("Verifique seus dados.");
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
