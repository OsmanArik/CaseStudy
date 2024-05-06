using Shared.Core.Constants;
using System.Net;

namespace Shared.Core.Utilies.Results
{
    public class ResultModel : ResultModel<string>, IResultModel
    {
        public ResultModel() : base() { }

        public ResultModel(string data)
        {
            Data = data;
        }

        public ResultModel(bool success) : base(success)
        {
        }
        public ResultModel(bool success, string message) : base(success, message)
        {
        }
        public ResultModel(bool success, string message, string data) : base(success, message, data)
        {
        }
    }

    public class ResultModel<T> : IResult<T>
    {
        public ResultModel() { }

        public ResultModel(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public ResultModel(T data)
        {
            Data = data;
        }

        public ResultModel(bool success, string message) : this(success)
        {
            Message = message;
        }
        public ResultModel(bool success, string message, T data) : this(success, message)
        {
            Data = data;
        }

        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = ResultMessages.Successful;
        public IEnumerable<string> Errors { get; set; }
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
    }
}