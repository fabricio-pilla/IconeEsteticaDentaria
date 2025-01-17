using IconeEsteticaDentaria.CacheManager;
using IconeEsteticaDentaria.Comum.Validator;
using IconeEsteticaDentaria.Data.Repositorios;
using IconeEsteticaDentaria.Web.Authentication;
using IconeEsteticaDentaria.Web.Util;
using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace IconeEsteticaDentaria.Web.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public JsonResult Logon(string login, string senha)
        {
            try
            {
                if (!Validator.StringValida(login))
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Campo login obrigatório!");
                else if (login.Length > 20)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Campo login ultrapassou o limite de caracteres!");
                else if (string.IsNullOrEmpty(senha))
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Campo senha obrigatório!");
                else if (senha.Length > 20)
                    return new HttpResultJson(HttpStatusCode.BadRequest, "Campo senha ultrapassou o limite de caracteres!");
                else
                {
                    var usuario = new UsuarioRepositorio().AutenticarUsuario(login, senha);
                    if (usuario != null)
                    {
                        //Limpa todos os caches do usuário.
                        new MemoryCacheManager().RemoveByPattern(usuario.Id.ToString());

                        _UsuarioLogado = usuario;

                        new AuthenticationForms().DoAuthentication(usuario.Id, usuario.Nome);

                        return new HttpResultJson(HttpStatusCode.OK, "", usuario);
                    }
                    else
                    {
                        return new HttpResultJson(HttpStatusCode.NotFound, "Login/Senha inválido(s)!");
                    }
                }
            }
            catch (Exception)
            {
                return new HttpResultJson(HttpStatusCode.BadRequest, "Ocorreu um problema ao logar no sistema!");
            }
        }

        public ActionResult LogOff()
        {
            if (_UsuarioLogado != null)
                new MemoryCacheManager().RemoveByPattern(_UsuarioLogado.Id.ToString());

            FormsAuthentication.SignOut();
            Session.Abandon();
            foreach (System.Collections.DictionaryEntry entry in System.Web.HttpContext.Current.Cache)
            {
                System.Web.HttpContext.Current.Cache.Remove((string)entry.Key);
            }
            return Redirect("/Account/Login");
        }
    }
}