using HillarysHair.Models;
using HillarysHair.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<HillarysHairDbContext>(builder.Configuration["HillarysHairDbConnectionString"]);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Stylists
app.MapGet("/api/stylists", (HillarysHairDbContext db) =>
{
    return db.Stylists
        .Where(s => s.IsActive)
        .Select(s => new StylistDTO
        {
            Id = s.Id,
            Name = s.Name,
            Phone = s.Phone,
            Email = s.Email,
            IsActive = s.IsActive
        })
        .ToList();
});

// Customers
app.MapGet("/api/customers", (HillarysHairDbContext db) =>
{
    return db.Customers
        .Select(c => new CustomerDTO
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email
        })
        .ToList();
});

app.MapPost("/api/customers", (HillarysHairDbContext db, CustomerCreateDTO newDTO) =>
{
    Customer customer = new Customer
    {
        Name = newDTO.Name,
        Phone = newDTO.Phone,
        Email = newDTO.Email
    };

    db.Customers.Add(customer);
    db.SaveChanges();

    return Results.Created($"/customers/{customer.Id}", new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Phone = customer.Phone,
        Email = customer.Email
    });
});

// Services
app.MapGet("/api/services", (HillarysHairDbContext db) =>
{
    return db.Services
        .Select(s => new ServiceDTO
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price
        })
        .ToList();
});

// Appointments
app.MapPost("/api/appointments", (HillarysHairDbContext db, AppointmentFormDTO newDTO) =>
{
    //need to go through the list of services and match the ones that have the same id as the newAcDTO ServiceIds coming in to be able to calculate total cost of the appointment
    List<Service> services = db.Services
        .Where(s => newDTO.ServiceIds.Contains(s.Id))
        .ToList();

    Appointment appointment = new Appointment
    {
        StylistId = newDTO.StylistId,
        CustomerId = newDTO.CustomerId,
        ScheduledAt = newDTO.ScheduledAt,
        Status = "Scheduled",
        TotalCost = services.Sum(s => s.Price),
        AppointmentServices = [.. newDTO.ServiceIds.Select(id => new AppointmentService
        {
            ServiceId = id
        })]
    };

    db.Appointments.Add(appointment);
    db.SaveChanges();

    // reloading the created appointment but were including the stylist and customer objs so we can return their names along with the rest of the newly created appointment
    appointment = db.Appointments
        .Include(a => a.Stylist)
        .Include(a => a.Customer)
        .First(a => a.Id == appointment.Id);

    return Results.Created($"/appointments/{appointment.Id}", new AppointmentDTO
    {
        Id = appointment.Id,
        StylistName = appointment.Stylist.Name,
        CustomerName = appointment.Customer.Name,
        ScheduledAt = appointment.ScheduledAt,
        Status = appointment.Status,
        TotalCost = appointment.TotalCost,
        Services = services.Select(s => new ServiceDTO
        {
            Id = s.Id,
            Name = s.Name,
            Price = s.Price
        }).ToList()
    });
});

app.MapGet("/api/appointments", (HillarysHairDbContext db) =>
{
    return db.Appointments
        .Include(a => a.Stylist)
        .Include(a => a.Customer)
        .Include(a => a.AppointmentServices)
            .ThenInclude(apptSvc => apptSvc.Service)
        .OrderByDescending(a => a.ScheduledAt)
        .Select(a => new AppointmentDTO
        {
            Id = a.Id,
            StylistId = a.StylistId,
            CustomerId = a.CustomerId,
            StylistName = a.Stylist.Name,
            CustomerName = a.Customer.Name,
            ScheduledAt = a.ScheduledAt,
            Status = a.Status,
            TotalCost = a.TotalCost,
            Services = a.AppointmentServices.Select(apptSvc => new ServiceDTO
            {
                Id = apptSvc.Service.Id,
                Name = apptSvc.Service.Name,
                Price = apptSvc.Service.Price
            }).ToList()
        })
        .ToList();
});

app.MapGet("/api/appointments/{id}", (HillarysHairDbContext db, int id) =>
{
    AppointmentDTO? appointment = db.Appointments
        .Include(a => a.Stylist)
        .Include(a => a.Customer)
        .Include(a => a.AppointmentServices)
            .ThenInclude(apptSvc => apptSvc.Service)
        .Where(a => a.Id == id)
        .Select(a => new AppointmentDTO
        {
            Id = a.Id,
            StylistId = a.StylistId,
            CustomerId = a.CustomerId,
            StylistName = a.Stylist.Name,
            CustomerName = a.Customer.Name,
            ScheduledAt = a.ScheduledAt,
            Status = a.Status,
            TotalCost = a.TotalCost,
            Services = a.AppointmentServices.Select(apptSvc => new ServiceDTO
            {
                Id = apptSvc.Service.Id,
                Name = apptSvc.Service.Name,
                Price = apptSvc.Service.Price
            }).ToList()
        })
        .SingleOrDefault();

    if (appointment == null)
        return Results.NotFound();

    return Results.Ok(appointment);
});

app.MapPut("/api/appointments/{id}", (HillarysHairDbContext db, int id, AppointmentFormDTO updateDTO) =>
{
    Appointment? appointment = db.Appointments
        .Include(a => a.AppointmentServices)
        .SingleOrDefault(a => a.Id == id);

    if (appointment == null)
        return Results.NotFound();

    List<Service> services = [.. db.Services.Where(s => updateDTO.ServiceIds.Contains(s.Id))];

    appointment.StylistId = updateDTO.StylistId;
    appointment.CustomerId = updateDTO.CustomerId;
    appointment.ScheduledAt = updateDTO.ScheduledAt;
    appointment.TotalCost = services.Sum(s => s.Price);
    appointment.AppointmentServices = [.. updateDTO.ServiceIds.Select(serviceId => new AppointmentService
    {
        ServiceId = serviceId
    })];

    db.SaveChanges();

    return Results.NoContent();
});

app.MapPatch("/api/appointments/{id}/complete", (HillarysHairDbContext db, int id) =>
{
    Appointment? appointment = db.Appointments.SingleOrDefault(a => a.Id == id);

    if (appointment == null)
        return Results.NotFound();

    if (appointment.Status == "Completed")
        return Results.BadRequest("Appointment is already completed.");

    appointment.Status = "Completed";
    db.SaveChanges();

    return Results.NoContent();
});

app.MapPatch("/api/appointments/{id}/cancel", (HillarysHairDbContext db, int id) =>
{
    Appointment? appointment = db.Appointments.SingleOrDefault(a => a.Id == id);

    if (appointment == null)
        return Results.NotFound();

    if (appointment.Status == "Cancelled")
        return Results.BadRequest("Appointment is already cancelled.");

    appointment.Status = "Cancelled";
    db.SaveChanges();

    return Results.NoContent();
});

app.Run();