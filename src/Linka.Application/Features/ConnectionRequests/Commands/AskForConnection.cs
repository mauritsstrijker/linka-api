using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ConnectionRequests.Commands
{
    public class AskForConnectionRequest : IRequest<AskForConnectionResponse>
    {
        public Guid TargetVolunteerId { get; set; }
    }

    public class AskForConnectionResponse;

    public class AskForConnectionHandler
        (
        IJwtClaimService jwtClaimService,
        IVolunteerRepository volunteerRepository,
        IConnectionRequestRepository repository,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<AskForConnectionRequest, AskForConnectionResponse>
    {
        public async Task<AskForConnectionResponse> Handle(AskForConnectionRequest request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(jwtClaimService.GetClaimValue("id"));
            var requesterVolunteer = await volunteerRepository.GetByUserId(currentUserId, cancellationToken) ?? throw new Exception();
            var targetVolunteer = await volunteerRepository.GetByUserId(request.TargetVolunteerId, cancellationToken) ?? throw new Exception();

            if (!await repository.HasPendingConnectionRequestAsync(currentUserId, request.TargetVolunteerId, cancellationToken)) { 
                var newConnectionRequest = ConnectionRequest.Create(requesterVolunteer, targetVolunteer);

                await repository.Insert(newConnectionRequest, cancellationToken);

                await unitOfWork.Commit(cancellationToken);

                return new AskForConnectionResponse();
            }

            throw new Exception("Já existe um pedido pendente entre os dois usuários.");
        }
    }
}
