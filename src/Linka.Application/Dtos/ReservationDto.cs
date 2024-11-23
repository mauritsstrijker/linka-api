namespace Linka.Application.Dtos
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public bool Withdrawn { get; set; }
        public bool Cancelled { get; set; }
        public int Cost { get; set; }
        public Guid VolunteerId { get; set; }
        public string VolunteerFullName { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public DateTime? RedeemDate { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
