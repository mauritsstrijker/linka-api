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

        ) : IRequest<List<GetAllEventByVolunteerIdResponse>>;

    public sealed record GetAllEventByVolunteerIdResponse
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

    public class GetAllEventByVolunteerIdHandler
        (
        IEventRepository eventRepository,
        IJwtClaimService jwtClaimService
        )
        : IRequestHandler<GetAllEventByVolunteerIdRequest, List<GetAllEventByVolunteerIdResponse>>
    {
        public async Task<List<GetAllEventByVolunteerIdResponse>> Handle(GetAllEventByVolunteerIdRequest request, CancellationToken cancellationToken)
        {
            var currentVolunteerId = jwtClaimService.GetClaimValue("id");

            var events = await eventRepository.GetByVolunteerId(currentVolunteerId, cancellationToken);

            var response = events.Select(e => new GetAllEventByVolunteerIdResponse(
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

    public class GetAllEventByVolunteerIdValidator : AbstractValidator<GetAllEventByVolunteerIdRequest>
    {
        public GetAllEventByVolunteerIdValidator()
        {
            RuleFor(x => x.VolunteerId).NotEmpty();
        }
    }
}
