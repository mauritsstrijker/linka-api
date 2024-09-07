using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string CNPJ { get; set; }
        public string CompanyName { get; set; }
        public string TradingName { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public User User { get; set; }
    }
}
