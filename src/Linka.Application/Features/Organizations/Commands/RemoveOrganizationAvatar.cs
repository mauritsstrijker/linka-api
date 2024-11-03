using Linka.Application.Common;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Organizations.Commands
{
    public class RemoveOrganizationAvatarRequest : IRequest<RemoveOrganizationAvatarResponse>
    {
    }

    public class RemoveOrganizationAvatarResponse;

    public class RemoveOrganizationHandler
        (
        IOrganizationRepository repository,
        IJwtClaimService jwtClaimService
        )
        : IRequestHandler<RemoveOrganizationAvatarRequest, RemoveOrganizationAvatarResponse>
    {
        public async Task<RemoveOrganizationAvatarResponse> Handle(RemoveOrganizationAvatarRequest request, CancellationToken cancellationToken)
        {
            var organizationId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

            var organization = await repository.Get(organizationId, cancellationToken) ?? throw new Exception();

            organization.ProfilePictureExtension = null;
            organization.ProfilePictureBytes = null;

            return new RemoveOrganizationAvatarResponse();
        }
    }
}
