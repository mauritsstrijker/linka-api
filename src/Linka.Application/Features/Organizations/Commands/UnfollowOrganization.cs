using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Organizations.Commands
{
    public class UnfollowOrganizationRequest : IRequest<UnfollowOrganizationResponse>
    {
        public Guid OrganizationId { get; set; }
    }

    public class UnfollowOrganizationResponse;

    public class UnfollowOrganizationHandler(IFollowRepository followRepository,
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<UnfollowOrganizationRequest, UnfollowOrganizationResponse>
    {
        public async Task<UnfollowOrganizationResponse> Handle(UnfollowOrganizationRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteer = await volunteerRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken) ?? throw new Exception();

            var organization = await organizationRepository.Get(request.OrganizationId, cancellationToken) ?? throw new Exception();

            if (await followRepository.IsFollowing(organization.Id, currentVolunteer.Id, cancellationToken))
            {
                var follow = await followRepository.GetByOrganizationIdAndVolunteerId(organization.Id, currentVolunteer.Id, cancellationToken);
                await followRepository.Delete(follow, cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }
            return new UnfollowOrganizationResponse();
        }
    }
}
