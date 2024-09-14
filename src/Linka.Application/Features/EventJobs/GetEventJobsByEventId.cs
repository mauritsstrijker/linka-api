using MediatR;

namespace Linka.Application.Features.EventJobs
{
    public sealed record GetEventJobsByEventIdRequest(Guid EventId) : IRequest<GetEventJobsByEventIdResponse>;

    public sealed record GetEventJobsByEventIdResponse
        (
        
        );
}
