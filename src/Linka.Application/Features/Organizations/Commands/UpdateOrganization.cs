using Linka.Application.Common;
using Linka.Application.Data;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using MediatR;

namespace Linka.Application.Features.Organizations.Commands
{
    public class UpdateOrganizationRequest : IRequest<UpdateOrganizationResponse>
    {
        public string? ProfilePictureBase64 { get; set; }
        public CreateAddressDto? Address { get; set; }
        public string? TradingName { get; set; }
        public string? About { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateOrganizationResponse;

    public class UpdateOrganizationHandler
        (
        IOrganizationRepository organizationRepository,
        IJwtClaimService jwtClaimService,
        IUnitOfWork unitOfWork
        )
        : IRequestHandler<UpdateOrganizationRequest, UpdateOrganizationResponse>
    {
        public async Task<UpdateOrganizationResponse> Handle(UpdateOrganizationRequest request, CancellationToken cancellationToken)
        {
            var currentOrganizationId = Guid.Parse(jwtClaimService.GetClaimValue("id"));

            var currentOrganization = await organizationRepository.Get(currentOrganizationId, cancellationToken) ?? throw new Exception();

            if (request.ProfilePictureBase64 is not null)
            {
                currentOrganization.ProfilePictureBytes = Convert.FromBase64String(request.ProfilePictureBase64);
            }

            if (request.About is not null)
            {
                currentOrganization.About = request.About;
            }

            if (request.Phone is not null)
            {
                currentOrganization.Phone = request.Phone;
            }

            if (request.TradingName is not null)
            {
                currentOrganization.TradingName = request.TradingName;
            }

            if (request.Address is not null)
            {
                currentOrganization.Address.City = request.Address.City;
                currentOrganization.Address.Number = request.Address.Number;
                currentOrganization.Address.Cep = request.Address.Cep;
                currentOrganization.Address.State = request.Address.State;
                currentOrganization.Address.Street = request.Address.Street;
                currentOrganization.Address.Neighborhood = request.Address.Neighborhood;
            }

            if (request.Email is not null)
            {
                currentOrganization.User.Email = request.Email;
            }

            await organizationRepository.Update(currentOrganization, cancellationToken);

            await unitOfWork.Commit(cancellationToken);

            return new UpdateOrganizationResponse();
        }
    }
}
