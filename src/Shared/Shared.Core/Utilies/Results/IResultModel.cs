namespace Shared.Core.Utilies.Results
{
    public interface IResultModel : IResult<string> { }

    public interface IResult<T>
    {
        T Data { get; set; }

        bool IsSuccess { get; }
        string Message { get; }
    }
}