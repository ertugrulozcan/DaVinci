using System;
using System.Net;

namespace Ertis.WebService.Models
{
    public class HttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; private set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public T Data { get; set; }

        /// <summary>
        /// Constructor 1
        /// </summary>
        public HttpResponse(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor 2
        /// </summary>
        public HttpResponse(HttpStatusCode statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        /// <summary>
        /// Constructor 3
        /// </summary>
        public HttpResponse(HttpStatusCode statusCode, T data)
        {
            this.StatusCode = statusCode;
            this.Data = data;
        }

        /// <summary>
        /// Constructor 4
        /// </summary>
        public HttpResponse(HttpStatusCode statusCode, string message, T data)
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Data = data;
        }
    }
}
