using Linka.Application.Common;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Connections;
public class CheckConnectionRequest : IRequest<CheckConnectionResponse>
{
    public Guid VolunteerId { get; set; }
}
public class CheckConnectionResponse
{
    public bool IsConnected { get; set; }
}
public class CheckConnectionHandler
    (
    IConnectionRepository connectionRepository,
    IJwtClaimService jwtClaimService
    )
    : IRequestHandler<CheckConnectionRequest, CheckConnectionResponse>
{
    public async Task<CheckConnectionResponse> Handle(CheckConnectionRequest request, CancellationToken cancellationToken)
    {
        Guid currentVolunteerId = Guid.Parse(jwtClaimService.GetClaimValue("id"));
        return new CheckConnectionResponse { IsConnected = await connectionRepository.HasConnectionAsync(currentVolunteerId, request.VolunteerId, cancellationToken) };
    }
}
