using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class TaskService
{
    private readonly TaskRepository _tasksRepository = TaskRepository.GetInstance();

    public OperationResult<bool> CreateTask(TaskDto taskDto)
    {
        var task = new UserTask(taskDto.Name);
        if (taskDto.Description != null)
        {
            task.Description = taskDto.Description;
        }

        if (_tasksRepository.CreateTask(task))
        {
            return OperationResult<bool>.Success(true);
        }
        return OperationResult<bool>.Failure(ErrorMessages.CanNotCreateTask());
    }

    public OperationResult<bool> UpdateTask(int taskId, TaskDto taskDto)
    {
        var task = _tasksRepository.GetTaskById(taskId);
        if (task == null) return OperationResult<bool>.Failure(ErrorMessages.CanNotUpdateTask());
        _tasksRepository.UpdateTask(taskId, taskDto);
        return OperationResult<bool>.Success(true);
    }

    public OperationResult<bool> DeleteTask(int taskId)
    {
        var result = _tasksRepository.DeleteTask(taskId);
        return result ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotDeleteTask());
    }

    public OperationResult<List<UserTask>> GetAllTasks()
    {
        return OperationResult<List<UserTask>>.Success(_tasksRepository.GetAllTasks());
    }

    public OperationResult<UserTask?> GetTasksById(int taskId)
    {
        return OperationResult<UserTask?>.Success(_tasksRepository.GetTaskById(taskId));
    }
}