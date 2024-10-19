using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record StartEventRequest
       (
           Guid EventId
       ) : IRequest<StartEventResponse>;

    public sealed record StartEventResponse;

    public class StartEventHandler
        (
        IJwtClaimService jwtClaimService,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<StartEventRequest, StartEventResponse>
    {
        public async Task<StartEventResponse> Handle(StartEventRequest request, CancellationToken cancellationToken)
        {
            //TODO pensar o que fazer apos cancelar, se iremos remover os eventjobs dos voluntarios etc

            var @event = await eventRepository.Get(request.EventId, cancellationToken);

            await ValidateCurrentOrganization(@event);

            @event.Status = EventStatus.InProgress;

            await eventRepository.Update(@event, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new StartEventResponse();
        }

        private async Task ValidateCurrentOrganization(Event @event)
        {
            var currentOrganizationId = jwtClaimService.GetClaimValue("id");

            if (@event.Organization.Id != Guid.Parse(currentOrganizationId))
            {
                throw new Exception("Este evento nao pertence a sua organizacao.");
            }
        }
    }

    public class StartEventValidator : AbstractValidator<StartEventRequest>
    {
        public StartEventValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
        }
    }
}
