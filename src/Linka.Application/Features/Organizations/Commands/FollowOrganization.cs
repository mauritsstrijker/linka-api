using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Organizations.Commands
{
    public class FollowOrganizationRequest : IRequest<FollowOrganizationResponse>
    {
        public Guid OrganizationId { get; set; }
    }

    public class FollowOrganizationResponse;

    public class FollowOrganizationHandler(IFollowRepository followRepository,
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<FollowOrganizationRequest, FollowOrganizationResponse>
    {
        public async Task<FollowOrganizationResponse> Handle(FollowOrganizationRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteer = await volunteerRepository.Get(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken) ?? throw new Exception();

            var organization = await organizationRepository.Get(request.OrganizationId, cancellationToken) ?? throw new Exception();

            if (!await followRepository.IsFollowing(organization.Id, currentVolunteer.Id, cancellationToken))
            {
                var newFollow = Follow.Create(organization, currentVolunteer);
                await followRepository.Insert(newFollow, cancellationToken);
                await unitOfWork.Commit(cancellationToken);
            }
            return new FollowOrganizationResponse();
        }
    }
}
