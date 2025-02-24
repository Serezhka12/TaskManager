using System.Collections.Concurrent;
using System.Text.Json;
using TaskManager.Entities;

namespace TaskManager.Data;

public class DataStorage
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private readonly ConcurrentDictionary<int, List<int>> _userTasks = new();
    private readonly ConcurrentDictionary<int, UserTask> _tasks = [];

    private int _lastUser;
    private int _lastTask;

    public ConcurrentDictionary<int, User> GetUsers() => _users;

    public ConcurrentDictionary<int, UserTask> GetTasks() => _tasks;
    public ConcurrentDictionary<int, List<int>> GetUserTasks() => _userTasks;

    public int GetUserId()
    {
        _lastUser += 1;
        return _lastUser;
    }
    public int GetTaskId()
    {
        _lastTask += 1;
        return _lastTask;
    }

    private static readonly string UsersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
    private static readonly string TasksFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");
    private static readonly string UserTasksFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_tasks.json");

    public void SaveToFile()
    {
        try
        {
            var serializerOptions = new JsonSerializerOptions { WriteIndented = true };

            var usersJson = JsonSerializer.Serialize(_users, serializerOptions);
            File.WriteAllText(UsersFilePath, usersJson);

            var userTasksJson = JsonSerializer.Serialize(_userTasks, serializerOptions);
            File.WriteAllText(UserTasksFilePath, userTasksJson);

            var tasksJson = JsonSerializer.Serialize(_tasks, serializerOptions);
            File.WriteAllText(TasksFilePath, tasksJson);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка під час збереження файлів: {ex.Message}");
        }
    }

    private void UploadFromFile()
    {
        try
        {
            UploadUsers();
            UploadUserTasks();
            UploadTasks();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка під час завантаження файлів: {ex.Message}");
        }
    }

    private void UploadUsers()
    {
        if (File.Exists(UsersFilePath))
        {
            var usersJson = File.ReadAllText(UsersFilePath);
            var usersData = JsonSerializer.Deserialize<ConcurrentDictionary<int, User>>(usersJson);
            if (usersData == null) return;
            foreach (var kvp in usersData)
            {
                _users[kvp.Key] = kvp.Value;
                if (kvp.Key > _lastUser)
                {
                    _lastUser = kvp.Key;
                }
            }
        }
        else
        {
            File.WriteAllText(UsersFilePath, "{}");
        }
    }

    private void UploadUserTasks()
    {
        if (File.Exists(UserTasksFilePath))
        {
            var tasksJson = File.ReadAllText(UserTasksFilePath);
            var tasksData = JsonSerializer.Deserialize<ConcurrentDictionary<int, List<int>>>(tasksJson);
            if (tasksData == null) return;
            foreach (var kvp in tasksData)
            {
                _userTasks[kvp.Key] = kvp.Value;
            }
        }
        else
        {
            File.WriteAllText(UserTasksFilePath, "{}");
        }
    }

    private void UploadTasks()
    {
        if (File.Exists(TasksFilePath))
        {
            var tasksList = JsonSerializer.Deserialize<ConcurrentDictionary<int, UserTask>>(File.ReadAllText(TasksFilePath));
            if (tasksList == null) return;
            foreach (var kvp in tasksList)
            {
                _tasks[kvp.Key] = kvp.Value;
                if (kvp.Key > _lastTask)
                {
                    _lastTask = kvp.Key;
                }
            }
        }
        else
        {
            File.WriteAllText(TasksFilePath, "[]");
        }
    }

    private static DataStorage? _instance;

    private DataStorage()
    {
        UploadFromFile();
    }

    public static DataStorage GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataStorage();
        }

        return _instance;
    }
}