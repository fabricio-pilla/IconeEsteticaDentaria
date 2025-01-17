using System.Web.Optimization;

namespace IconeEsteticaDentaria.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            #region ESTILOS
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include("~/Content/Estilos/site.css", new CssRewriteUrlTransformWrapper())
                .IncludeDirectory("~/Content", ".css"));
            #endregion

            #region PLUGINS

            #region Jquery
            bundles.Add(new ScriptBundle("~/bundles/js/jquery", "https://code.jquery.com/jquery-3.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/js/jquerymaskedinput", "https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.min.js"));
            #endregion

            #region Bootstrap
            bundles.Add(new StyleBundle("~/bundles/css/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/bootstrap", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"));
            #endregion

            #region Toastr
            bundles.Add(new StyleBundle("~/bundles/css/toastr", "//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/toastr", "//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"));
            #endregion

            #region Toastr
            bundles.Add(new StyleBundle("~/bundles/css/sweetalert", "https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/sweetalert", "https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"));
            #endregion

            #region JqueryDataTables
            bundles.Add(new StyleBundle("~/bundles/css/jquerydataTables", "//cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/jquerydataTables", "//cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"));
            #endregion

            #region Chosen
            bundles.Add(new StyleBundle("~/bundles/css/chosen", "https://cdnjs.cloudflare.com/ajax/libs/chosen/1.6.2/chosen.min.css"));
            bundles.Add(new StyleBundle("~/bundles/css/chosenBootstrap").Include("~/Content/Estilos/ChosenBootstrap.css"));
            bundles.Add(new ScriptBundle("~/bundles/js/chosen", "https://cdnjs.cloudflare.com/ajax/libs/chosen/1.6.2/chosen.jquery.min.js"));
            #endregion

            #endregion

            #region SHARED
            bundles.Add(new ScriptBundle("~/bundles/js/geral")
                                         .Include("~/Content/Scripts/geral-{version}.js")
                                         .Include("~/Content/Plugins/Scripts/modernizr-{version}.js")
                                         .IncludeDirectory("~/Content", ".js"));
            #endregion

            #region PAGINAS

            #region Account
            bundles.Add(new ScriptBundle("~/bundles/js/Account")
                                         .Include("~/Content/Scripts/Paginas/Account/account-{version}.js"));
            #endregion

            #region Ordem Serviço
            bundles.Add(new ScriptBundle("~/bundles/js/CadastrarOrdemServico")
                                         .Include("~/Content/Scripts/Paginas/OrdemServico/Cadastrar-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/ConsultarOrdemServico")
                                         .Include("~/Content/Scripts/Paginas/OrdemServico/Consultar-{version}.js"));
            #endregion

            #endregion

            // True = Quando for  minificar, False = quando for debugar, aí os arquivos css e js não são minificados
            BundleTable.EnableOptimizations = true;
        }

        public class CssRewriteUrlTransformWrapper : IItemTransform
        {
            public string Process(string includedVirtualPath, string input)
            {
                return new CssRewriteUrlTransform().Process("~" + System.Web.VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
            }
        }
    }
}
