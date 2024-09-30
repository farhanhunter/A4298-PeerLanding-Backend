using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO.Res
{
    public class ResBaseDto<T>
    {
        public bool Success { get; set; }
        public object Message { get; set; }
        public T Data { get; set; }

        public int StatusCode { get; set; }

        public ResBaseDto()
        {
            Success = true;
            Message = string.Empty;
            Data = default;
            StatusCode = 200;
        }

        public ResBaseDto(bool success, object message, T data, int statusCode)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }

        public void SetMessage(string message)
        {
            Message = message;
        }

        public void SetMessage(Dictionary<string, List<string>> messages)
        {
            Message = messages;
        }

        public void SetMessage(IEnumerable<string> messages)
        {
            Message = messages;
        }
        public void SetData(T data)
        {
            Data = data;
        }   
    }
}
