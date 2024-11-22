using Linka.Domain.Common;

namespace Linka.Domain.Entities;
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Cost { get; set; }
    public int AvailableQuantity { get; set; }
    public byte[]? Image { get; set; }
    public Organization Organization { get; set; }  
    public bool IsDeleted { get; set; }

    public static Product Create
        (
        string name,
        string description,
        int cost,
        int availableQuantity,
        byte[]? image,
        Organization organization
        )
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Cost = cost,
            AvailableQuantity = availableQuantity,
            Image = image,
            Organization = organization,
            IsDeleted = false
        };
    }
}
