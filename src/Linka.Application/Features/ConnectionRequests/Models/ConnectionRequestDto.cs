using Linka.Domain.Enums;

namespace Linka.Application.Features.ConnectionRequests.Models
{
    public class ConnectionRequestDto
    {
        public Guid RequesterId { get; set; }
        public Guid TargetId { get; set; }
        public ConnectionRequestStatus Status { get; set; }
    }
}
