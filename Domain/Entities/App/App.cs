namespace Domain.Entities.App;

public class App
{
    public Int64 Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User.User? User { get; set; }
}
