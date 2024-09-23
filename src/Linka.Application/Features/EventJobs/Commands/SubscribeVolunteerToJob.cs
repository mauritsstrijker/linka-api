using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.EventJobs.Commands
{
    public sealed record SubscribeVolunteerToJobRequest(Guid EventJobId) : IRequest<SubscribeVolunteerToJobResponse>;

    public sealed record SubscribeVolunteerToJobResponse();

    public class SubscribeVolunteerToJobHandler 
        (
        IEventJobRepository eventJobRepository,
        IVolunteerRepository volunteerRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<SubscribeVolunteerToJobRequest, SubscribeVolunteerToJobResponse>
    {
        public async Task<SubscribeVolunteerToJobResponse> Handle(SubscribeVolunteerToJobRequest request, CancellationToken cancellationToken)
        {
            var eventJob = await eventJobRepository.Get(request.EventJobId, cancellationToken) ?? throw new Exception();

            if (eventJob.Volunteers.Count < eventJob.MaxVolunteers)
            {
                var currentVolunteerId = jwtClaimService.GetClaimValue("id");

                var volunteer = await volunteerRepository.Get(Guid.Parse(currentVolunteerId), cancellationToken) ?? throw new Exception();

                eventJob.Volunteers.Add(volunteer);

                await eventJobRepository.Update(eventJob, cancellationToken);

                await unitOfWork.Commit(cancellationToken);

                return new SubscribeVolunteerToJobResponse();

            } else
            {
                throw new Exception("Número máximo de voluntários já atingido para esta função.");
            }

        }
    }

    public class SubscribeVolunteerToJobValidator : AbstractValidator<SubscribeVolunteerToJobRequest>
    {
        public SubscribeVolunteerToJobValidator()
        {
            RuleFor(x => x.EventJobId);
        }
    }
}
