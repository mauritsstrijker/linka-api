using Linka.Domain.Common;

namespace Linka.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Cep { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Neighborhood { get; set; }
        public string State { get; set; }
        public string Nickname { get; set; }
    }
}
