namespace HillarysHair.Models;

public class Stylist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public List<Appointment> Appointments { get; set; }
}
