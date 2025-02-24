namespace TaskManager.Entities;

public class UserTask(string name)
{
    public int Id { get; set; }

    public string Name { get; set; } = name;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}