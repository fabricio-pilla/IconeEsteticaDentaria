using IconeEsteticaDentaria.Comum.Extensions;
using IconeEsteticaDentaria.Domain;
using IconeEsteticaDentaria.Domain.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IconeEsteticaDentaria.Data.Repositorios
{
    public class ServicoRepositorio : BaseRepositorio
    {
        public IEnumerable<SelectGenericDto> RetornaServicosOrdemServico()
        {
            try
            {
                using (Cnn)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "proc_Web_RetornaServicos",
                        CommandType = CommandType.StoredProcedure
                    };

                    SqlDataReader dr = Cnn.LeDataReader(cmd);

                    var servicos = new List<SelectGenericDto>();
                    while (dr.Read())
                    {
                        servicos.Add(new SelectGenericDto
                        {
                            Id = dr["SERVICOID"].ToInt(),
                            Descricao = dr["DESCRICAO"].ToString()
                        });
                    }

                    return servicos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cnn.FechaConexao();
            }
        }

        public IEnumerable<Servico> RetornaServicosAdicionadosOrdemServico()
        {
            SqlDataReader dr = null;
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "proc_Web_RetornaServicos",
                    CommandType = CommandType.StoredProcedure
                };

                dr = Cnn.LeDataReader(cmd);

                var servicos = new List<Servico>();
                while (dr.Read())
                {
                    servicos.Add(new Servico
                    {
                        Id = dr["SERVICOID"].ToInt(),
                        Valor = dr["VALOR"].ToDecimal()
                    });
                }

                return servicos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }
    }
}
