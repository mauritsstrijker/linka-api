using Linka.Domain.Entities;

namespace Linka.Application.Common
{
    public interface IFeedService
    {
        Task<List<Post>> GetFeedForVolunteerAsync(Guid volunteerId);
        Task<List<Post>> GetOrganizationFeed(Guid organizationId);
    }
}
