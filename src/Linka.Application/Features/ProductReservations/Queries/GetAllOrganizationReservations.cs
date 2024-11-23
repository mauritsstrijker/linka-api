using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.ProductReservations.Queries
{
    public class GetAllOrganizationReservationsRequest : IRequest<GetAllOrganizationReservationsResponse>
    {
    }

    public class GetAllOrganizationReservationsResponse
    {
        public List<ReservationDto> Reservations { get; set; }
    }

    public class GetAllOrganizationReservationsHandler
        (
        IJwtClaimService jwtClaimService,
        IProductReservationRepository productReservationRepository
        ): IRequestHandler<GetAllOrganizationReservationsRequest, GetAllOrganizationReservationsResponse>
    {
        public async Task<GetAllOrganizationReservationsResponse> Handle(GetAllOrganizationReservationsRequest request, CancellationToken cancellationToken)
        {
            var reservations = await productReservationRepository.GetAllByOrganization(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken);

            return new GetAllOrganizationReservationsResponse { Reservations = MapReservationDtos(reservations) };
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
