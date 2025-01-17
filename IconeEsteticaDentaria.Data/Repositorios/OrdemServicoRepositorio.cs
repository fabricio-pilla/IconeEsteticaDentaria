using IconeEsteticaDentaria.Comum.Extensions;
using IconeEsteticaDentaria.Comum.Helpers;
using IconeEsteticaDentaria.Comum.Validator;
using IconeEsteticaDentaria.Domain;
using IconeEsteticaDentaria.Domain.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace IconeEsteticaDentaria.Data.Repositorios
{
    public class OrdemServicoRepositorio : BaseRepositorio
    {
        public OrdermServicoInicializarDto Inicializar()
        {
            var retorno = new OrdermServicoInicializarDto
            {
                DataEntrada = DateTime.Now.ToShortDateString(),
                Servicos = new ServicoRepositorio().RetornaServicosOrdemServico()
            };

            return retorno;
        }

        public bool AdicionarEditar(OrdemServicoSalvarDto parametros)
        {
            var retorno = false;
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "proc_Web_AdicionarEditarOrdemServico",
                    CommandType = CommandType.StoredProcedure
                };

                Cnn.AbreTransacao();

                var numeroOS = parametros.NumeroOrdemServico > 0 ? parametros.NumeroOrdemServico : RetornaNumeroOrdemServico();
                if (numeroOS > 0)
                    cmd.Parameters.AddWithValue("@NumeroOS", numeroOS);
                else
                {
                    retorno = false;
                    Cnn.AbortaTransacao();
                }

                var itensOS = RetornaItensOrdemServico(parametros.ItensOrdemServico, numeroOS, parametros.IdCliente);

                cmd.Parameters.AddWithValue("@IdCliente", parametros.IdCliente);
                cmd.Parameters.AddWithValue("@CpfPaciente", parametros.CpfPaciente.GetOnlyNumbers());
                cmd.Parameters.AddWithValue("@NomePaciente", parametros.NomePaciente);
                cmd.Parameters.AddWithValue("@Observacao", parametros.Observacao);
                cmd.Parameters.AddWithValue("@ValorTotalOS", itensOS.Sum(x => x.ValorTotal));
                cmd.Parameters.AddWithValue("@ItensOrdemServicoXML", SerializacaoHelper.SerializarParaXml(itensOS));

                retorno = Cnn.ExecutaSql(cmd) > 0;

                if (retorno)
                    Cnn.FechaTransacao();
                else
                    Cnn.AbortaTransacao();
            }
            catch (Exception ex)
            {
                Cnn.AbortaTransacao();
                retorno = false;
                throw ex;
            }
            finally
            {
                Cnn.FechaConexao();
            }
            return retorno;
        }

        private int RetornaNumeroOrdemServico()
        {
            var numeroOS = 0;
            SqlDataReader dr = null;
            try
            {
                SqlCommand cmd = new SqlCommand
                {
                    CommandText = "proc_Web_RetornaNovoNumeroOS",
                    CommandType = CommandType.StoredProcedure
                };

                dr = Cnn.LeDataReader(cmd);

                if (dr.Read())
                {
                    numeroOS = dr["OS"].ToInt();
                }

                return numeroOS;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (dr != null)
                    dr.Close();
            }
        }

        private List<ItensOrdemServico> RetornaItensOrdemServico(IEnumerable<ItensOrdemServicoSalvarDto> itensDto, int numeroOS, int clientId)
        {
            var servicos = new ServicoRepositorio().RetornaServicosAdicionadosOrdemServico();

            var index = 1;

            var itensOS = new List<ItensOrdemServico>();

            foreach (var item in itensDto)
            {
                var valor = servicos.FirstOrDefault(x => x.Id == item.ServicoId).Valor;
                itensOS.Add(new ItensOrdemServico
                {
                    Servico = new Servico
                    {
                        Id = item.ServicoId,
                        Valor = valor
                    },
                    Item = index,
                    OrdemServicoId = numeroOS,
                    PessoaId = clientId,
                    Quantidade = item.Quantidade,
                    ValorTotal = (item.Quantidade * valor)
                });

                index++;
            }

            return itensOS;
        }

        public IEnumerable<OrdemServicoPesquisarDto> PesquisarOrdemServico(int numeroOS, string nomePaciente, int usuarioLogadoID)
        {
            try
            {
                using (Cnn)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "proc_Web_RetornaOrdemServico",
                        CommandType = CommandType.StoredProcedure
                    };

                    if (numeroOS > 0)
                        cmd.Parameters.AddWithValue("@NumeroOS", numeroOS);

                    if (Validator.StringValida(nomePaciente))
                        cmd.Parameters.AddWithValue("@NomePessoa", nomePaciente);

                    cmd.Parameters.AddWithValue("@PessoaID", usuarioLogadoID);

                    SqlDataReader dr = Cnn.LeDataReader(cmd);

                    var retorno = new List<OrdemServicoPesquisarDto>();

                    while (dr.Read())
                    {
                        retorno.Add(new OrdemServicoPesquisarDto
                        {
                            CpfPaciente = dr["CPFPACIENTE"].ToString(),
                            DataEntrada = dr["DATA_ENTRADA"].ToDateTime().ToShortDateString(),
                            NomePaciente = dr["CLIENTE"].ToString(),
                            NumeroOS = dr["ORDEMSERVICOID"].ToInt(),
                            Situacao = dr["SITUACAO"].ToString(),
                            Edita = dr["EDITA"].ToString()
                        });
                    }

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public OrdemServicoCarregarDto CarregarOrdemServico(int numeroOS, int usuarioLogadoID)
        {
            try
            {
                using (Cnn)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "proc_Web_CarregarOrdemServico",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@NumeroOS", numeroOS);
                    cmd.Parameters.AddWithValue("@PessoaID", usuarioLogadoID);

                    SqlDataReader dr = Cnn.LeDataReader(cmd);

                    OrdemServicoCarregarDto retorno = null;

                    if (dr.Read())
                    {
                        retorno = new OrdemServicoCarregarDto
                        {
                            ClienteId = dr["PESSOAID"].ToInt(),
                            ClienteNome = dr["NOME"].ToString(),
                            DataEntrada = dr["DATA_ENTRADA"].ToDateTime().ToShortDateString(),
                            NumeroOrdemServico = dr["ORDEMSERVICOID"].ToInt(),
                            Observacao = dr["OBS"].ToString(),
                            PacienteCpf = dr["CPFPACIENTE"].ToString(),
                            PacienteNome = dr["CLIENTE"].ToString(),
                            Edita = dr["EDITA"].ToBool()
                        };
                    }

                    if (retorno != null)
                    {
                        dr.NextResult();
                        var itens = new List<ItensOrdemSericoCarregarDto>();
                        while (dr.Read())
                        {
                            itens.Add(new ItensOrdemSericoCarregarDto
                            {
                                Quantidade = dr["QUANTIDADE"].ToInt(),
                                ServicoDescricao = dr["DESCRICAO"].ToString(),
                                ServicoId = dr["SERVICOID"].ToInt()
                            });
                        }
                        retorno.ItensOrdemServico = itens;
                    }

                    return retorno;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
