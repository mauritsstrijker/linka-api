using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;
using System.Net;

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
            DateTime DathOfBirth
        )
        : IRequest<RegisterVolunteerResponse>;

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
            await ValidateVolunteerUniquenessAsync(request, cancellationToken);

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
            var volunteer = Volunteer.Create(request.CPF, request.Name, request.Surname, address, request.DathOfBirth, user);

            await volunteerRepository.Insert(volunteer, cancellationToken);
        }

        private async Task ValidateVolunteerUniquenessAsync(RegisterVolunteerRequest request, CancellationToken cancellationToken)
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
            RuleFor(x => x.CPF).NotNull();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}
