using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.EventJobs
{
    public sealed record GetEventJobsByEventIdRequest(Guid EventId) : IRequest<IEnumerable<GetEventJobsByEventIdResponse>>;

    public sealed record GetEventJobsByEventIdResponse
        (
        string Title,
        string Description,
        int MaxVolunteers,
        Guid EventId
        );

    public class GetEventJobsByEventIdHandler
        (
        IEventJobRepository eventJobRepository
        )
        : IRequestHandler<GetEventJobsByEventIdRequest, IEnumerable<GetEventJobsByEventIdResponse>>
    {
        public async Task<IEnumerable<GetEventJobsByEventIdResponse>> Handle(GetEventJobsByEventIdRequest request, CancellationToken cancellationToken)
        {
            var eventJobs = await eventJobRepository.GetAllJobsByEventId(request.EventId, cancellationToken);

            var response = eventJobs.Select(job => new GetEventJobsByEventIdResponse(
                job.Title,
                job.Description,
                job.MaxVolunteers,
                job.Event.Id
            )).ToList();

            return response;
        }
    }

    public class GetEventJobsByEventIdValidator : AbstractValidator<GetEventJobsByEventIdRequest>
    {
        public GetEventJobsByEventIdValidator() 
        { 
            RuleFor(x => x.EventId).NotEmpty().NotNull();
        }
    }
}
