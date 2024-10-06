using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.EventJobs
{
    public sealed record GetEventJobsByEventIdRequest(Guid EventId) : IRequest<IEnumerable<GetEventJobsByEventIdResponse>>;

    public sealed record GetEventJobsByEventIdResponse
        (
        Guid Id,
        string Title,
        string Description,
        int MaxVolunteers,
        Guid EventId,
        int VolunteersCount,
        List<VolunteerDto> VolunteersSubscribed
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
                job.Id,
                job.Title,
                job.Description,
                job.MaxVolunteers,
                job.Event.Id,
                job.Volunteers.Count,
                job.Volunteers.Select(v => new VolunteerDto(
                    v.Id,
                    v.CPF,
                    v.FullName,
                    v.ProfilePictureBytes != null
                        ? Convert.ToBase64String(v.ProfilePictureBytes)
                        : null 
                )).ToList()

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
