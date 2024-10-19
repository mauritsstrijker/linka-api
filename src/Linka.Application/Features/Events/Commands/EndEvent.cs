using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Application.Features.Events.Commands
{
    public sealed record EndEventRequest
       (
           Guid EventId
       ) : IRequest<EndEventResponse>;

    public sealed record EndEventResponse;

    public class EndEventHandler
        (
        IJwtClaimService jwtClaimService,
        IEventRepository eventRepository,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<EndEventRequest, EndEventResponse>
    {
        public async Task<EndEventResponse> Handle(EndEventRequest request, CancellationToken cancellationToken)
        {
            //TODO pensar o que fazer apos Endar, se iremos remover os eventjobs dos voluntarios etc

            var @event = await eventRepository.Get(request.EventId, cancellationToken);

            await ValidateCurrentOrganization(@event);

            @event.Status = EventStatus.Completed;

            await eventRepository.Update(@event, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new EndEventResponse();
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

    public class EndEventValidator : AbstractValidator<EndEventRequest>
    {
        public EndEventValidator()
        {
            RuleFor(x => x.EventId).NotEmpty();
        }
    }
}
