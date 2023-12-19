using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taogar.HTTP.Domain.Models.Errors
{
    public class BaseHTTPError : Exception
    {
        public BaseHTTPError(int code, string message) : base(message)
        {
            StatusCode = code;
            Message = message;
        }

        public int StatusCode { get; set; }
        public new string Message { get; set; }
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { StatusCode = this.StatusCode, Message = this.Message });
        }

        public virtual string ToLog()
        {
            return $"Warning: {StatusCode}, {Message}";
        }
    }
}
