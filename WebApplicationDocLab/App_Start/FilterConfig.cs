using System.Web;
using System.Web.Mvc;
using System;

namespace WebApplicationDocLab
{
    public sealed class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string _requiredRole;

        public SessionAuthorizeAttribute(string requiredRole)
        {
            _requiredRole = requiredRole;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            var userId = session?["UserId"];
            var role = Convert.ToString(session?["UserType"]);
            var status = Convert.ToString(session?["Status"]);

            if (userId == null ||
                !string.Equals(role, _requiredRole, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Login"
                    }));
            }
        }
    }

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
