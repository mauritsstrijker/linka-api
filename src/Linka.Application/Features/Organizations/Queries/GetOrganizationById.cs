using FluentValidation;
using Linka.Application.Dtos;
using Linka.Application.Repositories;
using Linka.Domain.Entities;
using MediatR;

namespace Linka.Application.Features.Organizations.Queries
{
    public sealed record GetOrganizationByIdRequest
        (
        Guid Id
        ) : IRequest<GetOrganizationByIdResponse>;

    public sealed record GetOrganizationByIdResponse
        (
            Guid UserId,
            string Email,
            string Username,
            string CompanyName,
            string TradingName,
            string Phone,
            string? About,
            AddressDto Address,
            string ProfilePictureBase64,
            int FollowersCount
        );

    public class GetOrganizationByIdHandler
        (
        IOrganizationRepository organizationRepository,
        IFollowRepository followRepository
        ) : IRequestHandler<GetOrganizationByIdRequest, GetOrganizationByIdResponse>
    {
        public async Task<GetOrganizationByIdResponse> Handle(GetOrganizationByIdRequest request, CancellationToken cancellationToken)
        {
            var organization = await organizationRepository.Get(request.Id, cancellationToken) ?? throw new Exception("Organização não encontrada.");
            var followersCount = await followRepository.FollowersCountById(organization.Id, cancellationToken);

            return MapToDto(organization, followersCount);
        }

        public static GetOrganizationByIdResponse MapToDto(Organization organization, int followersCount)
        {
            return new GetOrganizationByIdResponse
            (
                UserId: organization.User.Id,
                Email: organization.User.Email,
                Username: organization.User.Username,
                CompanyName: organization.CompanyName,
                TradingName: organization.TradingName,
                Phone: organization.Phone,
                About: organization.About,
                Address: new AddressDto
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
                ProfilePictureBase64: organization.ProfilePictureBytes != null
                    ? Convert.ToBase64String(organization.ProfilePictureBytes)
                    : null,
                FollowersCount: followersCount
            ); 
        }
    }
    public class GetOrganizationByIdValidator : AbstractValidator<GetOrganizationByIdRequest>
    {
        public GetOrganizationByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
