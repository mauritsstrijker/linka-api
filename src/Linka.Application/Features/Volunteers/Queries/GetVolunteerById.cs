using FluentValidation;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Volunteers.Queries
{
    public sealed record GetVolunteerByIdRequest(Guid VolunteerId) : IRequest<GetVolunteerByIdResponse>;
    
    public sealed record GetVolunteerByIdResponse(string Name, string Surname, string FullName, string Username, string? ProfilePictureBase64, string? ProfilePictureExtension, string Email);

    public sealed class GetVolunteerByIdHandler
        (
        IVolunteerRepository volunteerRepository
        ) : IRequestHandler<GetVolunteerByIdRequest, GetVolunteerByIdResponse>
    {
        public async Task<GetVolunteerByIdResponse> Handle(GetVolunteerByIdRequest request, CancellationToken cancellationToken)
        {
            var volunteer = await volunteerRepository.Get(request.VolunteerId, cancellationToken) ?? throw new Exception();

            return new GetVolunteerByIdResponse(
                volunteer.Name,
                volunteer.Surname,
                volunteer.FullName,
                volunteer.User.Username,
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
