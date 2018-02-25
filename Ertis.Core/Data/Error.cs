using System;
namespace Ertis.Core.Data
{
    public class Error
    {
        public int ErrorCode { get; private set; }

        public string ErrorMessage { get; private set; }

        private Error(int code, string message)
        {
            this.ErrorCode = code;
            this.ErrorMessage = message;
        }

        internal static Error New(int code, string message)
        {
            return new Error(code, message);
        }
    }
}
