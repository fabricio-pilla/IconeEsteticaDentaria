using IconeEsteticaDentaria.CacheManager;
using IconeEsteticaDentaria.Domain;
using System.Web.Mvc;
using System.Web.Security;

namespace IconeEsteticaDentaria.Web.Controllers
{
    public class BaseController : Controller
    {
        #region Objetos Cache
        Usuario usuarioLogado;
        protected Usuario _UsuarioLogado
        {
            get
            {
                int IdUsuario = RetornaIdUsuarioCookie();
                if (IdUsuario <= 0)
                {
                    Response.Redirect("/Account/LogOff", false);
                    return usuarioLogado;
                }
                var Cache = new CacheService().ReturnObjectByCache<Usuario>(IdUsuario, CacheKeys.USUARIO_LOGADO_USUARIOKEY, new Usuario());
                usuarioLogado = Cache;
                if (Cache.Id <= 0)
                {
                    Response.Redirect("/Account/LogOff", false);
                    FormsAuthentication.SignOut();
                }
                return usuarioLogado;
            }
            set
            {
                usuarioLogado = value;
                new CacheService().ReturnObjectByCache<Usuario>(usuarioLogado.Id, CacheKeys.USUARIO_LOGADO_USUARIOKEY, usuarioLogado, false);

            }
        }
        #endregion

        protected int RetornaIdUsuarioCookie()
        {
            FormsIdentity fIdent = System.Web.HttpContext.Current.User.Identity as FormsIdentity;
            int id = 0;

            if (fIdent != null && fIdent.Ticket != null && fIdent.Ticket.UserData != null)
                int.TryParse(fIdent.Ticket.UserData, out id);

            return id;
        }
    }
}