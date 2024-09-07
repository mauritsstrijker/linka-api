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

        public static Organization Create
            (
            string cnpj,
            string companyName,
            string tradingName,
            string phone,
            Address address,
            User user
            )
        {
            return new Organization
            {
                Id = Guid.NewGuid(),
                CNPJ = cnpj,
                CompanyName = companyName,
                TradingName = tradingName,
                Phone = phone,
                Address = address,
                User = user
            };
        }
    }
}
