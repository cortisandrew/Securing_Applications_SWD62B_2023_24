using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Securing_Applications_SWD62B_2023_24.Helpers
{
    public class CustomActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            Log($"User with name {context.HttpContext.User.Identity.Name} has requested an action:");

            Log($"ActionName: {context.ActionDescriptor.DisplayName}");

            foreach (string argumentKey in context.ActionArguments.Keys)
            {
                object? returnValue = null;
                bool success = context.ActionArguments.TryGetValue(argumentKey, out returnValue);
                
                if (success)
                {
                    string stringResult = "NULL";
                    if (returnValue != null)
                    {
                        stringResult = returnValue.ToString();
                    }
                    Log($"Argument Key: {argumentKey}, Argument Value: {stringResult}");
                }
            }

            context.Result = new RedirectToActionResult("Unauthorized", "File", "123");

            // context.Result = 
            base.OnActionExecuting(context);



        }
        /*
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Log($"ActionName: {actionContext.ActionDescriptor.ActionName}");

            foreach (string argumentKey in actionContext.ActionArguments.Keys)
            {
                Log($"Argument Key: {argumentKey}, Argument Value: {actionContext.ActionArguments[argumentKey].ToString()}");
            }
                
            base.OnActionExecuting(actionContext);
        }*/


        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
