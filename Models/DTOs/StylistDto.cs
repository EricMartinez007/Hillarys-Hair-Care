namespace HillarysHair.Models.DTOs;

public class StylistDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
}

public class StylistCreateDTO
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
}
