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

    public static ProductReservation Create(Product product, Volunteer volunteer)
    {
        return new ProductReservation
        {
            Id = Guid.NewGuid(),
            Product = product,
            Volunteer = volunteer,
            Cost = product.Cost
        };
    }
}
