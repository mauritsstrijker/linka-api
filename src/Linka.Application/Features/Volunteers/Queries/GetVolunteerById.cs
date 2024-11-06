using FluentValidation;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Volunteers.Queries
{
    public sealed record GetVolunteerByIdRequest(Guid VolunteerId) : IRequest<GetVolunteerByIdResponse>;
    
    public sealed record GetVolunteerByIdResponse(Guid UserId, string Name, string Surname, string FullName, string Username, AddressDto Address, int ConnectionsCount, string? ProfilePictureBase64, string? ProfilePictureExtension, string Email);

    public sealed class GetVolunteerByIdHandler
        (
        IVolunteerRepository volunteerRepository,
        IConnectionRepository connectionRepository
        ) : IRequestHandler<GetVolunteerByIdRequest, GetVolunteerByIdResponse>
    {
        public async Task<GetVolunteerByIdResponse> Handle(GetVolunteerByIdRequest request, CancellationToken cancellationToken)
        {
            var volunteer = await volunteerRepository.Get(request.VolunteerId, cancellationToken) ?? throw new Exception();
            var connectionsCount = await connectionRepository.ConnectionsCountByVolunteerId(volunteer.Id, cancellationToken);

            return new GetVolunteerByIdResponse(
                volunteer.User.Id,
                volunteer.Name,
                volunteer.Surname,
                volunteer.FullName,
                volunteer.User.Username,
                Address: new AddressDto
            (
                    Id: volunteer.Address.Id,
                    Cep: volunteer.Address.Cep,
                    City: volunteer.Address.City,
                    Street: volunteer.Address.Street,
                    Number: volunteer.Address.Number,
                    Neighborhood: volunteer.Address.Neighborhood,
                    State: volunteer.Address.State,
                    Nickname: volunteer.Address.Nickname
                ),
                connectionsCount,
                volunteer.ProfilePictureBytes != null ? Convert.ToBase64String(volunteer.ProfilePictureBytes) : null,
                volunteer.ProfilePictureExtension,
                volunteer.User.Email
            );
        }
    }

    public sealed class GetVolunteerByIdValidator : AbstractValidator<GetVolunteerByIdRequest>
    {
        public GetVolunteerByIdValidator()
        {
            RuleFor(x => x.VolunteerId).NotEmpty();
        }
    }
}
