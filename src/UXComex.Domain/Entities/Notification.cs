using UXComex.Domain.Abstract;

namespace UXComex.Domain.Entities;

public class Notification : BaseEntity, IAggregateRoot
{
    public DateTime RegisterDate { get; set; }
    public string Message { get; set; }

    public Notification(string message)
    {
        RegisterDate = DateTime.Now;
        Message = message;
    }

    public void Validate()
    {
        throw new NotImplementedException();
    }
}