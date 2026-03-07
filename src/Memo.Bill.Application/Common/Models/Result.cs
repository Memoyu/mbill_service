using System.Text.Encodings.Web;
using System.Text.Json;
using Memo.Bill.Domain.Enums;

namespace Memo.Bill.Application.Common.Models;

public class Result
{
    public bool IsSuccess { get; }

    public ResultCode Code { get; set; }

    public string Message { get; }

    protected Result(ResultCode code, string msg)
    {
        var isSuccess = code == ResultCode.Success;
        if (!isSuccess && string.IsNullOrWhiteSpace(msg)) throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Code = code;
        Message = msg;
    }

    public static Result Success(string msg = "")
    {
        return new Result(ResultCode.Success, msg);
    }

    public static Result<T> Success<T>(T data, string msg = "")
    {
        return new Result<T>(data, ResultCode.Success, msg);
    }

    public static Result Failure(string msg, ResultCode code = ResultCode.Failure)
    {
        return new Result(code, msg);
    }

    public static Result<T> Failure<T>(string msg, ResultCode code = ResultCode.Failure)
    {
        return new Result<T>(default, code, msg);
    }

    public override string ToString()
    {
        return this.ToJson(new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}

public class Result<T> : Result
{
    protected internal Result(T? data, ResultCode code, string msg) : base(code, msg)
    {
        Data = data;
    }

    public T? Data { get; }
}
