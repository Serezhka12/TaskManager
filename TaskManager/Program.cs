using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Services;

namespace TaskManager;

internal static class Program
{
    private static readonly UserServices _userServices = new();
    private static readonly TaskService _taskService = new();
    private static readonly UserTaskService _userTaskService = new();
    private static readonly Context _context = new();

    static async Task Main()
    {
        while (true)
        {
            if (_context.CurrentUserId == null)
            {
                ShowWelcomeMenu();
            }
            else
            {
                ShowUserMenu();
            }

            var command = Console.ReadLine();
            if (string.IsNullOrEmpty(command)) continue;

            try
            {
                await ProcessCommand(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(Constants.ErrorFormat, ex.Message));
            }
        }
    }

    private static void ShowWelcomeMenu()
    {
        Constants.Welcome();
        Constants.WriteAvailableCommands(Constants.WelcomeCommands);
    }

    private static void ShowUserMenu()
    {
        var commands = _context.CurrentUserRole == Role.Manager ? Constants.ManagerCommands : Constants.DevCommands;
        Constants.WriteAvailableCommands(commands);
    }

    private static async Task ProcessCommand(string command)
    {
        switch (command)
        {
            case Constants.CreateMan:
                await CreateManager();
                break;
            case Constants.CreateDev:
                await CreateDeveloper();
                break;
            case Constants.Login:
                await Login();
                break;
            case Constants.Exit:
                Environment.Exit(0);
                break;
            case Constants.Logout when _context.CurrentUserId != null:
                Logout();
                break;
            case Constants.CreateTask when _context.CurrentUserRole == Role.Manager:
                await CreateTask();
                break;
            case Constants.UpdateTask when _context.CurrentUserRole == Role.Manager:
                await UpdateTask();
                break;
            case Constants.DeleteTask when _context.CurrentUserRole == Role.Manager:
                await DeleteTask();
                break;
            case Constants.GetAllTasks when _context.CurrentUserRole == Role.Manager:
                await GetAllTasks();
                break;
            case Constants.GetAllUsers when _context.CurrentUserRole == Role.Manager:
                await GetAllUsers();
                break;
            case Constants.GetAllDevs when _context.CurrentUserRole == Role.Manager:
                await GetAllDevelopers();
                break;
            case Constants.AssignTaskToDev when _context.CurrentUserRole == Role.Manager:
                await AssignTaskToDev();
                break;
            case Constants.GetMyTasks when _context.CurrentUserRole == Role.Developer:
                await GetMyTasks();
                break;
            case Constants.CompleteTask when _context.CurrentUserRole == Role.Developer:
                await CompleteTask();
                break;
            case Constants.CompleteManyTasks when _context.CurrentUserRole == Role.Developer:
                await CompleteManyTasks();
                break;
            default:
                Console.WriteLine(Constants.UnknownCommand);
                break;
        }
    }

    private static async Task CreateManager()
    {
        Console.Write(Constants.EnterManagerName);
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) return;

