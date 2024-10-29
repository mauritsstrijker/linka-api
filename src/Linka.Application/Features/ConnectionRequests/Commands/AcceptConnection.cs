using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Commands;
public class AcceptConnectionRequest : IRequest<AcceptConnectionResponse>
{
    public Guid ConnectionRequestId { get; set; }
}

public class AcceptConnectionResponse;

public class AcceptConnectionHandler(IConnectionRequestRepository connectionRequestRepository, IUnitOfWork unitOfWork, IRepository<Connection> connectionRepository) : IRequestHandler<AcceptConnectionRequest, AcceptConnectionResponse>
{
    public async Task<AcceptConnectionResponse> Handle(AcceptConnectionRequest request, CancellationToken cancellationToken)
    {
        var connectionRequest = await connectionRequestRepository.Get(request.ConnectionRequestId, cancellationToken) ?? throw new Exception();

        connectionRequest.Status = ConnectionRequestStatus.Accepted;
        await connectionRequestRepository.Update(connectionRequest, cancellationToken);

        var newConnection = Connection.Create(connectionRequest.Requester, connectionRequest.Target);
        await connectionRepository.Insert(newConnection, cancellationToken);

        await unitOfWork.Commit(cancellationToken);

        return new AcceptConnectionResponse();
    }
}
