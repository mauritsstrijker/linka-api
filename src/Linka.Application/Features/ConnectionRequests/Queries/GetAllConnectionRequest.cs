using Linka.Application.Common;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Queries;
public class GetAllConnectionRequest : IRequest<GetAllConnectionRequestResponse>
{
}

public class GetAllConnectionRequestResponse
{
    public List<ConnectionRequest> ConnectionRequests { get; set; }

}

public class GetAllConnectionRequestHandler : IRequestHandler<GetAllConnectionRequest, GetAllConnectionRequestResponse>
{
    private readonly IJwtClaimService _jwtClaimService;
    private readonly IConnectionRequestRepository _connectionRequestRepository;

    public GetAllConnectionRequestHandler(IJwtClaimService jwtClaimService, IConnectionRequestRepository connectionRequestRepository)
    {
        _jwtClaimService = jwtClaimService;
        _connectionRequestRepository = connectionRequestRepository;
    }

    public async Task<GetAllConnectionRequestResponse> Handle(GetAllConnectionRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(_jwtClaimService.GetClaimValue("id"));

        var connectionRequests = await _connectionRequestRepository.GetByTargetIdAsync(currentUserId, cancellationToken);

        return new GetAllConnectionRequestResponse
        {
            ConnectionRequests = connectionRequests
        };
    }
}