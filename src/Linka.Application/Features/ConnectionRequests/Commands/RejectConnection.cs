using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Commands;
public class RejectConnectionRequest : IRequest<RejectConnectionResponse>
{
    public Guid ConnectionRequestId { get; set; }
}

public class RejectConnectionResponse;

public class RejectConnectionHandler(IConnectionRequestRepository connectionRequestRepository, IUnitOfWork unitOfWork) : IRequestHandler<RejectConnectionRequest, RejectConnectionResponse>
{
    public async Task<RejectConnectionResponse> Handle(RejectConnectionRequest request, CancellationToken cancellationToken)
    {
        var connectionRequest = await connectionRequestRepository.Get(request.ConnectionRequestId, cancellationToken) ?? throw new Exception();

        connectionRequest.Status = ConnectionRequestStatus.Rejected;
        await connectionRequestRepository.Update(connectionRequest, cancellationToken);

        await unitOfWork.Commit(cancellationToken);

        return new RejectConnectionResponse();
    }
}
