using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Models
{
    public class APIResponse<T> where T : class
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }


        public APIResponse(T? data, string message)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            Errors = null;
        }

        public APIResponse(List<string>? errors, string message)
        {
            Succeeded = false;
            Message = message;
            Data = null;
            Errors = errors;
        }

        public static APIResponse<T> SuccessResponse(T? Data, string Message) => new APIResponse<T>(Data, Message);
        public static APIResponse<T> FailureResponse(List<string>? errors, string Message) => new APIResponse<T>(errors, Message);
    }
}
