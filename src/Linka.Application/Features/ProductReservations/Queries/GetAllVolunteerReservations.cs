using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Queries
{
    public class GetAllVolunteersReservationsRequest : IRequest<GetAllVolunteerReservationsResponse>
    {
    }

    public class GetAllVolunteerReservationsResponse
    {
        public List<ReservationDto> Reservations { get; set; }
    }

    public class GetAllVolunteerReservationsHandler
        (
        IJwtClaimService jwtClaimService,
        IProductReservationRepository productReservationRepository
        ) : IRequestHandler<GetAllVolunteersReservationsRequest, GetAllVolunteerReservationsResponse>
    {
        public async Task<GetAllVolunteerReservationsResponse> Handle(GetAllVolunteersReservationsRequest request, CancellationToken cancellationToken)
        {
            var reservations = await productReservationRepository.GetAllByVolunteer(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken);

            return new GetAllVolunteerReservationsResponse { Reservations = MapReservationDtos(reservations) };
        }

        private List<ReservationDto> MapReservationDtos(List<ProductReservation> reservations)
        {
            return reservations.Select(reservation => new ReservationDto
            {
                Id = reservation.Id,
                Withdrawn = reservation.Withdrawn,
                Cancelled = reservation.Cancelled,
                Cost = reservation.Cost,
                VolunteerFullName = reservation.Volunteer.FullName,
                VolunteerId = reservation.Volunteer.Id,
                ProductId = reservation.Product.Id,
                ProductName = reservation.Product.Name,
                RedeemDate = reservation.RedeemDate ?? null,
                DateCreated = reservation.DateCreated
            }).ToList();
        }
    }
}
