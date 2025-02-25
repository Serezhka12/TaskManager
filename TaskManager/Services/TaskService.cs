using TaskManager.Data;
using TaskManager.Dto;
using TaskManager.Entities;
using TaskManager.Interfaces;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class TaskService: ITaskService
{
    private readonly ITaskRepository _tasksRepository = TaskRepository.GetInstance();

    public async Task<OperationResult<bool>> CreateTask(TaskDto taskDto)
    {
        var task = new UserTask(taskDto.Name);
        if (taskDto.Description != null)
        {
            task.Description = taskDto.Description;
        }

        var result = await _tasksRepository.CreateTask(task);
        return result
            ? OperationResult<bool>.Success(true)
            : OperationResult<bool>.Failure(ErrorMessages.CanNotCreateTask());
    }

    public async Task<OperationResult<bool>> UpdateTask(int taskId, TaskDto taskDto)
    {
        var task = await _tasksRepository.GetTaskById(taskId);
        if (task == null) return OperationResult<bool>.Failure(ErrorMessages.CanNotUpdateTask());
        await _tasksRepository.UpdateTask(taskId, taskDto);
        return OperationResult<bool>.Success(true);
    }

    public async Task<OperationResult<bool>> DeleteTask(int taskId)
    {
        var result = await _tasksRepository.DeleteTask(taskId);
        return result ? OperationResult<bool>.Success(true) : OperationResult<bool>.Failure(ErrorMessages.CanNotDeleteTask());
    }

    public async Task<OperationResult<List<UserTask>>> GetAllTasks()
    {
        var tasks = await _tasksRepository.GetAllTasks();
        return OperationResult<List<UserTask>>.Success(tasks);
    }

    public async Task<OperationResult<UserTask?>> GetTasksById(int taskId)
    {
        var task = await _tasksRepository.GetTaskById(taskId);
        return OperationResult<UserTask?>.Success(task);
    }
}