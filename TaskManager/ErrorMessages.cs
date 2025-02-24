namespace TaskManager;

public static class ErrorMessages
{
    public static string UserNotFound(int userId) => $"There is no user with this userId: {userId}";

    public static string UserTasksNotFound(int userId) => $"User with this userId {userId} has no tasks";
    public static string CanNotCreateTask() => "Task can not be created";
    public static string CanNotDeleteTask() => "Task can not be deleted";
    public static string CanNotUpdateTask() => "Task can not be updated as it dont exist";
    public static string CanNotAssignTask() => "Task can not be assigned to user";
    public static string CanNotCreateDeveloper() => "Developer can not be created";
    public static string CanNotCreateManager() => "Manager can not be created";


}