using IconeEsteticaDentaria.Web.Attributes;
using System.Web.Mvc;

namespace IconeEsteticaDentaria.Web.Controllers
{
    [CustomAuthorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(_UsuarioLogado);
        }       
    }
}