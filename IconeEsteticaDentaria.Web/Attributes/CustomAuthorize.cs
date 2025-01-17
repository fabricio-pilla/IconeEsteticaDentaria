using System.Web.Mvc;

namespace IconeEsteticaDentaria.Web.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
                filterContext.Result = new HttpUnauthorizedResult();
           
        }
    }
}