using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Helpers;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Volunteers.Commands
{
    public sealed record RegisterVolunteerRequest
        (
            string Username,
            string Email,
            string Password,
            string CPF,
            string Name,
            string Surname,
            CreateAddressDto Address,
            DateTime DathOfBirth,
            byte[]? ProfilePictureBytes,
        )
        : BaseRegisterRequest(Username, Password, Email, Address, ProfilePictureBytes), IRequest<RegisterVolunteerResponse>;

    public sealed record RegisterVolunteerResponse();

    public sealed class RegisterVolunteerHandler 
        (
        IUserRepository userRepository,
        IVolunteerRepository volunteerRepository,
        IRepository<Address> addressRepository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<RegisterVolunteerRequest, RegisterVolunteerResponse>
    {
        public async Task<RegisterVolunteerResponse> Handle(RegisterVolunteerRequest request, CancellationToken cancellationToken)
        {
            await ValidateUniquenessAsync(request, cancellationToken);

            var user = await CreateUser(request, cancellationToken);
            
            var address = await CreateAddress(request, cancellationToken);

            await CreateVolunteer(request, address, user, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new RegisterVolunteerResponse();
        }

        private async Task<User> CreateUser(RegisterVolunteerRequest request, CancellationToken cancellationToken)
        {
            var user = User.Create(request.Username, request.Email, request.Password, UserType.Volunteer);

            await userRepository.Insert(user, cancellationToken);

            return user;
        }

        private async Task<Address> CreateAddress(RegisterVolunteerRequest request, CancellationToken cancellationToken)
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

        private async Task CreateVolunteer(RegisterVolunteerRequest request, Address address, User user, CancellationToken cancellationToken)
        {
            var profilePictureExtension = request.ProfilePictureBytes != null
                ? ProfilePictureHelper.GetImageExtension(request.ProfilePictureBytes)
                : null;

            var volunteer = Volunteer.Create(request.CPF, request.Name, request.Surname, address, request.DathOfBirth, user, request.ProfilePictureBytes, profilePictureExtension);

            await volunteerRepository.Insert(volunteer, cancellationToken);
        }

        private async Task ValidateUniquenessAsync(RegisterVolunteerRequest request, CancellationToken cancellationToken)
        {
            await EnsureUserIsUniqueAsync(request.Username, request.Email, cancellationToken);
            await EnsureCPFIsUniqueAsync(request.CPF, cancellationToken);
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

        private async Task EnsureCPFIsUniqueAsync(string cpf, CancellationToken cancellationToken)
        {
            if (await volunteerRepository.GetByCPF(cpf.Trim(), cancellationToken) is not null)
            {
                throw new Exception("CPF já cadastrado.");
            }
        }
    }

    public sealed class RegisterVolunteerValidator : AbstractValidator<RegisterVolunteerRequest>
    {
        public RegisterVolunteerValidator()
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

            RuleFor(x => x.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Length(11).WithMessage("CPF deve ter 11 caracteres.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Sobrenome é obrigatório.");

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

            RuleFor(x => x.DathOfBirth)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
                .LessThan(DateTime.Now).WithMessage("Data de nascimento deve estar no passado.");

            RuleFor(x => x.ProfilePictureBytes)
              .MustAsync(async (bytes, cancellationToken) => await ProfilePictureHelper.ValidateImageAsync(bytes))
              .When(x => x.ProfilePictureBytes != null)
              .WithMessage("Imagem de perfil inválida.");
        }
    }
}
