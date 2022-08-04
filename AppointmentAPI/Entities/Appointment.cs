using AppointmentAPI.Entities.Common;

namespace AppointmentAPI.Entities;

public class Appointment : BaseEntity
{
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTime? Date { get; set; }
    public string? Description { get; set; }
}