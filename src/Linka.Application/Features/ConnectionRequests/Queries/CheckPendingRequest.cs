using Linka.Application.Common;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Queries;
public class CheckPendingRequestRequest : IRequest<CheckPendingRequestResponse>
{
    public Guid VolunteerId { get; set; }
}

public class CheckPendingRequestResponse
{
    public bool IsPending { get; set; }
    public bool? IsCurrentUserRequester { get; set; }
}

public class CheckPendingRequestHandler(IConnectionRequestRepository repository, IJwtClaimService jwtClaimService) : IRequestHandler<CheckPendingRequestRequest, CheckPendingRequestResponse>
{
    public async Task<CheckPendingRequestResponse> Handle(CheckPendingRequestRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

        var connectionRequest = await repository.GetPendingRequestAsync(currentUserId, request.VolunteerId, cancellationToken);

        if (connectionRequest != null)
        {
            return new CheckPendingRequestResponse
            {
                IsPending = true,
                IsCurrentUserRequester = connectionRequest.Requester.Id == currentUserId
            };
        }

        return new CheckPendingRequestResponse
        {
            IsPending = false,
            IsCurrentUserRequester = null 
        };
    }
}
