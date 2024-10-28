using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdoptionHub.Filters
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public RoleAuthorizeAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetString("userRole");

            if (string.IsNullOrEmpty(userRole) || !userRole.Equals(_requiredRole))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
