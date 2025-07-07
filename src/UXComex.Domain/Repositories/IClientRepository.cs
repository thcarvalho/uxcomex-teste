using UXComex.Domain.Abstract;
using UXComex.Domain.Entities;

namespace UXComex.Domain.Repositories;

public interface IClientRepository : IRepository<Client>
{
    Task<bool> ExistsEmailAsync(string email, string? except = "");
}