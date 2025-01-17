using System.Configuration;
using System.Data.SqlClient;

namespace IconeEsteticaDentaria.Data.DAO
{
    /// <summary>
    /// Possui informacao e estrutura basica a todos os repositorios
    /// </summary>
    public abstract class DAOBase
    {
        //Estabele uma sqlconnection
        protected SqlConnection CriarConexao()
        {
            return new SqlConnection
            {
                //Recupera valor da connectionstring definida na web.config
                ConnectionString = ConfigurationManager.ConnectionStrings["iconeesteticaambiental"].ConnectionString
            };
        }
    }
}
