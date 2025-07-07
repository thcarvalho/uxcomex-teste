using UXComex.Domain.DTOs.Client;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Mappers;

public static class ClientMapper
{
    public static Client ToEntity(this ClientRequestDTO clientDto)
        => new Client(
            clientDto.Name,
            clientDto.Email,
            clientDto.Phone);

    public static ClientResponseDto ToDTO(this Client client)
        => new ClientResponseDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            RegisterDate = client.RegisterDate
        };
}