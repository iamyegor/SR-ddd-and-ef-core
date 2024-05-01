namespace App;

public interface IBus
{
    void Send(string message);
}

public sealed class Bus : IBus
{
    public void Send(string message)
    {
        // Put the message on a bus instead
        Console.WriteLine($"Message sent: '{message}'");
    }
}

public sealed class MessageBus
{
    private readonly IBus _bus;

    public MessageBus(IBus bus)
    {
        _bus = bus;
    }

    public void SendEmailChangedMessage(long studentId, string newEmail)
    {
        _bus.Send("Type: STUDENT_EMAIL_CHANGED; " + $"Id: {studentId}; " + $"NewEmail: {newEmail}");
    }
}
