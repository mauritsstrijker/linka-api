using Linka.Application.Common;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Volunteers.Commands
{
    public class RemoveVolunteerAvatarRequest : IRequest<RemoveVolunteerAvatarResponse>
    {
    }

    public class RemoveVolunteerAvatarResponse;

    public class RemoveVolunteerHandler
        (
        IVolunteerRepository repository,
        IJwtClaimService jwtClaimService
        )
        : IRequestHandler<RemoveVolunteerAvatarRequest, RemoveVolunteerAvatarResponse>
    {
        public async Task<RemoveVolunteerAvatarResponse> Handle(RemoveVolunteerAvatarRequest request, CancellationToken cancellationToken)
        {
            var volunteerId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

            var volunteer = await repository.Get(volunteerId, cancellationToken) ?? throw new Exception();

            volunteer.ProfilePictureExtension = null;
            volunteer.ProfilePictureBytes = null;

            return new RemoveVolunteerAvatarResponse();
        }
    }
}
