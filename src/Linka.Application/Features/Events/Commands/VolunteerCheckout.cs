using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public record VolunteerCheckOutRequest(Guid EventId, Guid VolunteerId) : IRequest<VolunteerCheckOutResponse>;

    public record VolunteerCheckOutResponse;

    public class VolunteerCheckOutHandler
        (
        IEventRepository eventRepository,
        IEventJobRepository eventJobRepository,
        IVolunteerRepository volunteerRepository,
        IJobVolunteerActivityRepository jobVolunteerActivityRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        ) : IRequestHandler<VolunteerCheckOutRequest, VolunteerCheckOutResponse>
    {
        public async Task<VolunteerCheckOutResponse> Handle(VolunteerCheckOutRequest request, CancellationToken cancellationToken)
        {
            var volunteer = await volunteerRepository.Get(request.VolunteerId, cancellationToken);

            var eventJobs = await eventJobRepository.GetAllJobsByEventId(request.EventId, cancellationToken);

            var selectedEventJob = eventJobs.FirstOrDefault(x => x.Volunteers.Contains(volunteer));

            var jobVolunteerActivity = await jobVolunteerActivityRepository.GetByJobAndVolunteer(selectedEventJob.Id, volunteer.Id, cancellationToken);

            if (jobVolunteerActivity is not null)
            {
                jobVolunteerActivity.CheckOut = DateTime.Now;

                if (jobVolunteerActivity.CheckIn.HasValue && jobVolunteerActivity.CheckOut.HasValue)
                {
                    TimeSpan timeWorked = jobVolunteerActivity.CheckOut.Value - jobVolunteerActivity.CheckIn.Value;

                    int pointsEarned = (int)Math.Round(timeWorked.TotalHours);

                    volunteer.Points += pointsEarned;
                    volunteer.AllTimePoints += pointsEarned;

                    await jobVolunteerActivityRepository.Update(jobVolunteerActivity, cancellationToken);

                    await volunteerRepository.Update(volunteer, cancellationToken);

                    await unitOfWork.Commit(cancellationToken);
                }
            }
            return new VolunteerCheckOutResponse();
        }
    }

    public class VolunteerCheckOutValidator : AbstractValidator<VolunteerCheckOutRequest>
    {
        public VolunteerCheckOutValidator()
        {
            RuleFor(x => x.EventId)
                .NotNull()
                .NotEmpty();
        }
    }
}
