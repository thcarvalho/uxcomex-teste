using UXComex.Domain.Abstract;
using UXComex.Domain.Validations;

namespace UXComex.Domain.Entities;

public class Client : BaseEntity, IAggregateRoot
{
    public string Name { get; protected set; }
    public string Email { get; protected set; }
    public string Phone { get; protected set; }
    public DateTime RegisterDate { get; protected set; }

    protected Client() { }
    public Client(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
        RegisterDate = DateTime.Now;

        Validate();
    }

    public void Validate()
        => base.Validate(new ClientEntityValidator(), this);
}