        var result = await _userServices.CreateManager(name);
        Console.WriteLine(result.IsSuccess ? Constants.ManagerCreatedSuccess(result.Data) : result.Error);
    }

    private static async Task CreateDeveloper()
    {
        Console.Write(Constants.EnterDeveloperName);
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) return;

        var result = await _userServices.CreateUser(name);
        Console.WriteLine(result.IsSuccess ? Constants.DeveloperCreatedSuccess(result.Data) : result.Error);
    }

    private static async Task Login()
    {
        Console.Write(Constants.EnterUserId);
        if (!int.TryParse(Console.ReadLine(), out var userId))
        {
            Console.WriteLine(Constants.InvalidUserId);
            return;
        }

        var user = await _userServices.GetUserById(userId);
        if (!user.IsSuccess || user.Data == null)
        {
            Console.WriteLine(Constants.UserNotFound);
            return;
        }

        _context.CurrentUserId = userId;
        _context.CurrentUserRole = user.Data.Role;
        Console.WriteLine(string.Format(Constants.LoggedInAsFormat, user.Data.Role));
    }

    private static void Logout()
    {
        _context.CurrentUserId = null;
        _context.CurrentUserRole = null;
        Console.WriteLine(Constants.LoggedOut);
    }

    private static async Task CreateTask()
    {
        Console.Write(Constants.EnterTaskName);
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) return;

        Console.Write(Constants.EnterTaskDescription);
        var description = Console.ReadLine();

        var taskDto = new TaskDto { Name = name, Description = description };
        var result = await _taskService.CreateTask(taskDto);
        Console.WriteLine(result.IsSuccess ? Constants.TaskCreatedSuccess : result.Error);
    }

    private static async Task UpdateTask()
    {
        Console.Write(Constants.EnterTaskId);
        if (!int.TryParse(Console.ReadLine(), out var taskId)) return;

        Console.Write(Constants.EnterNewTaskName);
        var name = Console.ReadLine();
        if (string.IsNullOrEmpty(name)) return;

        Console.Write(Constants.EnterNewTaskDescription);
        var description = Console.ReadLine();

        var taskDto = new TaskDto { Name = name, Description = description };
        var result = await _taskService.UpdateTask(taskId, taskDto);
        Console.WriteLine(result.IsSuccess ? Constants.TaskUpdatedSuccess : result.Error);
    }

    private static async Task DeleteTask()
    {
        Console.Write(Constants.EnterTaskId);
        if (!int.TryParse(Console.ReadLine(), out var taskId)) return;

        var result = await _taskService.DeleteTask(taskId);
        Console.WriteLine(result.IsSuccess ? Constants.TaskDeletedSuccess : result.Error);
    }

    private static async Task GetAllTasks()
    {
        var result = await _taskService.GetAllTasks();
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
            return;
        }

        foreach (var task in result.Data!)
        {
            Console.WriteLine(string.Format(Constants.TaskFormat, task.Id, task.Name, task.Description, task.IsActive));
        }
    }

    private static async Task GetAllUsers()
    {
        var result = await _userServices.GetAllUsers();
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
            return;
        }

        foreach (var user in result.Data!)
        {
            Console.WriteLine(string.Format(Constants.UserFormat, user.Id, user.Name, user.Role));
        }
    }

    private static async Task GetAllDevelopers()
    {
        var result = await _userServices.GetAllDevelopers();
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
            return;
        }

        foreach (var developer in result.Data!)
        {
            Console.WriteLine(string.Format(Constants.UserFormat, developer.Id, developer.Name, developer.Role));
        }
    }

    private static async Task AssignTaskToDev()
    {
        Console.Write(Constants.EnterDevId);
        if (!int.TryParse(Console.ReadLine(), out var devId)) return;

        Console.Write(Constants.EnterTaskId);
        if (!int.TryParse(Console.ReadLine(), out var taskId)) return;

        var result = await _userTaskService.AssignTaskToUser(devId, taskId);
        Console.WriteLine(result.IsSuccess ? Constants.TaskAssignedSuccess : result.Error);
    }

    private static async Task GetMyTasks()
    {
        if (_context.CurrentUserId == null) return;

        var result = await _userTaskService.GetUserAllTasks(_context.CurrentUserId.Value);
        if (!result.IsSuccess)
        {
            Console.WriteLine(result.Error);
            return;
        }

        foreach (var task in result.Data!)
        {
            Console.WriteLine(string.Format(Constants.TaskFormat, task.Id, task.Name, task.Description, task.IsActive));
        }
    }

    private static async Task CompleteTask()
    {
        Console.Write(Constants.EnterTaskId);
        if (!int.TryParse(Console.ReadLine(), out var taskId)) return;

        await _userTaskService.CompleteTask(taskId);
        Console.WriteLine(Constants.TaskCompleted);
    }

    private static async Task CompleteManyTasks()
    {
        if (_context.CurrentUserId == null) return;

        var tasks = await _userTaskService.GetUserAllTasks(_context.CurrentUserId.Value);
        if (!tasks.IsSuccess || tasks.Data == null || tasks.Data.Count == 0)
        {
            Console.WriteLine(Constants.NoActiveTasks);
            return;
        }

        var activeTasks = tasks.Data.Where(t => t.IsActive).ToList();
        if (!activeTasks.Any())
        {
            Console.WriteLine(Constants.NoActiveTasks);
            return;
        }

        Console.WriteLine(Constants.ParallelTasksExecution);
        var completionTasks = activeTasks.Select(task => Task.Run(async () =>
        {
            await _userTaskService.CompleteTask(task.Id);
            Console.WriteLine($"Task {task.Id} ended");
        }));

        await Task.WhenAll(completionTasks);
        Console.WriteLine(Constants.AllTasksCompleted);
    }
}