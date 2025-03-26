namespace UnoLib1.Entity;

public class CredsEntity(Guid id, string username, string password)
{
    public Guid Id { get; } = id;
    public string Username { get; } = username;
    public string Password { get; } = password;
}
