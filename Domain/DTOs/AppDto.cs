namespace Domain.DTOs;

public class AppDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class CreateAppDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateAppDto
{
    public string Name { get; set; } = string.Empty;
}
