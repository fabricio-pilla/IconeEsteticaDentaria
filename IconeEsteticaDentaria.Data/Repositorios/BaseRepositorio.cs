using IconeEsteticaDentaria.Data.DAO;

namespace IconeEsteticaDentaria.Data.Repositorios
{
    public class BaseRepositorio
    {
        AcessoDados _cnn { get; set; }
        public AcessoDados Cnn
        {
            get
            {
                if (_cnn == null)
                    _cnn = new AcessoDados();
                return _cnn;
            }
            set
            {
                _cnn = value;
            }
        }
    }
}
