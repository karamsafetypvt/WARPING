
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string _requiredRole;

    public SessionAuthorizeAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var userRole = context.HttpContext.Session.GetString("Role");

        if (string.IsNullOrEmpty(userRole) || userRole != _requiredRole)
        {
            context.Result = new RedirectToActionResult("Index", "Account", null);
        }

        base.OnActionExecuting(context);
    }
}
