using System;

namespace PayMongo.Payment.Api.Application;

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }

    protected Result(bool isSuccess, string? message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success(string? message = null) => new Result(true, message);
    public static Result Failure(string? message) => new Result(false, message);
}

public class Result<T> : Result
{
    public T? Data { get; }

    private Result(bool isSuccess, T? data, string? message) : base(isSuccess, message)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string? message = null) => new Result<T>(true, data, message);
    public static new Result<T> Failure(string? message) => new Result<T>(false, default, message);
}