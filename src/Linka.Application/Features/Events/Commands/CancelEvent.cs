using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record CancelEventRequest
        (
            Guid EventId
        ) : IRequest<CancelEventResponse>;

    public sealed record CancelEventResponse;

    public class CancelEventHandler
        (
        IJwtClaimService jwtClaimService,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<CancelEventRequest, CancelEventResponse>
    {
        public async Task<CancelEventResponse> Handle(CancelEventRequest request, CancellationToken cancellationToken)
        {
            //TODO pensar o que fazer apos cancelar, se iremos remover os eventjobs dos voluntarios etc
            
            var @event = await eventRepository.Get(request.EventId, cancellationToken);

            await ValidateCurrentOrganization(@event);

            @event.Status = EventStatus.Canceled;

            await eventRepository.Update(@event, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new CancelEventResponse();
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

    public class CancelEventValidator : AbstractValidator<CancelEventRequest>
    {
        public CancelEventValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
        }
    }
}
