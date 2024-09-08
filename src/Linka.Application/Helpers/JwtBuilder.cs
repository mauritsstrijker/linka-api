using Linka.Application.Repositories;
using Linka.Domain.Entities;
using Linka.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Linka.Application.Helpers
{
    public class JwtBuilder
        (
        IVolunteerRepository volunteerRepository,
        IOrganizationRepository organizationRepository,
        IConfiguration configuration
        )
    {
        public async Task<string> GenerateJwtAuthToken(User user, CancellationToken cancellationToken)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Authentication:Secret"]);
            var identity = new ClaimsIdentity();

            string displayName;
            string userType;
            string volunteerOrOrganizationId;

            if (user.Type == UserType.Volunteer)
            {
                var volunteer = await volunteerRepository.GetByUserId(user.Id, cancellationToken);
                displayName = volunteer.FullName;
                userType = "volunteer";
                volunteerOrOrganizationId = volunteer.Id.ToString();
            }
            else if (user.Type == UserType.Organization)
            {
                var organization = await organizationRepository.GetByUserId(user.Id, cancellationToken);
                displayName = organization.TradingName;
                userType = "organization";
                volunteerOrOrganizationId = organization.Id.ToString();
            } else
            {
                throw new Exception("Operação inválida.");
            }

            identity.AddClaims(
            [
                new Claim("userId", user.Id.ToString()),
                new Claim("id", volunteerOrOrganizationId),
                new Claim("displayName", displayName),
                new Claim("type", userType)
            ]);

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
