using AppointmentAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentAPI.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.FullName).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Phone).HasMaxLength(11).IsRequired();
        builder.Property(p => p.Date).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
    }
}
