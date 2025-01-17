using System;
using System.Web;
using System.Web.Security;

namespace IconeEsteticaDentaria.Web.Authentication
{
    public class AuthenticationForms
    {
        public bool DoAuthentication(int usuarioId, string usuarioNome)
        {
            try
            {
                var now = DateTime.Now.ToLocalTime();
                var ticketLogin = new FormsAuthenticationTicket(
                    1,
                    usuarioNome,
                    now,
                    now.Add(FormsAuthentication.Timeout),
                    false,
                    usuarioId.ToString(),
                    FormsAuthentication.FormsCookiePath);
                var encryptedLoginTicket = FormsAuthentication.Encrypt(ticketLogin);

                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedLoginTicket)
                {
                    HttpOnly = true,
                    Expires = now.Add(FormsAuthentication.Timeout),
                    Secure = FormsAuthentication.RequireSSL,
                    Path = FormsAuthentication.FormsCookiePath,
                    Domain = FormsAuthentication.CookieDomain,
                };
                authCookie.Value = FormsAuthentication.Encrypt(ticketLogin);
                HttpContext.Current.Response.Cookies.Add(authCookie);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }
    }
}