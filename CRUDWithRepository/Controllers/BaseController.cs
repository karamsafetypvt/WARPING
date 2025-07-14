using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWithRepository.UI.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var UserID = HttpContext.Session.GetString("UserID");
            if (string.IsNullOrEmpty(UserID))
            {
                context.Result = RedirectToAction("Index", "Account");
            }

            base.OnActionExecuting(context);
        }
    }
}
