using System;
using System.Data;
using System.Data.SqlClient;

namespace IconeEsteticaDentaria.Data.DAO
{
    /// <summary>
    /// Todos os métodos desta classe abrem e fecham a conexão após o término do comando no banco de dados
    /// </summary>
    public class AcessoDados : DAOBase, IDisposable
    {
        #region atributos
        SqlConnection _conexao;
        SqlDataAdapter DataAd;
        DataSet DataS;
        SqlDataReader DataR;
        public bool EmTransacao = false;
        SqlTransaction Transacao;
        #endregion

        public AcessoDados()
        {
            _conexao = CriarConexao();
        }

        #region metodos publicos
        /// <summary>
        /// Recebe os comandos a serem executados e retorna uma tabela com os valores retornados do banco
        /// </summary>
        /// <param name="cmd">SQLCommand com a procedure ou demais comando a ser executado no banco de dados</param>
        /// <returns>DataTable com os valores retornados pelo banco de dados</returns>
        public DataTable LeDataTable(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = _conexao;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                DataS = new DataSet();
                if (EmTransacao) cmd.Transaction = Transacao;
                DataAd = new SqlDataAdapter(cmd);
                DataAd.Fill(DataS);
                if (!EmTransacao) cmd.Connection.Close();
                return DataS.Tables.Count > 0 ? DataS.Tables[0] : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (DataAd != null) DataAd.Dispose();
                DataS = null;
            }
        }
        /// <summary>
        /// Recebe o comando para ser executado no banco de dados e retorna um DataSet
        /// </summary>
        /// <param name="_Cmd">SqlCommand com os parâmetros a serem executados no banco</param>
        /// <returns>DataSet com os dados retornados do banco</returns>
        public DataSet LeDataSet(SqlCommand _Cmd)
        {
            try
            {
                _Cmd.Connection = _conexao;
                _Cmd.Connection.Open();
                DataS = new DataSet();
                DataAd = new SqlDataAdapter(_Cmd);
                DataAd.Fill(DataS);
                if (!EmTransacao) _Cmd.Connection.Close();
                DataAd = null;
                return DataS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (DataAd != null) DataAd.Dispose();
                DataS = null;
            }
        }
        /// <summary>
        /// Executa os comandos passados por parâmetro e retorna um SqlDataReader com os dados para serem percorridos na aplicação
        /// </summary>
        /// <param name="cmd">SqlCommand com os comandos para serem executados no banco para o retorno dos dados em formato de SqlDataReader</param>
        /// <returns>SqlDataReader com os dados para serem percorridos na aplicação</returns>
        public SqlDataReader LeDataReader(SqlCommand cmd)
        {
            try
            {
                DataR = null;
                cmd.Connection = _conexao;
                if (EmTransacao) cmd.Transaction = Transacao;
                if (cmd.Connection.State == ConnectionState.Closed) cmd.Connection.Open();
                DataR = cmd.ExecuteReader();
                return DataR;
            }
            catch (Exception ex)
            {
                if (DataR != null)
                {
                    DataR.Close();
                    DataR.Dispose();
                }
                throw ex;
            }
        }
        /// <summary>
        /// Executa um comando sql no banco de dados, essa deve ser um insert ou update ou ainda procedure ou function sem retorno do banco de dados
        /// </summary>
        /// <param name="cmd">SqlCommand que recebe os comandos para serem executados no banco</param>
        /// <returns>Int32 com o id inserido ou quantidade de registros retornados</returns>
        public Int32 ExecutaSql(SqlCommand cmd)
        {
            int Cont = 0;
            try
            {
                cmd.Connection = _conexao;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                if (EmTransacao) cmd.Transaction = Transacao;
                Cont = cmd.ExecuteNonQuery();
                if (!EmTransacao) cmd.Connection.Close();
                return Cont;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cont = 0;
            }
        }
        /// <summary>
        /// Executa comandos no banco que retornam um objeto para ser trabalhado no banco de dados
        /// </summary>
        /// <param name="cmd">SqlCommand com os comandos para serem executados no banco, estes podem retornar dados como select</param>
        /// <returns>Object com os dados retornados do banco</returns>
        public object ExecutaScalar(SqlCommand cmd)
        {
            object sResult = null;
            try
            {
                cmd.Connection = _conexao;
                if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                if (EmTransacao) cmd.Transaction = Transacao;
                sResult = cmd.ExecuteScalar();
                if (!EmTransacao) cmd.Connection.Close();
                return sResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sResult = null;
            }
        }
        /// <summary>
        /// Abre uma transação com o banco de dados e deixa a conexão aberta
        /// </summary>
        public void AbreTransacao()
        {
            try
            {
                if (_conexao.State == ConnectionState.Closed) _conexao.Open();
                Transacao = _conexao.BeginTransaction();
                EmTransacao = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Abre transação com o banco deixando a conexão aberta e indica o tipo de nível de isolamento
        /// </summary>
        /// <param name="TipoTransacao">IsolationLevel com o tipo de nível de isolação desejado</param>
        public void AbreTransacao(IsolationLevel TipoTransacao)
        {
            try
            {
                if (_conexao.State == ConnectionState.Closed) _conexao.Open();
                Transacao = _conexao.BeginTransaction(TipoTransacao);
                EmTransacao = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Fecha a transação e conexão com o banco de dados
        /// </summary>
        public void FechaTransacao()
        {
            try
            {
                if (EmTransacao) Transacao.Commit();
                EmTransacao = false;
                if (_conexao.State == ConnectionState.Open) _conexao.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Aborta a transação com o banco de dados executando um rollback e fechando a conexão.
        /// </summary>
        public void AbortaTransacao()
        {
            try
            {
                if (EmTransacao) Transacao.Rollback();
                EmTransacao = false;
                if (_conexao.State == ConnectionState.Open) _conexao.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            _conexao.Dispose();
        }

        public void FechaConexao()
        {
            try
            {
                if (_conexao.State == ConnectionState.Open) _conexao.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
