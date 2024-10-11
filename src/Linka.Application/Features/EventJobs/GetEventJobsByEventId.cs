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
        List<SubscribedVolunteerDto> VolunteersSubscribed
        );

    public class GetEventJobsByEventIdHandler
        (
        IEventJobRepository eventJobRepository,
        IJobVolunteerActivityRepository jobVolunteerActivityRepository
        )
        : IRequestHandler<GetEventJobsByEventIdRequest, IEnumerable<GetEventJobsByEventIdResponse>>
    {
        public async Task<IEnumerable<GetEventJobsByEventIdResponse>> Handle(GetEventJobsByEventIdRequest request, CancellationToken cancellationToken)
        {
            var eventJobs = await eventJobRepository.GetAllJobsByEventId(request.EventId, cancellationToken);

            var response = await Task.WhenAll(eventJobs.Select(async job =>
            {
                var subscribedVolunteers = await Task.WhenAll(job.Volunteers.Select(async v =>
                {
                    var volunteerActivity = await jobVolunteerActivityRepository.GetByJobAndVolunteer(job.Id, v.Id, cancellationToken);

                    return new SubscribedVolunteerDto(
                        v.Id,
                        v.CPF,
                        v.FullName,
                        v.ProfilePictureBytes != null ? Convert.ToBase64String(v.ProfilePictureBytes) : null,
                        volunteerActivity?.CheckIn, 
                        volunteerActivity?.CheckOut
                    );
                }));

                return new GetEventJobsByEventIdResponse(
                    job.Id,
                    job.Title,
                    job.Description,
                    job.MaxVolunteers,
                    job.Event.Id,
                    job.Volunteers.Count,
                    subscribedVolunteers.ToList()
                );
            }));

            return response.ToList();
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
