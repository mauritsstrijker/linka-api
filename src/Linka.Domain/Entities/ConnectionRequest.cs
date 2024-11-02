using Linka.Domain.Common;
using Linka.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linka.Domain.Entities
{
    public class ConnectionRequest : BaseEntity
    {
        [ForeignKey("RequesterId")]
        public Volunteer Requester { get; set; }

        [ForeignKey("TargetId")]
        public Volunteer Target { get; set; }
        public ConnectionRequestStatus Status { get; set; }
        public static ConnectionRequest Create(Volunteer requester, Volunteer target)
        {
            return new ConnectionRequest
            {
                Id = Guid.NewGuid(),
                Requester = requester,
                Target = target,
                Status = ConnectionRequestStatus.Pending
            };
        }
    }
}
