namespace ExpenseTracker.Domain.Common;

public class Result<T>
{
    public T? Value { get; private set; }
    public int StatusCode { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? Error { get; private set; }

    public static Result<T> Success(T value)
        => new() { IsSuccess = true, Value = value };

    public static Result<T> Failure(string error, int statuscode = 400)
        => new() { IsSuccess = false, StatusCode = statuscode, Error = error };
}