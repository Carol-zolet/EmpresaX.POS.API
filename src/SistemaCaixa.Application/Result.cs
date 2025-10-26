namespace SistemaCaixa.Application;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public bool IsNotFound { get; }
    public string Error { get; }
    public T Data { get; }

    protected Result(bool isSuccess, T data, string error, bool isNotFound = false)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        IsNotFound = isNotFound;
    }

    public static Result<T> Success(T data) => new(true, data, string.Empty);
    public static Result<T> Failure(string error) => new(false, default!, error);
    public static Result<T> NotFound(string error) => new(false, default!, error, true);
}
