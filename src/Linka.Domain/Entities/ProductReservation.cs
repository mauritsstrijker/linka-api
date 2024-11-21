using Linka.Domain.Common;

namespace Linka.Domain.Entities;
public class ProductReservation : BaseEntity
{
    public DateTime? RedeemDate { get; set; }
    public bool Withdrawn { get; set; }
    public bool Cancelled { get; set; }
    public int Cost { get; set; }
    public Product Product { get; set; }
    public Volunteer Volunteer { get; set; }
}
