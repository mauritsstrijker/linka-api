using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.EventJobs.Commands
{
    public sealed record UnsubscribeVolunteerToJobRequest(Guid EventJobId) : IRequest<UnsubscribeVolunteerToJobResponse>;

    public sealed record UnsubscribeVolunteerToJobResponse();

    public class UnsubscribeVolunteerToJobHandler
        (
        IEventJobRepository eventJobRepository,
        IVolunteerRepository volunteerRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<UnsubscribeVolunteerToJobRequest, UnsubscribeVolunteerToJobResponse>
    {
        public async Task<UnsubscribeVolunteerToJobResponse> Handle(UnsubscribeVolunteerToJobRequest request, CancellationToken cancellationToken)
        {
            var eventJob = await eventJobRepository.Get(request.EventJobId, cancellationToken) ?? throw new Exception();

            var currentVolunteerId = jwtClaimService.GetClaimValue("id");

            var volunteer = await volunteerRepository.Get(Guid.Parse(currentVolunteerId), cancellationToken) ?? throw new Exception();

            if (eventJob.Volunteers.Remove(volunteer))
            {
                await eventJobRepository.Update(eventJob, cancellationToken);

                await unitOfWork.Commit(cancellationToken);
            }

            return new UnsubscribeVolunteerToJobResponse();
        }
    }

    public class UnsubscribeVolunteerToJobValidator : AbstractValidator<SubscribeVolunteerToJobRequest>
    {
        public UnsubscribeVolunteerToJobValidator()
        {
            RuleFor(x => x.EventJobId);
        }
    }
}
