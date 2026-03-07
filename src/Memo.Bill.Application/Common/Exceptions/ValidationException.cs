using FluentValidation.Results;

namespace Memo.Bill.Application.Common.Exceptions;
public class ValidationException : Exception
{
    public ValidationException()
       : base("一个或多个属性验证失败。")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
