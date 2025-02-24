namespace TaskManager;

public class OperationResult<T>
{
    public bool IsSuccess { get; set; }

    public T? Data { get; set; }

    public string? Error { get; set; }

    private OperationResult(T data)
    {
        IsSuccess = true;
        Data = data;
        Error = string.Empty;
    }

    private OperationResult(string error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static OperationResult<T> Success(T data) => new OperationResult<T>(data);

    public static OperationResult<T> Failure(string error) => new OperationResult<T>(error);
}