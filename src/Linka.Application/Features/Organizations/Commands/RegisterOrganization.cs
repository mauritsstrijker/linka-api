using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Organizations.Commands
{
    public sealed record RegisterOrganizationRequest
     (
         string CNPJ,
         string CompanyName,
         string TradingName,
         string Phone,
         string Username,
         string Password,
         string Email,
         AddressDto Address,
         string? ProfilePictureBase64
     )
     : BaseRegisterRequest(Username, Password, Email, Address, ProfilePictureBase64), IRequest<RegisterOrganizationResponse>;


    public sealed record RegisterOrganizationResponse();

    public sealed class RegisterOrganizationHandler
        (
        IUserRepository userRepository,
        IOrganizationRepository organizationRepository,
        IRepository<Address> addressRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<RegisterOrganizationRequest, RegisterOrganizationResponse>
    {
        public async Task<RegisterOrganizationResponse> Handle(RegisterOrganizationRequest request, CancellationToken cancellationToken)
        {
            await ValidateUniquenessAsync(request, cancellationToken);

            var user = await CreateUser(request, cancellationToken);

            var address = await CreateAddress(request, cancellationToken);

            await CreateOrganization(request, address, user, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new RegisterOrganizationResponse();
        }

        private async Task<User> CreateUser(RegisterOrganizationRequest request, CancellationToken cancellationToken)
        {
            var user = User.Create(request.Username, request.Email, request.Password, UserType.Organization);

            await userRepository.Insert(user, cancellationToken);

            return user;
        }

        private async Task<Address> CreateAddress(RegisterOrganizationRequest request, CancellationToken cancellationToken)
        {
            var address = Address.Create
                 (
                 request.Address.Cep,
                 request.Address.City,
                 request.Address.Street,
                 request.Address.Number,
                 request.Address.Neighborhood,
                 request.Address.State,
                 request.Address.Nickname
                 );

            await addressRepository.Insert(address, cancellationToken);

            return address;
        }

        private async Task CreateOrganization(RegisterOrganizationRequest request, Address address, User user, CancellationToken cancellationToken)
        {
            var profilePictureBytes = request.ProfilePictureBase64 is null ? null : Convert.FromBase64String(request.ProfilePictureBase64);

            var profilePictureExtension = profilePictureBytes != null
                ? ProfilePictureHelper.GetImageExtension(profilePictureBytes)
                : null;

            var organization = Organization.Create(request.CNPJ, request.CompanyName, request.TradingName, request.Phone, address, user, profilePictureBytes, profilePictureExtension);

            await organizationRepository.Insert(organization, cancellationToken);
        }

        private async Task ValidateUniquenessAsync(RegisterOrganizationRequest request, CancellationToken cancellationToken)
        {
            await EnsureUserIsUniqueAsync(request.Username, request.Email, cancellationToken);
            await EnsureCNPJIsUniqueAsync(request.CNPJ, cancellationToken);
        }

        private async Task EnsureUserIsUniqueAsync(string username, string email, CancellationToken cancellationToken)
        {
            if (await userRepository.GetByUsername(username.Trim(), cancellationToken) is not null)
            {
                throw new Exception("Usuário já cadastrado.");
            }

            if (await userRepository.GetByEmail(email.Trim(), cancellationToken) is not null)
            {
                throw new Exception("Email já cadastrado.");
            }
        }

        private async Task EnsureCNPJIsUniqueAsync(string cnpj, CancellationToken cancellationToken)
        {
            if (await organizationRepository.GetByCNPJ(cnpj.Trim(), cancellationToken) is not null)
            {
                throw new Exception("CNPJ já cadastrado.");
            }
        }
    }

    public sealed class RegisterOrganizationValidator : AbstractValidator<RegisterOrganizationRequest>
    {
        public RegisterOrganizationValidator()
        {
            RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Nome de usuário é obrigatório.")
            .Length(3, 50).WithMessage("Nome de usuário deve ter entre 3 e 50 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.CNPJ)
                .NotEmpty().WithMessage("CNPJ é obrigatório.")
                .Length(14).WithMessage("CNPJ deve ter 14 caracteres.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Nome da empresa é obrigatório.");

            RuleFor(x => x.TradingName)
                .NotEmpty().WithMessage("Nome fantasia é obrigatório.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefone é obrigatório.");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("Endereço é obrigatório.")
                .ChildRules(address =>
                {
                    address.RuleFor(x => x.Cep)
                        .NotEmpty().WithMessage("CEP é obrigatório.")
                        .Length(8).WithMessage("CEP deve ter 8 caracteres.");

                    address.RuleFor(x => x.City)
                        .NotEmpty().WithMessage("Cidade é obrigatória.");

                    address.RuleFor(x => x.Street)
                        .NotEmpty().WithMessage("Rua é obrigatória.");

                    address.RuleFor(x => x.Number)
                        .GreaterThan(0).WithMessage("Número deve ser maior que 0.");

                    address.RuleFor(x => x.Neighborhood)
                        .NotEmpty().WithMessage("Bairro é obrigatório.");

                    address.RuleFor(x => x.State)
                        .NotEmpty().WithMessage("Estado é obrigatório.");

                    address.RuleFor(x => x.Nickname)
                        .MaximumLength(50).WithMessage("Apelido deve ter no máximo 50 caracteres.");
                });

            RuleFor(x => x.ProfilePictureBase64)
              .MustAsync(async (base64, cancellationToken) => await ProfilePictureHelper.ValidateImageAsync(Convert.FromBase64String(base64), 600, 600))
              .When(x => x.ProfilePictureBase64 != null)
              .WithMessage("Imagem de perfil inválida.");
        }
    }
}
