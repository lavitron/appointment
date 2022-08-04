namespace AppointmentAPI.Entities.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
