using System;
namespace Ertis.Core.Data
{
    public class Result
    {
        public bool Success { get; private set; }

        public int ResultCode { get; private set; }

        public string Message { get; private set; }

        public object Data { get; set; }

        public Error Error { get; set; }

        public Result(bool success, int resultCode, string message = null, object data = null)
        {
            this.Success = success;
            this.ResultCode = resultCode;
            this.Message = message;
            this.Data = data;
        }

        public Result(Error error, int resultCode)
        {
            this.Success = false;
            this.ResultCode = resultCode;
            this.Error = error;
        }
    }
}
