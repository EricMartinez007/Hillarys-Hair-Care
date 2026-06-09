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
app.MapPost("/api/appointments", (HillarysHairDbContext db, AppointmentCreateDTO newAcDTO) =>
{
    //need to go through the list of services and match the ones that have the same id as the newAcDTO ServiceIds coming in to be able to calculate total cost of the appointment
    List<Service> services = db.Services
        .Where(s => newAcDTO.ServiceIds.Contains(s.Id))
        .ToList();

    Appointment appointment = new Appointment
    {
        StylistId = newAcDTO.StylistId,
        CustomerId = newAcDTO.CustomerId,
        ScheduledAt = newAcDTO.ScheduledAt,
        Status = "Scheduled",
        TotalCost = services.Sum(s => s.Price),
        AppointmentServices = newAcDTO.ServiceIds.Select(id => new AppointmentService
        {
            ServiceId = id
        }).ToList()
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

app.Run();