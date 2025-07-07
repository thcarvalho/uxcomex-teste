namespace UXComex.Domain.DTOs.Client;

public record ClientRequestDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}