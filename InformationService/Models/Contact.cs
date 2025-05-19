namespace InformationService.Models
{
    public class Contact
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
