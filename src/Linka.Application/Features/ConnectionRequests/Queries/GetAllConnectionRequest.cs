using Linka.Application.Common;
using Linka.Application.Features.ConnectionRequests.Models;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Queries;
public class GetAllConnectionRequest : IRequest<GetAllConnectionRequestResponse>
{
}

public class GetAllConnectionRequestResponse
{
    public List<ConnectionRequestDto> ConnectionRequests { get; set; }

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

        var connectionRequestDtos = connectionRequests.Select(cr => new ConnectionRequestDto
        {
            RequesterId = cr.Requester.Id,
            TargetId = cr.Target.Id,
            Status = cr.Status
        }).ToList();

        return new GetAllConnectionRequestResponse
        {
            ConnectionRequests = connectionRequestDtos
        };
    }
}