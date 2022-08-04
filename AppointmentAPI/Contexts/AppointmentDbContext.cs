using AppointmentAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AppointmentAPI.Contexts;
public class AppointmentDbContext : DbContext
{
    public AppointmentDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Appointment>? Appointments { get; set; }
}