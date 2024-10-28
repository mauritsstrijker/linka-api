using Linka.Application.Common;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Organizations.Queries
{
    public class GetAllFollowingOrganizationsRequest : IRequest<List<GetAllFollowingOrganizationsResponse>>
    {
    }

    public class GetAllFollowingOrganizationsResponse
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string CompanyName { get; set; }
        public string TradingName { get; set; }
        public string Phone { get; set; }
        public AddressDto Address { get; set; }
        public string ProfilePictureBase64 { get; set; }
    }

    public class GetAllFolowingOrganizationsHandler(IOrganizationRepository organizationRepository, IJwtClaimService jwtClaimService) : IRequestHandler<GetAllFollowingOrganizationsRequest, List<GetAllFollowingOrganizationsResponse>>
{
        public async Task<List<GetAllFollowingOrganizationsResponse>> Handle(GetAllFollowingOrganizationsRequest request, CancellationToken cancellationToken)
        {
            var organizations = await organizationRepository.GetAllFollowing(Guid.Parse(jwtClaimService.GetClaimValue("id")), cancellationToken);

            return MapToDto(organizations);
        }
        public static List<GetAllFollowingOrganizationsResponse> MapToDto(List<Organization> organizations)
        {
            return organizations.Select(organization => new GetAllFollowingOrganizationsResponse
            {
                UserId = organization.User.Id,
                Id = organization.Id,
                Email = organization.User.Email,
                Username = organization.User.Username,
                CompanyName = organization.CompanyName,
                TradingName = organization.TradingName,
                Phone = organization.Phone,
                Address = new AddressDto
                (
                    Id: organization.Address.Id,
                    Cep: organization.Address.Cep,
                    City: organization.Address.City,
                    Street: organization.Address.Street,
                    Number: organization.Address.Number,
                    Neighborhood: organization.Address.Neighborhood,
                    State: organization.Address.State,
                    Nickname: organization.Address.Nickname
                ),
                ProfilePictureBase64 = organization.ProfilePictureBytes != null
                    ? Convert.ToBase64String(organization.ProfilePictureBytes)
                    : null
            }).ToList();
        }
    }
}
