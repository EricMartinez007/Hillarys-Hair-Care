namespace HillarysHair.Models;

public class Appointment
{
    public int Id { get; set; }
    public int StylistId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; }
    public decimal TotalCost { get; set; }
    public Stylist Stylist { get; set; }
    public Customer Customer { get; set; }
    public List<AppointmentService> AppointmentServices { get; set; }
}
