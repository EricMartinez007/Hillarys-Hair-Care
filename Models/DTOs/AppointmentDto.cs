namespace HillarysHair.Models.DTOs;

public class AppointmentDTO
{
    public int Id { get; set; }
    public int StylistId { get; set; }
    public int CustomerId { get; set; }
    public string StylistName { get; set; }
    public string CustomerName { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; }
    public decimal TotalCost { get; set; }
    public List<ServiceDTO> Services { get; set; }
}

public class AppointmentFormDTO
{
    public int StylistId { get; set; }
    public int CustomerId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public List<int> ServiceIds { get; set; }
}

public class AppointmentServicesUpdateDTO
{
    public List<int> ServiceIds { get; set; }
}
