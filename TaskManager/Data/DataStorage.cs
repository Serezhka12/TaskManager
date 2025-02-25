using System.Text.Json;
using TaskManager.Entities;

namespace TaskManager.Data;

public class DataStorage
{
    private static readonly string UsersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.json");
    private static readonly string TasksFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasks.json");
    private static readonly string UserTasksFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_tasks.json");
    private static readonly string CountersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "counters.json");

    private static readonly JsonSerializerOptions SerializerOptions = new() { WriteIndented = true };

    private readonly SemaphoreSlim _usersSemaphore = new(1, 1);
    private readonly SemaphoreSlim _tasksSemaphore = new(1, 1);
    private readonly SemaphoreSlim _userTasksSemaphore = new(1, 1);
    private readonly SemaphoreSlim _countersSemaphore = new(1, 1);

    public async Task<Dictionary<int, User>> GetUsers()
    {
        await _usersSemaphore.WaitAsync();
        try
        {
            if (!File.Exists(UsersFilePath))
            {
                await File.WriteAllTextAsync(UsersFilePath, "{}");
                return new Dictionary<int, User>();
            }

            var json = await File.ReadAllTextAsync(UsersFilePath);
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                return new Dictionary<int, User>();
            }

            var users = JsonSerializer.Deserialize<Dictionary<int, User>>(json);
            return users ?? new Dictionary<int, User>();
        }
        finally
        {
            _usersSemaphore.Release();
        }
    }

    public async Task<Dictionary<int, UserTask>> GetTasks()
    {
        await _tasksSemaphore.WaitAsync();
        try
        {
            if (!File.Exists(TasksFilePath))
            {
                await File.WriteAllTextAsync(TasksFilePath, "{}");
                return new Dictionary<int, UserTask>();
            }

            var json = await File.ReadAllTextAsync(TasksFilePath);
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                return new Dictionary<int, UserTask>();
            }

            var tasks = JsonSerializer.Deserialize<Dictionary<int, UserTask>>(json);
            return tasks ?? new Dictionary<int, UserTask>();
        }
        finally
        {
            _tasksSemaphore.Release();
        }
    }

    public async Task<Dictionary<int, List<int>>> GetUserTasks()
    {
        await _userTasksSemaphore.WaitAsync();
        try
        {
            if (!File.Exists(UserTasksFilePath))
            {
                await File.WriteAllTextAsync(UserTasksFilePath, "{}");
                return new Dictionary<int, List<int>>();
            }

            var json = await File.ReadAllTextAsync(UserTasksFilePath);
            if (string.IsNullOrEmpty(json) || json == "{}")
            {
                return new Dictionary<int, List<int>>();
            }

            var userTasks = JsonSerializer.Deserialize<Dictionary<int, List<int>>>(json);
            return userTasks ?? new Dictionary<int, List<int>>();
        }
        finally
        {
            _userTasksSemaphore.Release();
        }
    }

    public async Task<int> GetUserId()
    {
        await _countersSemaphore.WaitAsync();
        try
        {
            var counters = await GetCounters();
            counters.LastUserId++;
            await SaveCounters(counters);
            return counters.LastUserId;
        }
        finally
        {
            _countersSemaphore.Release();
        }
    }

    public async Task<int> GetTaskId()
    {
        await _countersSemaphore.WaitAsync();
        try
        {
            var counters = await GetCounters();
            counters.LastTaskId++;
            await SaveCounters(counters);
            return counters.LastTaskId;
        }
        finally
        {
            _countersSemaphore.Release();
        }
    }

    public async Task SaveUsers(Dictionary<int, User> users)
    {
        await _usersSemaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(users, SerializerOptions);
            await File.WriteAllTextAsync(UsersFilePath, json);
        }
        finally
        {
            _usersSemaphore.Release();
        }
    }

    public async Task SaveTasks(Dictionary<int, UserTask> tasks)
    {
        await _tasksSemaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(tasks, SerializerOptions);
            await File.WriteAllTextAsync(TasksFilePath, json);
        }
        finally
        {
            _tasksSemaphore.Release();
        }
    }

    public async Task SaveUserTasks(Dictionary<int, List<int>> userTasks)
    {
        await _userTasksSemaphore.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(userTasks, SerializerOptions);
            await File.WriteAllTextAsync(UserTasksFilePath, json);
        }
        finally
        {
            _userTasksSemaphore.Release();
        }
    }

    private class Counters
    {
        public int LastUserId { get; set; }
        public int LastTaskId { get; set; }
    }

    private async Task<Counters> GetCounters()
    {
        if (!File.Exists(CountersFilePath))
        {
            var counters = new Counters { LastUserId = 0, LastTaskId = 0 };
            var json = JsonSerializer.Serialize(counters, SerializerOptions);
            await File.WriteAllTextAsync(CountersFilePath, json);
            return counters;
        }

        var existingJson = await File.ReadAllTextAsync(CountersFilePath);
        return JsonSerializer.Deserialize<Counters>(existingJson) ?? new Counters();
    }

    private async Task SaveCounters(Counters counters)
    {
        var json = JsonSerializer.Serialize(counters, SerializerOptions);
        await File.WriteAllTextAsync(CountersFilePath, json);
    }

    private static DataStorage? _instance;
    private DataStorage() { }

    public static DataStorage GetInstance()
    {
        if (_instance == null)
        {
            _instance = new DataStorage();
        }

        return _instance;
    }
}