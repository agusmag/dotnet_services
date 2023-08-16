using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAuthors.Filters
{
	public class MyActionFilter : IActionFilter
	{
        private readonly ILogger<MyActionFilter> logger;

        public MyActionFilter(ILogger<MyActionFilter> logger)
		{
            this.logger = logger;
        }

        // Executed before the Action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Before executing the action");
        }

        // Executed After the Action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("After executing the action");
        }

    }
}

