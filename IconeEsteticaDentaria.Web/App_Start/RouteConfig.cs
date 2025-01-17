using System.Web.Mvc;
using System.Web.Routing;

namespace IconeEsteticaDentaria.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute("Sair",
                "sair",
                new { controller = "Account", action = "LogOff" });

            #region Ordem Serviço
            routes.MapRoute("CadastrarOrdemServico",
                "cadastrarordemservico",
                new { controller = "OrdemServico", action = "Cadastrar" });

            routes.MapRoute("EditarOrdemServico",
                "editarordemservico/{id}",
                new { controller = "OrdemServico", action = "Editar" });

            routes.MapRoute("ConsultarOrdemServico",
                "consultarordemservico",
                new { controller = "OrdemServico", action = "Consultar" });

            routes.MapRoute("Inicializar",
                "inicializar",
                new { controller = "OrdemServico", action = "Inicializar" });

            routes.MapRoute("SalvarOrdemServico",
              "salvarordemservico",
              new { controller = "OrdemServico", action = "Salvar" });

            routes.MapRoute("PesquisarOrdemServico",
              "pesquisarordemservico",
              new { controller = "OrdemServico", action = "Pesquisar" });

            routes.MapRoute("CarregarDadosEditar",
              "carregardadoseditarordemservico/{numeroOS}",
              new { controller = "OrdemServico", action = "CarregarDadosEditar" });

            #endregion

            routes.MapRoute(
             name: "Default",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );


        }
    }
}
