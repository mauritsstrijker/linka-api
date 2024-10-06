using FluentValidation;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Events.Queries
{
    public sealed record GetAllEventByOrganizationIdRequest
        (
            Guid OrganizationId
        ) : IRequest<List<GetAllEventByOrganizationIdResponse>>;

    public sealed record GetAllEventByOrganizationIdResponse
        (
        Guid Id,
        string Title,
        string Description,
        DateTime StartDateTime,
        DateTime EndDateTime,
        Address Address,
        EventStatus Status,
        string? ImageBase64
        );

    public class GetAllEventByOrganizationIdHandler
        (
        IEventRepository eventRepository
        )
        : IRequestHandler<GetAllEventByOrganizationIdRequest, List<GetAllEventByOrganizationIdResponse>>
    {
        public async Task<List<GetAllEventByOrganizationIdResponse>> Handle(GetAllEventByOrganizationIdRequest request, CancellationToken cancellationToken)
        {
            var events = await eventRepository.GetByOrganizationId(request.OrganizationId, cancellationToken);

            var response = events.Select(e => new GetAllEventByOrganizationIdResponse(
             e.Id,
             e.Title,
             e.Description,
             e.StartDateTime,
             e.EndDateTime,
             e.Address,
             e.Status,
             e.ImageBytes != null ? Convert.ToBase64String(e.ImageBytes) : null
         )).ToList();


            return response;
        }
    }

    public class GetAllEventByOrganizationIdValidator : AbstractValidator<GetAllEventByOrganizationIdRequest>
    {
        public GetAllEventByOrganizationIdValidator()
        {
            RuleFor(x => x.OrganizationId).NotEmpty();
        }
    }
}
