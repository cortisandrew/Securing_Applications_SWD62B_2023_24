using Microsoft.AspNetCore.Mvc.Filters;

namespace Securing_Applications_SWD62B_2023_24.Helpers
{
    public class AuthorizeFileAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // perform checks and make decisions
            base.OnActionExecuted(context);
        }
    }
}
