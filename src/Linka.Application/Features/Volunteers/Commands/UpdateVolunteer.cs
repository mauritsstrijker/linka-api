using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Volunteers.Commands
{
    public class UpdateVolunteerRequest : IRequest<UpdateVolunteerResponse>
    {
        public string? ProfilePictureBase64 { get; set; }
        public CreateAddressDto? Address { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email {  get; set; }
    }

    public class UpdateVolunteerResponse;

    public class UpdateVolunteerHandler
        (
        IVolunteerRepository volunteerRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<UpdateVolunteerRequest, UpdateVolunteerResponse>
    {
        public async Task<UpdateVolunteerResponse> Handle(UpdateVolunteerRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteerId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

            var currentVolunteer = await volunteerRepository.Get(currentVolunteerId, cancellationToken) ?? throw new Exception();

            if (request.ProfilePictureBase64 is not null)
            {
                currentVolunteer.ProfilePictureBytes = Convert.FromBase64String(request.ProfilePictureBase64);
            }

            if (request.Name is not null && request.Surname is not null)
            {
                currentVolunteer.Name = request.Name;
                currentVolunteer.Surname = request.Surname;
            }

            if (request.Address is not null)
            {
                currentVolunteer.Address.City = request.Address.City;
                currentVolunteer.Address.Number = request.Address.Number;
                currentVolunteer.Address.Cep = request.Address.Cep;
                currentVolunteer.Address.State = request.Address.State;
                currentVolunteer.Address.Street = request.Address.Street;
                currentVolunteer.Address.Neighborhood = request.Address.Neighborhood;
            }

            if (request.Email is not null)
            {
                currentVolunteer.User.Email = request.Email;
            }

            await volunteerRepository.Update(currentVolunteer, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new UpdateVolunteerResponse();
        }
    }
}
