using IconeEsteticaDentaria.Comum.Extensions;
using IconeEsteticaDentaria.Domain;
using System;
using System.Data;
using System.Data.SqlClient;

namespace IconeEsteticaDentaria.Data.Repositorios
{
    public class UsuarioRepositorio : BaseRepositorio
    {
        public Usuario AutenticarUsuario(string login, string senha)
        {
            try
            {
                using (Cnn)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        CommandText = "sp_Usuario_Login",
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    SqlDataReader dr = Cnn.LeDataReader(cmd);

                    if (dr.Read())
                    {
                        return new Usuario
                        {
                            Id = dr["PESSOAID"].ToInt(),
                            Nome = dr["NOME"].ToString()                                                       
                        };
                    }
                    else
                        return null;
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
    }
}
