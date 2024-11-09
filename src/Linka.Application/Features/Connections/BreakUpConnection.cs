using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Connections
{
    public class BreakUpConnectionRequest : IRequest<BreakUpConnectionResponse>
    {
        public Guid TargetVolunteerId { get; set; }
    }
    public class BreakUpConnectionResponse
    {

    }
    public class BreakUpConnectionHandler
        (
        IConnectionRepository connectionRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<BreakUpConnectionRequest, BreakUpConnectionResponse>
    {
        public async Task<BreakUpConnectionResponse> Handle(BreakUpConnectionRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteerId = Guid.Parse(jwtClaimService.GetClaimValue("id"));
            var connection = await connectionRepository.GetConnection(currentVolunteerId, request.TargetVolunteerId, cancellationToken);

            if(connection != null)
            {
                await connectionRepository.Delete(connection, cancellationToken);
                await unitOfWork.Commit(cancellationToken);

                return new BreakUpConnectionResponse();
            }

            throw new Exception("Voce nao tem uma conexao com este voluntario.");
        }
    }
}
