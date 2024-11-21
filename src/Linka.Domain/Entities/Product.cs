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
}
