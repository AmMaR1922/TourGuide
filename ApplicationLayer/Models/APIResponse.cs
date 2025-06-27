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
        public int StatusCode { get; set; }

        public static APIResponse<T> SuccessResponse(int StatusCode, T? Data, string Message) => new APIResponse<T>() { Data = Data, Message = Message, StatusCode = StatusCode, Succeeded = true};
        public static APIResponse<T> FailureResponse(int StatusCode, List<string>? errors, string Message) => new APIResponse<T>() { Errors = errors, Message = Message, StatusCode = StatusCode, Succeeded = false};
    }
}
