using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intents.Models
{
    public class Response<TData>
    {
        public const string SUCCESS = "Success";
        public const string ERROR = "Error";
        public TData Result { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }
        public string Version
        {
            get
            {
                return "1.0.0";
            }
        }

        public string dateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }
        public Response()
        {
            this.ErrorMessage = "Success";
        }

        public static Response<TData> Success(TData data, string message = null)
        {
            return new Response<TData>()
            {
                Result = data,
                ErrorMessage = message ?? SUCCESS,
                StatusCode = 1
            };
        }

        public static Response<TData> Error(TData data = default(TData), string message = null)
        {
            return new Response<TData>()
            {
                Result = data,
                ErrorMessage = message ?? ERROR,
                StatusCode = 2
            };
        }
    }

    public class Response : Response<object>
    {
        public static Response<string> GetDefaultErrorMessage(string customErrorMessage = null)
        {
            return new Response<string>()
            {
                ErrorMessage = customErrorMessage ?? "Error",
                StatusCode = 2
            };
        }

        public static Response<string> GetDefaultSuccessMessage()
        {
            return new Response<string>()
            {
                StatusCode = 1
            };
        }
    }
}
