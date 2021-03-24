using DepsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DepsWebApp.Filters
{
    /// <summary>
    /// Exception Filter attribute
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute, IAsyncExceptionFilter
    {
        /// <summary>
        /// Exception handling method.
        /// </summary>
        /// <param name="context">Exception Context.</param>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            int code = GetExceptionCode(context.Exception);
            context.Result = new JsonResult(new Error { Code = code, Message = context.Exception.Message });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Method that gets custom exception code.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public int GetExceptionCode(Exception ex)
        {
            var exception = ex.GetType();
            return exception.Equals(typeof(NotImplementedException)) ? 1 :
                  (exception.Equals(typeof(NullReferenceException)) ? 2 :
                  (exception.Equals(typeof(ArgumentException)) ? 3 :
                  (exception.Equals(typeof(FileNotFoundException)) ? 4 :
                  (exception.Equals(typeof(ArgumentOutOfRangeException)) ? 5 :
                  (exception.Equals(typeof(JsonException)) ? 6 : 0
                  )))));
        }
    }
}