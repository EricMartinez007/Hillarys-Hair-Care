using Microsoft.EntityFrameworkCore;
using HillarysHair.Models;

public class HillarysHairDbContext : DbContext
{
    public DbSet<Stylist> Stylists { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<AppointmentService> AppointmentServices { get; set; }

    public HillarysHairDbContext(DbContextOptions<HillarysHairDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppointmentService>()
            .HasKey(apptSvc => new { apptSvc.AppointmentId, apptSvc.ServiceId });

        modelBuilder.Entity<Stylist>().HasData(new Stylist[]
        {
            new Stylist { Id = 1, Name = "Nami", Phone = "(803) 555-0101", Email = "nami@strawhat.crew", IsActive = true },
            new Stylist { Id = 2, Name = "Nico Robin", Phone = "(803) 555-0102", Email = "robin@strawhat.crew", IsActive = true },
            new Stylist { Id = 3, Name = "Boa Hancock", Phone = "(803) 555-0103", Email = "hancock@kuja.pirates", IsActive = true },
            new Stylist { Id = 4, Name = "Perona", Phone = "(803) 555-0104", Email = "perona@thriller.bark", IsActive = false }
        });

        modelBuilder.Entity<Customer>().HasData(new Customer[]
        {
            new Customer { Id = 1, Name = "Monkey D. Luffy", Phone = "(803) 555-0201", Email = "luffy@strawhat.crew" },
            new Customer { Id = 2, Name = "Roronoa Zoro", Phone = "(803) 555-0202", Email = "zoro@strawhat.crew" },
            new Customer { Id = 3, Name = "Vinsmoke Sanji", Phone = "(803) 555-0203", Email = "sanji@strawhat.crew" },
            new Customer { Id = 4, Name = "Usopp", Phone = "(803) 555-0204", Email = "usopp@strawhat.crew" },
            new Customer { Id = 5, Name = "Tony Tony Chopper", Phone = "(803) 555-0205", Email = "chopper@strawhat.crew" },
            new Customer { Id = 6, Name = "Trafalgar Law", Phone = "(803) 555-0206", Email = "law@heart.pirates" },
            new Customer { Id = 7, Name = "Portgas D. Ace", Phone = "(803) 555-0207", Email = "ace@whitebeard.pirates" },
            new Customer { Id = 8, Name = "Dracule Mihawk", Phone = "(803) 555-0208", Email = "mihawk@shichibukai.gov" }
        });

        modelBuilder.Entity<Service>().HasData(new Service[]
        {
            new Service { Id = 1, Name = "Haircut", Price = 35.00m },
            new Service { Id = 2, Name = "Coloring", Price = 75.00m },
            new Service { Id = 3, Name = "Beard Trim", Price = 20.00m },
            new Service { Id = 4, Name = "Deep Conditioning", Price = 45.00m }
        });

        modelBuilder.Entity<Appointment>().HasData(new Appointment[]
        {
            new Appointment { Id = 1, StylistId = 1, CustomerId = 1, ScheduledAt = new DateTime(2026, 6, 10, 10, 0, 0, DateTimeKind.Utc), Status = "Scheduled", TotalCost = 55.00m },
            new Appointment { Id = 2, StylistId = 2, CustomerId = 2, ScheduledAt = new DateTime(2026, 6, 10, 14, 0, 0, DateTimeKind.Utc), Status = "Scheduled", TotalCost = 110.00m },
            new Appointment { Id = 3, StylistId = 1, CustomerId = 3, ScheduledAt = new DateTime(2026, 6, 5, 9, 0, 0, DateTimeKind.Utc), Status = "Completed", TotalCost = 65.00m },
            new Appointment { Id = 4, StylistId = 3, CustomerId = 4, ScheduledAt = new DateTime(2026, 6, 11, 11, 0, 0, DateTimeKind.Utc), Status = "Scheduled", TotalCost = 35.00m },
            new Appointment { Id = 5, StylistId = 2, CustomerId = 5, ScheduledAt = new DateTime(2026, 6, 3, 13, 0, 0, DateTimeKind.Utc), Status = "Cancelled", TotalCost = 110.00m },
            new Appointment { Id = 6, StylistId = 1, CustomerId = 6, ScheduledAt = new DateTime(2026, 6, 12, 15, 0, 0, DateTimeKind.Utc), Status = "Scheduled", TotalCost = 80.00m },
            new Appointment { Id = 7, StylistId = 3, CustomerId = 7, ScheduledAt = new DateTime(2026, 6, 4, 10, 0, 0, DateTimeKind.Utc), Status = "Completed", TotalCost = 20.00m },
            new Appointment { Id = 8, StylistId = 2, CustomerId = 8, ScheduledAt = new DateTime(2026, 6, 13, 16, 0, 0, DateTimeKind.Utc), Status = "Scheduled", TotalCost = 55.00m }
        });

        modelBuilder.Entity<AppointmentService>().HasData(new AppointmentService[]
        {
            new AppointmentService { AppointmentId = 1, ServiceId = 1 },
            new AppointmentService { AppointmentId = 1, ServiceId = 3 },
            new AppointmentService { AppointmentId = 2, ServiceId = 1 },
            new AppointmentService { AppointmentId = 2, ServiceId = 2 },
            new AppointmentService { AppointmentId = 3, ServiceId = 3 },
            new AppointmentService { AppointmentId = 3, ServiceId = 4 },
            new AppointmentService { AppointmentId = 4, ServiceId = 1 },
            new AppointmentService { AppointmentId = 5, ServiceId = 1 },
            new AppointmentService { AppointmentId = 5, ServiceId = 2 },
            new AppointmentService { AppointmentId = 6, ServiceId = 1 },
            new AppointmentService { AppointmentId = 6, ServiceId = 4 },
            new AppointmentService { AppointmentId = 7, ServiceId = 3 },
            new AppointmentService { AppointmentId = 8, ServiceId = 1 },
            new AppointmentService { AppointmentId = 8, ServiceId = 3 }
        });
    }
}
