namespace AppointmentAPI.Dto
{
    public class AppointmentAddDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }
}
