using Loans.AppServices.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Loans.Host;

internal class ApplicationServicesExceptionsFilter: IActionFilter, IOrderedFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not ServicesException)
        {
            return;
        }

        context.Result = new BadRequestObjectResult(context.Exception.Message);
        context.ExceptionHandled = true;
    }

    public int Order => int.MaxValue - 10;
}