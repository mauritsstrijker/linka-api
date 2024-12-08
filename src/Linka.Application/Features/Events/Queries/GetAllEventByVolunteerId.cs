using FluentValidation;
using Linka.Application.Common;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using MediatR;

namespace Linka.Application.Features.Events.Queries
{
    public sealed record GetAllEventByVolunteerIdRequest
        (
        Guid VolunteerId
        ) : IRequest<List<GetAllEventByVolunteerIdResponse>>;

    public sealed record GetAllEventByVolunteerIdResponse
        (
        Guid Id,
        Guid Organizationid,
        string Title,
        string Description,
        DateTime StartDateTime,
        DateTime EndDateTime,
        Address Address,
        EventStatus Status,
        string? ImageBase64
        );

    public class GetAllEventByVolunteerIdHandler
        (
        IEventRepository eventRepository,
        IJwtClaimService jwtClaimService
        )
        : IRequestHandler<GetAllEventByVolunteerIdRequest, List<GetAllEventByVolunteerIdResponse>>
    {
        public async Task<List<GetAllEventByVolunteerIdResponse>> Handle(GetAllEventByVolunteerIdRequest request, CancellationToken cancellationToken)
        {
            var events = await eventRepository.GetByVolunteerId(request.VolunteerId, cancellationToken);

            var response = events.Select(e => new GetAllEventByVolunteerIdResponse(
             e.Id,
             e.Organization.Id,
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

    public class GetAllEventByVolunteerIdValidator : AbstractValidator<GetAllEventByVolunteerIdRequest>
    {
        public GetAllEventByVolunteerIdValidator()
        {
        }
    }
}
