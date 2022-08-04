namespace AppointmentAPI.Dto
{
    public class AppointmentUpdateDto
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }
}
