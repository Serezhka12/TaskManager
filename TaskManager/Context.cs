using TaskManager.Entities;

namespace TaskManager;

public class Context
{
    public Role? CurrentUserRole { get; set; } = null;
    public int? CurrentUserId { get; set; } = null;
}