using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public record VolunteerCheckInRequest(Guid EventId) : IRequest<VolunteerCheckInResponse>;

    public record VolunteerCheckInResponse();

    public class VolunteerCheckInHandler
        (
        IEventRepository eventRepository,
        IEventJobRepository eventJobRepository,
        IVolunteerRepository volunteerRepository,
        IJobVolunteerActivityRepository jobVolunteerActivityRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<VolunteerCheckInRequest, VolunteerCheckInResponse>
    {
        public async Task<VolunteerCheckInResponse> Handle(VolunteerCheckInRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteerId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

            var volunteer = await volunteerRepository.Get(currentVolunteerId, cancellationToken);

            var eventJobs = await eventJobRepository.GetAllJobsByEventId(request.EventId, cancellationToken);

            var selectedEventJob = eventJobs.FirstOrDefault(x => x.Volunteers.Contains(volunteer));

            var jobVolunteerActivity = await jobVolunteerActivityRepository.GetByJobAndVolunteer(selectedEventJob.Id, volunteer.Id, cancellationToken);

            if (
            selectedEventJob is not null && jobVolunteerActivity is null)
            {
                var activity = new JobVolunteerActivity
                {
                    Id = Guid.NewGuid(),
                    Job = selectedEventJob,
                    Volunteer = volunteer,
                    CheckIn = DateTime.Now
                };

                await jobVolunteerActivityRepository.Insert(activity, cancellationToken);

                await unitOfWork.Commit(cancellationToken);
            }

            return new VolunteerCheckInResponse();
        }
    }

    public class VolunteerCheckInValidator : AbstractValidator<VolunteerCheckInRequest>
    {
        public VolunteerCheckInValidator()
        {
            RuleFor(x => x.EventId)
                .NotNull()
                .NotEmpty();
        }
    }
}
