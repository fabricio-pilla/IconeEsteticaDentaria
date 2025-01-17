
namespace IconeEsteticaDentaria.Data.Repositorios
{
    public class PessoaRepositorio: BaseRepositorio
    {
        //public IEnumerable<SelectGenericDto> RetornaClientesOrdemServico()
        //{
        //    try
        //    {
        //        using (Cnn)
        //        {
        //            SqlCommand cmd = new SqlCommand
        //            {
        //                CommandText = "proc_Web_RetornaClientes",
        //                CommandType = CommandType.StoredProcedure
        //            };

        //            SqlDataReader dr = Cnn.LeDataReader(cmd);

        //            var clientes = new List<SelectGenericDto>();
        //            while (dr.Read())
        //            {
        //                clientes.Add(new SelectGenericDto
        //                {
        //                    Id = dr["PESSOAID"].ToInt(),
        //                    Descricao = dr["NOME"].ToString()
        //                });
        //            }

        //            return clientes;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Cnn.FechaConexao();
        //    }
        //}
    }
}
