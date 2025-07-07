using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UXComex.Application.Exceptions;
using UXComex.Domain.DTOs.Shared;

namespace UXComex.API.Filters;

public class AppExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException)
        {
            var exception = context.Exception as ValidationException;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new ObjectResult(new ApiResponse<object>(false, new List<string>
            {
                exception.Message
            }));
        }

        if (context.Exception is DomainException)
        {
            var exception = context.Exception as DomainException;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new ObjectResult(new ApiResponse<object>(false, new List<string>
            {
                exception.Message
            }));
        }

        if (context.Exception is NotFoundException)
        {
            var exception = context.Exception as NotFoundException;
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Result = new ObjectResult(new ApiResponse<object>(false, new List<string>
            {
                exception.Message
            }));
        }
    }
}
