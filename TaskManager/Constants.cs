namespace TaskManager;

public static class Constants
{
    public const string CreateMan = "CreateMan";
    public const string CreateDev = "CreateDev";
    public const string Login = "Login";
    public const string Exit = "Exit";

    public static readonly List<string> WelcomeCommands =
    [
        CreateMan,
        CreateDev,
        Login,
        Exit
    ];

    public const string CreateTask = "CreateTask";
    public const string UpdateTask = "UpdateTask";
    public const string DeleteTask = "DeleteTask";
    public const string GetAllTasks = "GetAllTasks";
    public const string GetAllUsers = "GetAllUsers";
    public const string GetAllDevs = "GetAllDevs";
    public const string AssignTaskToDev = "AssignTaskToDev";

    public static readonly List<string> ManagerCommands =
    [
        CreateTask,
        UpdateTask,
        DeleteTask,
        GetAllTasks,
        GetAllUsers,
        GetAllDevs,
        AssignTaskToDev,
        Logout
    ];

    public const string GetMyTasks = "GetMyTasks";
    public const string CompleteTask = "CompleteTask";
    public const string CompleteManyTasks = "CompleteManyTasks";
    public const string Logout = "Logout";

    public static readonly List<string> DevCommands =
    [
        GetMyTasks,
        CompleteTask,
        CompleteManyTasks,  
        Logout
    ];

    // Console messages
    public const string WelcomeMessage = "Welcome to TaskManager";
    public const string AvailableCommandsFormat = "Available Commands: {0}";
    public const string EnterManagerName = "Enter manager name: ";
    public const string ManagerCreatedSuccess = "Manager successfully created";
    public const string EnterDeveloperName = "Enter developer name: ";
    public const string DeveloperCreatedSuccess = "Developer successfully created";
    public const string EnterUserId = "Enter user ID: ";
    public const string InvalidUserId = "Invalid ID";
    public const string UserNotFound = "User not found";
    public const string LoggedInAsFormat = "You are logged in as {0}";
    public const string LoggedOut = "You have been logged out";
    public const string EnterTaskName = "Enter task name: ";
    public const string EnterTaskDescription = "Enter task description (optional): ";
    public const string TaskCreatedSuccess = "Task created successfully";
    public const string EnterTaskId = "Enter task ID: ";
    public const string EnterNewTaskName = "Enter new task name: ";
    public const string EnterNewTaskDescription = "Enter new task description (optional): ";
    public const string TaskUpdatedSuccess = "Task updated successfully";
    public const string TaskDeletedSuccess = "Task deleted successfully";
    public const string TaskFormat = "ID: {0}, Name: {1}, Description: {2}, Active: {3}";
    public const string EnterDevId = "Enter developer ID: ";
    public const string TaskAssignedSuccess = "Task assigned successfully";
    public const string NoActiveTasks = "You have no active tasks";
    public const string ParallelTasksExecution = "Executing all active tasks in parallel...";
    public const string AllTasksCompleted = "All tasks completed";
    public const string TaskCompleted = "Task completed";
    public const string UnknownCommand = "Unknown command";
    public const string ErrorFormat = "Error: {0}";

    public static void Welcome()
    {
        Console.WriteLine(WelcomeMessage);
        Console.WriteLine();
    }

    public static void WriteAvailableCommands(List<string> commands)
    {
        Console.WriteLine(string.Format(AvailableCommandsFormat, string.Join(", ", commands)));
        Console.WriteLine();
    }
}