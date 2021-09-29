using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos
{
    public class Response<T>
    {
        public T Data { get; set; }
        private bool Success { get; set; }
        private int StatusCode { get; set; }
        private string Message { get; set; }
        public Response(bool success, int statusCode, string message, T data)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
        public static Response<T> CreateSuccessResponse(T data)
        {
            return new Response<T>(true, 201, "success !", data);
        }
        public static Response<T> CreateErrorResponse(Exception exception = null)
        {
            return new Response<T>(false, 500, exception.ToString(), default(T));
        }
    }
}
